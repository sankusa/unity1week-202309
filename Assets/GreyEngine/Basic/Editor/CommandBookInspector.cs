using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using GreyEngine.Basic.TypeConversion;
using GreyEngine.Basic.Utils;
using GreyEngine.Basic.EditorResources;

namespace GreyEngine.Basic {
    [CustomEditor(typeof(CommandBook))]
    public class CommandBookInspector : Editor
    {
        // -------- 編集or参照するデータ --------
        // コマンドデータベース
        private CommandDatabase database = new CommandDatabase();
        // ブック
        CommandBook book;
        // コマンド
        List<Command> commands;
        // ブック変数
        List<Variable> variables;
        // -------- 編集or参照するデータ --------

        // --------GUI関係 --------
        // エディタサイズ編集表示設定
        private bool editorSettingVisible = false;
        // コマンドリスト関係
        private ReorderableList commandList;
        private Vector2 listScrollPos;
        private int commandIndentLevel = 0;
        private int commandIndentWidth = 10;
        private int rowIconWidth = 20;
        private Color listBackgroundColor = Color.black;
        // コマンド本体関係
        private CommandEditor commandEditor;
        private GenericMenu addMenu;
        private GenericMenu insertMenu;
        private Vector2 commandScrollPos;
        // 変数関係
        private ReorderableList variableList;
        private Vector2 variableScrollPos;
        // 各種表示サイズ設定
        [System.NonSerialized] private int commandListHeight = 300;
        [System.NonSerialized] private int tagWidth = 100;
        [System.NonSerialized] private int commandHeight = 300;
        [System.NonSerialized] private int variableHeight = 200;

        private SerializedProperty commandsProp;
        private SerializedProperty variablesProp;

        private bool commandInserted = false;

        // --------GUI関係 --------
        
        void Awake() {
            // データベース更新時の処理を登録
            CommandRegisterWindow.onCommandDatabaseSaved += OnCommandDatabaseSaved;
        }

        void OnDestroy() {
            // データベース更新時の処理を削除
            CommandRegisterWindow.onCommandDatabaseSaved -= OnCommandDatabaseSaved;
        }
        
        void OnEnable() {
            commandsProp = serializedObject.FindProperty("commands");
            variablesProp = serializedObject.FindProperty("variables");

            // ブック
            book = target as CommandBook;
            // コマンド
            commands = book.commands;
            // 変数
            variables = book.variables;
            // コマンドリスト初期化
            commandList = new ReorderableList(commands, typeof(Command), true, true, false, false);
            commandList = new ReorderableList(serializedObject, commandsProp, true, true, false, false);
            commandList.drawHeaderCallback += DrawCommandHeader;
            commandList.drawElementCallback += DrawCommandElement;
            commandList.drawElementBackgroundCallback += DrawElementBackground;
            commandList.onSelectCallback += OnCommandSelect;
            commandList.onChangedCallback += OnCommandChanged;
            // 変数リスト初期化
            //variableList = new ReorderableList(variables, typeof(Variable), true, true, true, true);
            variableList = new ReorderableList(serializedObject, variablesProp, true, true, true, true);
            variableList.drawHeaderCallback += DrawVariableHeader;
            variableList.drawElementCallback += DrawVariableElement;
            variableList.onAddDropdownCallback += OnAddVariableDropdown;
            // コマンドデータベースをロード
            LoadDatabase();
        }

        public override void OnInspectorGUI() {
            // リスト挿入時、OnCommandSelectが自動では呼び出されないため、強制的に呼び出し
            if(commandInserted) {
                OnCommandSelect(commandList);
                commandInserted = false;
            }

            serializedObject.Update();

            // 変数初期化
            commandIndentLevel = 0;

            // -------- エディタサイズ編集フィールド表示開始 --------
            editorSettingVisible = EditorGUILayout.Foldout(editorSettingVisible, "エディタ設定を表示");
            if(editorSettingVisible == true) {
                commandListHeight = EditorGUILayout.IntField("List Height", commandListHeight);
                tagWidth = EditorGUILayout.IntField("Tag Width", tagWidth);
                commandHeight = EditorGUILayout.IntField("Command Height", commandHeight);
                variableHeight = EditorGUILayout.IntField("Variable Height", variableHeight);
            }
            // -------- エディタサイズ編集フィールド表示開始 --------

            // -------- コマンドリスト表示開始 --------
            listScrollPos = EditorGUILayout.BeginScrollView(listScrollPos, GUILayout.Height(commandListHeight));
            // リスト表示
            commandList.DoLayoutList();
            EditorGUILayout.EndScrollView();
            // -------- コマンドリスト表示終了 --------

            // 区切り線
            EditorUtil.LayoutLine(1, Color.black);

            using (new EditorGUILayout.HorizontalScope()) {
                // スペース
                GUILayout.FlexibleSpace();
                // 追加ボタン
                if(GUILayout.Button("追加", GUILayout.Width(40))) {
                    addMenu.ShowAsContext();
                }
                // 挿入ボタン
                if(GUILayout.Button("挿入", GUILayout.Width(40))) {
                    insertMenu.ShowAsContext();
                }
                // 削除ボタン
                if(GUILayout.Button(EditorIcons.Instance.minusIcon, GUILayout.Width(40))) {
                    // if(commandList.index > -1) commands.RemoveAt(commandList.index);
                    if(commandList.index > -1) commandsProp.DeleteArrayElementAtIndex(commandList.index);
                    if(commandList.index >= commandsProp.arraySize) commandList.index = commandsProp.arraySize - 1;
                    commandEditor = null;
                    EditorUtility.SetDirty(book);
                }
                // コピーボタン
                if(GUILayout.Button(EditorIcons.Instance.copyIcon, GUILayout.Width(40))) {
                    //if(commandList.index > -1) commands.Insert(commandList.index + 1, new Command(commands[commandList.index]));
                    //EditorUtility.SetDirty(book);
                    if(commandList.index > -1) commandsProp.InsertArrayElementAtIndex(commandList.index + 1);
                    PasteCommandProp(commandsProp.GetArrayElementAtIndex(commandList.index + 1), commands[commandList.index]);
                }
            }
            // -------- コマンド編集表示開始 ----
            commandScrollPos = EditorGUILayout.BeginScrollView(commandScrollPos, GUILayout.Height(commandHeight));

            // コマンドエディタ
            if(commandEditor != null) {
                if(commandList.index < commandList.count) {
                    commandEditor.Draw(database, variables);
                }
            }
            EditorGUILayout.EndScrollView();
            // -------- コマンド編集表示終了 ----

            // 区切り線
            EditorUtil.LayoutLine(0.5f, Color.white);

            // -------- 変数表示開始 --------
            variableScrollPos = EditorGUILayout.BeginScrollView(variableScrollPos, GUILayout.Height(variableHeight));
            variableList.DoLayoutList();
            EditorGUILayout.EndScrollView();
            // -------- 変数表示終了 --------

            // 変更を保存
            serializedObject.ApplyModifiedProperties();
        }
        
        // コマンドデータベースをロード
        private void LoadDatabase() {
            // ロード
            database = CommandDatabase.LoadDatabase();
            // 追加ボタン用メニュー作成
            addMenu = new GenericMenu();
            foreach(CommandData data in database.datas) {
                // 「All/分類/名前空間/クラス名/メソッド」
                addMenu.AddItem(new GUIContent(data.GetPath()), on : false, func : () => {
                        serializedObject.Update();
                        // commands.Add(data.CreateCommand());
                        commandsProp.InsertArrayElementAtIndex(commandsProp.arraySize);
                        PasteCommandProp(commandsProp.GetArrayElementAtIndex(commandsProp.arraySize - 1), data.CreateCommand());
                        // EditorUtility.SetDirty(book);
                        serializedObject.ApplyModifiedProperties();
                    });
                // CommandDataに設定したパス
                if(data.customMenuPath != "") {
                    addMenu.AddItem(new GUIContent(data.customMenuPath), on : false, func : () => {
                            serializedObject.Update();
                            // commands.Add(data.CreateCommand());
                            commandsProp.InsertArrayElementAtIndex(commandsProp.arraySize);
                            PasteCommandProp(commandsProp.GetArrayElementAtIndex(commandsProp.arraySize - 1), data.CreateCommand());
                            // EditorUtility.SetDirty(book);
                            serializedObject.ApplyModifiedProperties();
                        });
                }
            }
            // 挿入ボタン用メニュー作成
            insertMenu = new GenericMenu();
            foreach(CommandData data in database.datas) {
                // 「All/分類/名前空間/クラス名/メソッド」
                insertMenu.AddItem(new GUIContent(data.GetPath()), on : false, func : () => {
                        if(commandList.index > -1) {
                            serializedObject.Update();
                            // commands.Insert(commandList.index, data.CreateCommand());
                            commandsProp.InsertArrayElementAtIndex(commandList.index);
                            PasteCommandProp(commandsProp.GetArrayElementAtIndex(commandList.index), data.CreateCommand());
                            // EditorUtility.SetDirty(book);
                            serializedObject.ApplyModifiedProperties();
                            commandInserted = true;
                        }
                    });
                // CommandDataに設定したパス
                if(data.customMenuPath != "") {
                    insertMenu.AddItem(new GUIContent(data.customMenuPath), on : false, func : () => {
                            if(commandList.index > -1) {
                                serializedObject.Update();
                                // commands.Insert(commandList.index, data.CreateCommand());
                                commandsProp.InsertArrayElementAtIndex(commandList.index);
                                PasteCommandProp(commandsProp.GetArrayElementAtIndex(commandList.index), data.CreateCommand());
                                // EditorUtility.SetDirty(book);
                                serializedObject.ApplyModifiedProperties();
                                commandInserted = true;
                            }
                        });
                }
            }
        }

        // データベース更新時コールバック関数
        private void OnCommandDatabaseSaved() {
            LoadDatabase();
            if(commandList.index >= 0) {
                commandEditor = new CommandEditor(commands[commandList.index], commandsProp.GetArrayElementAtIndex(commandList.index));
            }
        }

        // コマンドリストヘッダ描画時コールバック関数
        private void DrawCommandHeader(Rect rect) {
            EditorGUI.LabelField(rect, "コマンド");
        }

        // コマンドリスト背景描画時コールバック関数
        private void DrawElementBackground(Rect rect, int index, bool isActive, bool isFocused) {
            // index = -1 の場合がある(理由は不明)ため、その場合は処理を抜ける
            if(index < 0)return;
            Command command = commands[index];
            CommandData data = database.FindCommandData(command);
            // アイコン分のインデント
            Rect fixedRect = new Rect(rect.x + rowIconWidth, rect.y, rect.width - rowIconWidth, rect.height);
            // EditorUtil.Box(rect, listBackgroundColor);
            EditorUtil.Box(fixedRect, new Color(index % 2, index % 2, index % 2, 0.05f));
            // 通常の背景(CommandDataに設定された色)
            if(data != null && data.color.a != 0) {
                //EditorUtil.Box(fixedRect, new Color(0.25f, 0.25f, 0.25f, 1f));
                EditorUtil.Box(fixedRect, data.color);
            }
            // フォーカス時
            if(isFocused) {
                EditorUtil.Box(fixedRect, Color.black);
            }
        }
        // コマンドリスト要素描画時コールバック関数
        private void DrawCommandElement(Rect rect, int index, bool isActive, bool isFocused) {
            // 対象のコマンド
            Command command = commands[index];

            // -------- インデント計算開始 --------
            // EndIfの場合、この要素から1段上げる
            if(command.className == typeof(CommandBookReader).FullName && command.methodName == "EndIf") {
                commandIndentLevel--;
                if(commandIndentLevel < 0) commandIndentLevel = 0;
            }
            // インデント幅
            int indentWidth = commandIndentLevel * commandIndentWidth;
            //Ifの場合、次の要素から1段下げる
            if(command.className == typeof(CommandBookReader).FullName && command.methodName == "If") {
                commandIndentLevel++;
            }
            // -------- インデント計算終了 --------

            // -------- 各GUIの表示領域計算開始 --------
            // インデント用Box表示領域
            Rect indentRect = new Rect(rect.x, rect.y, indentWidth, rect.height);
            // サマリ表示領域
            Rect summaryRect = new Rect(rect.x + indentWidth, rect.y, rect.width - indentWidth, rect.height);
            // タグ表示領域
            Rect tagRect = new Rect(rect.xMax - tagWidth, rect.yMin, tagWidth, rect.height / 2);
            // WaitUntil表示領域
            Rect waitUntilRect = new Rect(rect.xMax - tagWidth, rect.yMin + rect.height / 2, tagWidth, rect.height / 2);
            // -------- 各GUIの表示領域計算終了 --------
            
            // -------- サマリ生成開始 --------
            CommandData data = database.FindCommandData(command);
            string summary = "";
            if(data != null) {
                summary = data.CreateSummary(command);
            } else {
                data = CommandDatabase.allData.FindCommandData(command);
                if(data != null) {
                    EditorUtil.Box(rect, new Color(0.3f, 0.3f, 0.2f));
                    summary = data.CreateSummary(command);
                } else {
                    EditorUtil.Box(rect, new Color(0.3f, 0f,0f));
                }
            }
            // 戻り値を変数に保存する場合、">> 変数名"を追加
            if(command.returnSaveVariableName != "") {
                summary += "<color=lime>  >>  " + command.returnSaveVariableName + "</color>";
            }
            // -------- サマリ生成終了 --------

            // -------- GUI表示開始 --------
            if(indentWidth != 0) {
                EditorUtil.Box(indentRect, "", listBackgroundColor);
            }
            EditorGUI.LabelField(summaryRect, summary, EditorUtil.CreateRichTextStyle(GUI.skin.label));
            if(command.executeStop) {
                Rect lineRect = new Rect(rect.x + indentWidth, rect.y + rect.height - 2f, (rect.width - indentWidth) / 2f, 2);
                EditorUtil.Box(lineRect, new Color(0.4f, 0.8f, 0.4f));
            }
            if(command.tag != "") {
                EditorUtil.Box(tagRect, new Color(0.1f, 0.3f, 0.1f));
                EditorGUI.LabelField(tagRect, "<size=11>Tag・・・"+command.tag+"</size>", EditorUtil.CreateRichTextStyle(GUI.skin.label));
            }
            if(command.waitUntil) {
                EditorUtil.Box(waitUntilRect, new Color(0.2f, 0.4f, 0.2f));
                EditorGUI.LabelField(waitUntilRect, "<size=11>WaitUntil・・・"+command.waitConditionValueString+"</size>", EditorUtil.CreateRichTextStyle(GUI.skin.label));
            }
            // -------- GUI表示終了 --------
        }

        // コマンド選択時コールバック関数
        private void OnCommandSelect(ReorderableList list) {
            commandEditor = new CommandEditor(commands[list.index], commandsProp.GetArrayElementAtIndex(list.index));
        }

        // リスト変更時コールバック関数
        private void OnCommandChanged(ReorderableList list) {
            EditorUtility.SetDirty(book);
        }

        // 変数リストヘッダ描画時コールバック関数
        private void DrawVariableHeader(Rect rect) {
            EditorGUI.LabelField(rect, "ブック変数");
        }

        // 変数リスト要素表示コールバック関数
        private void DrawVariableElement(Rect rect, int index, bool isActive, bool isFocused) {
            Variable target = variables[index];
            TypeConverter converter = UtilsForEditor.MasterConverter.GetConverter(target.typeName);
            Rect typeRect = new Rect(rect.x, rect.y, 60, rect.height);
            Rect nameRect = new Rect(typeRect.xMax, rect.y, 80, rect.height);
            Rect valueRect = new Rect(nameRect.xMax + 2, rect.y, rect.width - 140, rect.height);

            EditorGUI.LabelField(typeRect, converter.SimpleTypeName);
            // target.name = EditorGUI.TextField(nameRect, target.name);
            variablesProp.GetArrayElementAtIndex(index).FindPropertyRelative("name").stringValue = EditorGUI.TextField(nameRect, target.name);
            // target.valueString = converter.Field(valueRect, "", target.valueString);
            variablesProp.GetArrayElementAtIndex(index).FindPropertyRelative("valueString").stringValue = converter.Field(valueRect, "", target.valueString);
        }
        
        // 変数プラスボタン押下時コールバック関数
        private void OnAddVariableDropdown(Rect menuRect, ReorderableList list) {
            GenericMenu menu = new GenericMenu();
            foreach(TypeConverter converter in UtilsForEditor.MasterConverter.converters) {
                menu.AddItem(new GUIContent(converter.SimpleTypeName), on : false, func : () => {
                    // variables.Add(new Variable("", converter.Type.FullName, converter.InitialString));
                    serializedObject.Update();
                    variablesProp.InsertArrayElementAtIndex(variablesProp.arraySize);
                    variablesProp.GetArrayElementAtIndex(variablesProp.arraySize - 1).FindPropertyRelative("name").stringValue = "";
                    variablesProp.GetArrayElementAtIndex(variablesProp.arraySize - 1).FindPropertyRelative("typeName").stringValue = converter.Type.FullName;
                    variablesProp.GetArrayElementAtIndex(variablesProp.arraySize - 1).FindPropertyRelative("valueString").stringValue = converter.InitialString;
                    serializedObject.ApplyModifiedProperties();
                });
            }
            menu.DropDown(menuRect);
        }

        public void PasteCommandProp(SerializedProperty commandProp, Command command) {
            commandProp.FindPropertyRelative("category").intValue = (int) command.category;
            commandProp.FindPropertyRelative("className").stringValue = command.className;
            commandProp.FindPropertyRelative("methodName").stringValue = command.methodName;
            SerializedProperty argsTypeNamesProp = commandProp.FindPropertyRelative("argsTypeNames");
            argsTypeNamesProp.ClearArray();
            foreach(string argTypeName in command.argsTypeNames) {
                argsTypeNamesProp.InsertArrayElementAtIndex(argsTypeNamesProp.arraySize);
                argsTypeNamesProp.GetArrayElementAtIndex(argsTypeNamesProp.arraySize - 1).stringValue = argTypeName;
            }
            SerializedProperty argValueStringsProp = commandProp.FindPropertyRelative("argValueStrings");
            argValueStringsProp.ClearArray();
            foreach(string argValueString in command.argValueStrings) {
                argValueStringsProp.InsertArrayElementAtIndex(argValueStringsProp.arraySize);
                argValueStringsProp.GetArrayElementAtIndex(argValueStringsProp.arraySize - 1).stringValue = argValueString;
            }
            SerializedProperty argVariableUseFlagsProp = commandProp.FindPropertyRelative("argVariableUseFlags");
            argVariableUseFlagsProp.ClearArray();
            foreach(bool argVariableUseFlag in command.argVariableUseFlags) {
                argVariableUseFlagsProp.InsertArrayElementAtIndex(argVariableUseFlagsProp.arraySize);
                argVariableUseFlagsProp.GetArrayElementAtIndex(argVariableUseFlagsProp.arraySize - 1).boolValue = argVariableUseFlag;
            }
            SerializedProperty argVariableNamesProp = commandProp.FindPropertyRelative("argVariableNames");
            argVariableNamesProp.ClearArray();
            foreach(string argVariableName in command.argVariableNames) {
                argVariableNamesProp.InsertArrayElementAtIndex(argVariableNamesProp.arraySize);
                argVariableNamesProp.GetArrayElementAtIndex(argVariableNamesProp.arraySize - 1).stringValue = argVariableName;
            }
            commandProp.FindPropertyRelative("returnTypeName").stringValue = command.returnTypeName;
            commandProp.FindPropertyRelative("returnSaveVariableName").stringValue = command.returnSaveVariableName;
            commandProp.FindPropertyRelative("executeStop").boolValue = command.executeStop;
            commandProp.FindPropertyRelative("tag").stringValue = command.tag;
            commandProp.FindPropertyRelative(nameof(command.waitUntil)).boolValue = command.waitUntil;
            commandProp.FindPropertyRelative(nameof(command.waitConditionValueString)).stringValue = command.waitConditionValueString;
        }
    }
}