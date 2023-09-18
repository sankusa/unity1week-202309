using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GreyEngine.Basic.Utils;
using GreyEngine.Basic.TypeConversion;
using System;
using System.Linq;

namespace GreyEngine.Basic {
    // Commandインスタンス編集用GUI
    public class CommandEditor
    {
        Command command;
        SerializedProperty commandProp;
        // 見出し
        GUIStyle titleStyle = EditorUtil.CreateColoredStyle(GUI.skin.box, new Color(0.0f, 0.0f, 0.0f));
        // 引数→変数の場合
        GUIStyle variableStyle = EditorUtil.CreateColoredStyle(GUI.skin.box, new Color(0.3f, 0.1f, 0.3f));
        // オプション有効時
        GUIStyle activeStyle = EditorUtil.CreateColoredStyle(GUI.skin.box, new Color(0.1f, 0.3f, 0.1f));

        public CommandEditor(Command command, SerializedProperty commandProp) {
            this.command = command;
            this.commandProp = commandProp;
        }

        public void Draw(CommandDatabase database, List<Variable> variables) {
            // 0 登録済み
            // 1 未登録だが使用可能(非推奨)
            // 2 使用不能
            int registered = 0;
            if(database.FindCommandData(command) != null) {
                registered = 0;
            } else if(CommandDatabase.allData.FindCommandData(command) != null) {
                registered = 1;
            } else {
                registered = 2;
            }

            using (new EditorGUILayout.VerticalScope(EditorUtil.CreateColoredStyle(GUI.skin.box, new Color(0.2f, 0.2f, 0.2f)))) {
                using (new EditorGUILayout.HorizontalScope()) {
                    // コマンド分類表示
                    if(command.category == CommandCategory.Normal) {
                        EditorGUILayout.LabelField("通常コマンド");
                    } else if(command.category == CommandCategory.BookControl) {
                        EditorGUILayout.LabelField("ブック操作コマンド");
                    } else {
                        EditorGUILayout.LabelField(command.category.ToString());
                    }
                    // 注意コメント
                    if(registered == 1) {
                        EditorUtil.LayoutBox("未登録コマンド(実行可能)", new Color(0.5f, 0.5f, 0f));
                    } else if(registered == 2) {
                        EditorUtil.LayoutBox("存在しないコマンド(実行不可)", new Color(0.5f, 0f, 0f));
                    }
                }

                using (new EditorGUILayout.VerticalScope()) {
                    EditorGUILayout.LabelField(command.className + "->" + command.methodName, titleStyle);
                    // 引数フィールド
                    for(int i = 0; i < command.argsTypeNames.Count; i++) {
                        // ラベル作成
                        string label = "";
                        if(registered == 0) {
                            CommandData data = database.FindCommandData(command);
                            TypeConverter converter = UtilsForEditor.MasterConverter.GetConverter(data.argTypeNames[i]);
                            label = converter.SimpleTypeName + "  " + data.argNames[i];
                        } else if(registered == 1) {
                            CommandData data = CommandDatabase.allData.FindCommandData(command);
                            TypeConverter converter = UtilsForEditor.MasterConverter.GetConverter(data.argTypeNames[i]);
                            label = converter.SimpleTypeName + "  " + data.argNames[i];
                        } else if(registered == 2) {
                            label = "?";
                        }
                        // 表示
                        using (new EditorGUILayout.HorizontalScope()) {
                            using (new EditorGUILayout.HorizontalScope(command.argVariableUseFlags[i] ? variableStyle : GUI.skin.box)) {
                                // command.argVariableUseFlags[i] = EditorGUILayout.Toggle(command.argVariableUseFlags[i], GUI.skin.button, GUILayout.Width(18), GUILayout.Height(18));
                                commandProp.FindPropertyRelative("argVariableUseFlags").GetArrayElementAtIndex(i).boolValue = EditorGUILayout.Toggle(command.argVariableUseFlags[i], GUI.skin.button, GUILayout.Width(18), GUILayout.Height(18));
                                if(command.argVariableUseFlags[i]) {
                                    List<string> variableNames = variables.Where(x => x.typeName == command.argsTypeNames[i]).Select(x => x.name).ToList();
                                    int selectIndex = variableNames.IndexOf(command.argVariableNames[i]);
                                    if(selectIndex == -1) selectIndex = 0;
                                    selectIndex = EditorGUILayout.Popup(label + "(変数)", selectIndex, variableNames.ToArray());
                                    commandProp.FindPropertyRelative("argVariableNames").GetArrayElementAtIndex(i).stringValue = variableNames[selectIndex];
                                    // command.argVariableNames[i] = EditorGUILayout.TextField(label + "(変数)", command.argVariableNames[i]);
                                    //commandProp.FindPropertyRelative("argVariableNames").GetArrayElementAtIndex(i).stringValue = EditorGUILayout.TextField(label + "(変数)", command.argVariableNames[i]);
                                } else {
                                    // command.argValueStrings[i] = UtilsForEditor.MasterConverter.GetConverter(command.argsTypeNames[i]).FieldLayout(label, command.argValueStrings[i]);
                                    commandProp.FindPropertyRelative("argValueStrings").GetArrayElementAtIndex(i).stringValue = UtilsForEditor.MasterConverter.GetConverter(command.argsTypeNames[i]).FieldLayout(label, command.argValueStrings[i]);
                                }
                            }
                        }
                    }
                }
                // オプション
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("オプション", titleStyle);
                using (new EditorGUILayout.VerticalScope()) {
                    // 停止フラグ
                    using (new EditorGUILayout.HorizontalScope(command.executeStop ? activeStyle : GUI.skin.box)) {
                        //command.executeStop = EditorGUILayout.Toggle("実行後に停止", command.executeStop);
                        commandProp.FindPropertyRelative("executeStop").boolValue = EditorGUILayout.Toggle("実行後に停止", command.executeStop);
                    }
                    // タグ
                    using (new EditorGUILayout.HorizontalScope(command.tag != "" ? activeStyle : GUI.skin.box)) {
                        // command.tag = EditorGUILayout.TextField("タグ", command.tag);
                        commandProp.FindPropertyRelative("tag").stringValue = EditorGUILayout.TextField("タグ", command.tag);
                    }
                    // 戻り値
                    if(UtilsForEditor.MasterConverter.isConvertibleType(command.returnTypeName)) {
                        using (new EditorGUILayout.HorizontalScope(command.returnSaveVariableName != "" ? activeStyle : GUI.skin.box)) {
                            string simpleTypeName = UtilsForEditor.MasterConverter.GetConverter(command.returnTypeName).SimpleTypeName;
                            // command.returnSaveVariableName = EditorGUILayout.TextField("戻り値保存(" + simpleTypeName + ")", command.returnSaveVariableName);
                            commandProp.FindPropertyRelative("returnSaveVariableName").stringValue = EditorGUILayout.TextField("戻り値保存(" + simpleTypeName + ")", command.returnSaveVariableName);
                        }
                        using (new EditorGUILayout.VerticalScope(command.waitUntil ? activeStyle : GUI.skin.box)) {
                            commandProp.FindPropertyRelative("waitUntil").boolValue = EditorGUILayout.Toggle("WaitUntil", command.waitUntil);
                            if(command.waitUntil) {
                                try {
                                    commandProp.FindPropertyRelative("waitConditionValueString").stringValue = UtilsForEditor.MasterConverter.GetConverter(command.returnTypeName).FieldLayout("比較値", command.waitConditionValueString);
                                } catch(FormatException) {
                                    commandProp.FindPropertyRelative("waitConditionValueString").stringValue = UtilsForEditor.MasterConverter.GetConverter(command.returnTypeName).InitialString;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}