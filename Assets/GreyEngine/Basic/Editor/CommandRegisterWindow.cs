using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GreyEngine.Basic;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditorInternal;
using GreyEngine.Basic.Utils;
using GreyEngine.Basic.EditorResources;
public class CommandRegisterWindow : EditorWindow
{
    // 保存時コールバック
    public static event Action onCommandDatabaseSaved;
    [SerializeField] private CommandTable table;
    private List<CommandData> allCommands;
    private CommandDatabase database;
    // GUI
    private ReorderableList commandList;
    private Vector2 scrollPos;
    private CommandCategory targetCategory = CommandCategory.Normal;
    // テーブル情報
    private List<string> tablePaths;
    private List<string> tableLabels;
    private string activeTablePath = "";
    private string oldTablePath = "";
    // GUIサイズ設定
    [NonSerialized] int categoryWidth = 60;
    [NonSerialized] int classWidth = 200;
    [NonSerialized] int methodWidth = 350;
    [NonSerialized] int summaryWidth = 200;
    [NonSerialized] int pathWisth = 200;
    [NonSerialized] int colorWidth = 40;

    GenericMenu commandMenu;
    [MenuItem("Tools/GreyEngine/コマンド登録")]
    private static void ShowWindow() {
        GetWindow<CommandRegisterWindow>("コマンド登録");
    }

    void OnEnable() {
        // データベース取得
        database = CommandDatabase.LoadDatabase();
        // パスリスト取得
        LoadPathList();
        // アクティブパス名がリストにない場合、リストの0番目をアクティブに
        if(tablePaths.IndexOf(activeTablePath) == -1) activeTablePath = tablePaths[0];
        // アクティブテーブル名が変わっていたらテーブル再ロード
        LoadTable();
        
        // 全コマンド取得
        allCommands = CommandDataUtil.GetCommands(
            t => {return /*t.IsSubclassOf(typeof(MonoBehaviour)) &&*/ !t.IsAbstract;},
            m => {return m.ReflectedType.Equals(m.DeclaringType) && m.IsPublic && !m.IsStatic && !m.IsVirtual &&!m.IsAbstract && !m.IsConstructor;}
        );
        // 追加メニュー
        UpdateCommandMenu();
    }

    // ファイル名が変更された場合、パスリスト再ロード
    void OnProjectChange() {
        LoadPathList();
    }

    void OnGUI() {
        int pathIndex = tablePaths.IndexOf(activeTablePath);
        if(pathIndex == -1) pathIndex = 0;
        pathIndex = EditorGUILayout.Popup(pathIndex, tableLabels.ToArray());
        activeTablePath = tablePaths[pathIndex];
        // アクティブパス名が前フレームと変わっていたらテーブル再ロード
        if(activeTablePath != oldTablePath) LoadTable();

        // コマンド分類設定
        EditorGUILayout.LabelField("作成するコマンドの分類");
        targetCategory = (CommandCategory)EditorGUILayout.EnumPopup(targetCategory);

        
        // スクロール開始
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        commandList.DoLayoutList();

        // スクロール終了
        EditorGUILayout.EndScrollView();

        EditorUtil.LayoutLine(1, Color.white);

        using (new EditorGUILayout.HorizontalScope()) {
            // 保存ボタン
            if(GUILayout.Button("保存", GUILayout.Width(40))) {
                EditorUtility.SetDirty(table);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                onCommandDatabaseSaved?.Invoke();
            }
            // スペース
            GUILayout.FlexibleSpace();
            // 追加ボタン
            if(GUILayout.Button(EditorIcons.Instance.plusIcon, GUILayout.Width(40))) {
                commandMenu.ShowAsContext();
            }
            // 削除ボタン
            if(GUILayout.Button(EditorIcons.Instance.minusIcon, GUILayout.Width(40))) {
                if(commandList.index > -1) table.datas.RemoveAt(commandList.index);
                if(commandList.index >= table.datas.Count) commandList.index = table.datas.Count - 1;
                OnTableChange();
            }
        }

        // テーブルのパスを記録
        oldTablePath = activeTablePath;
    }

    // プロジェクト内全テーブルのパス取得
    private void LoadPathList() {
        tablePaths = AssetUtil.GetAssetPaths<CommandTable>();
        tableLabels = CreateTableLabels(tablePaths);
    }

    private void LoadTable() {
        // テーブルをロード
        table = AssetDatabase.LoadAssetAtPath<CommandTable>(activeTablePath);
        table.datas.Sort(new CommandDataComparer());
        // リスト作成
        commandList = new ReorderableList(table.datas, typeof(CommandTable), false, true, false, false);
        commandList.drawHeaderCallback += DrawCommandHeader;
        commandList.drawElementCallback += DrawCommandElement;
    }

    private void UpdateCommandMenu() {
        // 追加メニュー
        commandMenu = new GenericMenu();
        foreach(CommandData data in allCommands) {
            // テーブルに存在していなければ
            if(!database.Exists(data)) {
                commandMenu.AddItem(new GUIContent(data.GetPath()), on : false, func : () => {
                        CommandData additionalData = new CommandData(data);
                        additionalData.category = targetCategory;
                        table.datas.Add(additionalData);
                        table.datas.Sort(new CommandDataComparer());
                        OnTableChange();
                    });
            }
        }
    }
    // コマンドリストヘッダ描画時
    private void DrawCommandHeader(Rect rect) {
        // カテゴリ表示サイズ
        Rect categoryRect = new Rect(rect);
        categoryRect.width = categoryWidth;
        // クラス名表示サイズ
        Rect classRect = new Rect(categoryRect.xMax, categoryRect.y, classWidth, categoryRect.height);
        // メソッド表示サイズ
        Rect methodRect = new Rect(classRect.xMax, classRect.y, methodWidth, classRect.height);
        // サマリー入力サイズ
        Rect summaryRect = new Rect(methodRect.xMax, methodRect.y, summaryWidth, methodRect.height);
        // カスタムメニューパス入力サイズ
        Rect pathRect = new Rect(summaryRect.xMax, summaryRect.y, pathWisth, summaryRect.height);
        // 表示色入力サイズ
        Rect colorRect = new Rect(pathRect.xMax, pathRect.y, colorWidth, pathRect.height);

        using(new EditorGUILayout.HorizontalScope()) {
            EditorGUI.LabelField(categoryRect, "分類");
            EditorGUI.LabelField(classRect, "クラス");
            EditorGUI.LabelField(methodRect, "メソッド");
            EditorGUI.LabelField(summaryRect, "サマリ");
            EditorGUI.LabelField(pathRect, "メニューパス");
            EditorGUI.LabelField(colorRect, "表示色");
        }
    }
    // コマンドリスト要素描画時
    private void DrawCommandElement(Rect rect, int index, bool isActive, bool isFocused) {
        if(table.datas[index].category == CommandCategory.Normal) {
            if(!CommandDatabase.allData.Exists(table.datas[index])) {
                EditorUtil.Box(rect, new Color(0.5f, 0f, 0f));
            }
        } else if(table.datas[index].category == CommandCategory.BookControl) {
            EditorUtil.Box(rect, new Color(0f, 0.3f, 0.3f));
        }

        // カテゴリ表示サイズ
        Rect categoryRect = new Rect(rect);
        categoryRect.width = categoryWidth;
        // クラス名表示サイズ
        Rect classRect = new Rect(categoryRect.xMax, categoryRect.y, classWidth, categoryRect.height);
        // メソッド表示サイズ
        Rect methodRect = new Rect(classRect.xMax, classRect.y, methodWidth, classRect.height);
        // サマリー入力サイズ
        Rect summaryRect = new Rect(methodRect.xMax, methodRect.y, summaryWidth, methodRect.height);
        // カスタムメニューパス入力サイズ
        Rect pathRect = new Rect(summaryRect.xMax, summaryRect.y, pathWisth, summaryRect.height);
        // 表示色入力サイズ
        Rect colorRect = new Rect(pathRect.xMax, pathRect.y, colorWidth, pathRect.height);

        using (new EditorGUILayout.HorizontalScope()) {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.EnumPopup(categoryRect, table.datas[index].category);
            EditorGUI.TextField(classRect, table.datas[index].className);
            EditorGUI.TextField(methodRect, table.datas[index].GetMethodLabel());
            EditorGUI.EndDisabledGroup();
            // 入力可能データ
            EditorGUI.BeginChangeCheck();
            table.datas[index].summary = EditorGUI.TextField(summaryRect, table.datas[index].summary);
            table.datas[index].customMenuPath = EditorGUI.TextField(pathRect, table.datas[index].customMenuPath);
            table.datas[index].color = EditorGUI.ColorField(colorRect, table.datas[index].color);
            if(EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(table);
            }
        }
    }

    private void OnTableChange() {
        UnityEditor.Undo.RecordObject(table, "Edit CommandTable");
        EditorUtility.SetDirty(table);
        database = CommandDatabase.LoadDatabase();
        UpdateCommandMenu();
    }

    private List<string> CreateTableLabels(List<string> paths) {
        List<string> labels = new List<string>();
        foreach (string s in paths) {
            labels.Add(s.Replace("/", " " + '\u2215' + " "));
        }
        return labels;
    }
}
