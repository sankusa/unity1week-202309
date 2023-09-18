using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using GreyEngine.Basic.TypeConversion;
using Zenject;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace GreyEngine.Basic {
    // 仕様
    // 「インデックスのコマンドを実行し、インデックスを1加算」を繰り返す。
    // 停止フラグ付きのコマンドまで実行する。
    // 停止後、インデックスは最後に実行したコマンドの次のインデックスの値になる。
    // ロックはコマンド及び外部から行われ、runningの値は維持したまま処理を止める。
    // Run関数で実行する際、ロック状態はリセットされる。
    public class CommandBookReader : MonoBehaviour {
        [Inject] DiContainer container;
        [SerializeField] private List<CommandBook> books;
        private CommandBook currentBook = null;
        private int index = 0;
        private bool running = false;
        public bool Running => running;
        private bool locked = false;
        // スリープ用
        private float sleepTime = 0f;
        // If関数用
        private bool ifSkip = false;
        private int ifSkippingIndentLevel = 0;
        // 切り替え用
        private bool indexIncrementSkip = false;

        private MasterTypeConverter masterConverter;

        void Awake() {
            masterConverter = MasterTypeConverter.CreateInstance();
            currentBook = books.Count > 0 ? books[0] : null;
        }

        IEnumerator Start() {
            // 1フレームに1回は抜ける
            while(true) {
                if(currentBook == null) {
                    Debug.LogWarning("Book Is Null");
                }
                if(sleepTime != 0f) {
                    yield return new WaitForSeconds(sleepTime);
                    sleepTime = 0f;
                }
                while(true) {
                    // 条件チェック
                    if(sleepTime != 0f) break;
                    if(locked) break;
                    if(!running) break;
                    if(ValidateIndex() == false) break;
                    // コマンド取得
                    Command command = currentBook.commands[index];
                    // If関数によるスキップ中
                    if(ifSkip == true) {
                        // If関数の場合
                        if(command.className == GetType().FullName && command.methodName == "If") {
                            ifSkippingIndentLevel++;
                        // EndIf関数の場合
                        } else if(command.className == typeof(CommandBookReader).FullName && command.methodName == "EndIf") {
                            if(ifSkippingIndentLevel == 0) {
                                ifSkip = false;
                            } else {
                                ifSkippingIndentLevel--;
                            }
                        }
                    // コマンド実行
                    } else {
                        if(command.executeStop == true) running = false;

                        CommandBook oldBook = currentBook;
                        int oldIndex = index;

                        yield return ExecuteCommand(command);

                        if(currentBook != oldBook || index != oldIndex) {
                            indexIncrementSkip = true;
                        }
                    }
                    if(indexIncrementSkip) {
                        indexIncrementSkip = false;
                    } else {
                        index++;
                    }
                }

                yield return null;
            }
        }
        
        // インデックス妥当性チェック
        private bool ValidateIndex() {
            return 0 <= index && index < currentBook.commands.Count;
        }
        // コマンド実行
        private IEnumerator ExecuteCommand(Command command) {
            if(command.category == CommandCategory.Normal || command.category == CommandCategory.BookControl) {
                if(command.className != "" && command.methodName != "") {
                    // クラス取得
                    Type type = Type.GetType(command.className);
                    if(type == null) {
                        ErrorLog("Class Not Found", command);
                        yield break;
                    }
                    // 引数型配列取得
                    Type[] argTypes = TypeNamesToTypes(command.argsTypeNames.ToArray());
                    if(argTypes ==null) {
                        ErrorLog("ArgType Not Found", command);
                        yield break;
                    }
                    // 引数を文字列から本来の型に変換
                    List<object> argList= new List<object>();
                    for(int i = 0; i < command.argsTypeNames.Count; i++) {
                        if(command.argVariableUseFlags[i]) {
                            Variable v = FindVariable(command.argVariableNames[i], command.argsTypeNames[i]);
                            if(v == null) {
                                ErrorLog("Variable Not Foud. Name = " + command.argVariableNames[i] + ", Type = " + command.argsTypeNames[i], command);
                                yield break;;
                            }
                            argList.Add(masterConverter.Convert(command.argsTypeNames[i], v.valueString));
                        } else {
                            argList.Add(masterConverter.Convert(command.argsTypeNames[i], command.argValueStrings[i]));
                        }
                    }
                    if(argList == null) {
                        ErrorLog("Faild To Args Convert", command);
                        yield break;
                    }
                    // メソッド情報取得
                    MethodInfo method = type.GetMethod(command.methodName, argTypes);
                    if(method == null) {
                        ErrorLog("MethodInfo Not Found", command);
                        yield break;
                    }
                    // 対象のコンポーネントを検索
                    object targetInstance = null;
                    // 通常コマンドの場合、シーン内を検索
                    if(command.category == CommandCategory.Normal) {
                        // #if UNITY_EDITOR
                        //     // FindObjectsOfTypeAllはエディタ上ではプレハブも取ってくるため、ヒエラルキー上のものだけに選別する
                            
                        //     targetInstances = Resources.FindObjectsOfTypeAll(type)
                        //     .Where(go => AssetDatabase.GetAssetOrScenePath(go).Contains(".unity")).ToArray();
                        // #else
                        //     targetInstances = Resources.FindObjectsOfTypeAll(type);
                        // #endif
                        targetInstance = container.TryResolve(type);
                        if(targetInstance == null) {
                            if(type.IsSubclassOf(typeof(MonoBehaviour))) {
                                targetInstance = FindObjectOfType(type);
                            }
                        }
                    // ブック操作コマンドの場合、このインスタンスを使用
                    } else if(command.category == CommandCategory.BookControl) {
                        targetInstance = this;
                    }
                    if(targetInstance == null) {
                        ErrorLog("Target Instance Not Found", command);
                    } else {
                        Variable reternVariable = null;
                        if(command.returnSaveVariableName != "") {
                            reternVariable = FindVariable(command.returnSaveVariableName, command.returnTypeName);
                            if(reternVariable == null) {
                                ErrorLog("Return variable not found", command);
                            }
                        }

                        object ret = null;
                        if(command.waitUntil) {
                            TypeConverter converter = masterConverter.GetConverter(command.returnTypeName);
                            yield return new WaitUntil(() => {
                                ret = method.Invoke(targetInstance, argList.ToArray());
                                return converter.ValueToString(ret) == command.waitConditionValueString;
                            });
                        } else {
                            ret = method.Invoke(targetInstance, argList.ToArray());
                        }
                        
                        if(reternVariable != null) {
                            reternVariable.valueString = masterConverter.GetConverter(command.returnTypeName).ValueToString(ret);
                        }
                    }
                // クラス名またはメソッド名の不正
                } else {
                    ErrorLog("Invalid className or mathodName", command);
                    yield break;
                }
            // 不明なコマンド分類
            } else {
                ErrorLog("Unknown Command Category", command);
                yield break;
            }
        }
        // タイプ名→タイプ型への変換
        private Type[] TypeNamesToTypes(string[] typeNames) {
            List<Type> types = new List<Type>();
            foreach(string typeName in typeNames) {
                Type t = Type.GetType(typeName);
                if(t == null) {
                    t = Type.GetType(typeName + ",UnityEngine");
                    if(t == null) {
                        Debug.LogWarning(typeName + " Not Found");
                        return null;
                    }
                }
                types.Add(t);
            }
            return types.ToArray();
        }
        // 変数検索
        private Variable FindVariable(string name, string typeName) {
            foreach(Variable v in currentBook.variables) {
                if(v.name == name && v.typeName == typeName) return v;
            }
            return null;
        }
        // エラーログ(コマンド内容を出力)
        private void ErrorLog(string message, Command command) {
            string log = message;
            log += "\r\nComamand : category = " + command.category.ToString() + " " + command.className + "." + command.methodName + "(";
            for(int i = 0; i < command.argsTypeNames.Count; i++) {
                if(i != 0) log += ",";
                log += command.argsTypeNames[i] + " " + command.argValueStrings[i];
            }
            log += ")";
            if(command.returnSaveVariableName != "") {
                log += "\r\nreturn value save to variable \""+ command.returnSaveVariableName + "\"(" + command.returnTypeName + ")";
            }
            Debug.LogWarning(log);
        }

        // ブック操作関数(コマンドデータベースに登録)
        // None
        public void None() {
            
        }
        // ブック切り替え
        public void SwitchBook(string bookName, bool run) {
            SwitchBook(bookName, 0, run);
        }
        public void SwitchBook(string bookName, int index, bool run) {
            CommandBook newBook = books.Find(x => x.name == bookName);
            if(newBook == null) {
                Debug.LogWarning("CommandBook Not Found. BookName = " + bookName);
            } else {
                currentBook = newBook;
                this.index = index;
                running = run;
                locked = false;
            }
        }
        public void SwitchBook(string bookName, string tag, bool run) {
            SwitchBook(bookName, 0, run);
            SetIndexByTag(tag);
        }
        // スリープ関数
        public void Sleep(float time) {
            sleepTime = time;
        }
        // ロック関数
        public void SetLock(bool value) {
            locked = value;
        }
        // インデックス関係
        public void SetIndex(int value) {
            index = value;
        }
        public bool SetIndexByTag(string value) {
            int targetIndex = -1;
            foreach(Command command in currentBook.commands) {
                if(value == command.tag) {
                    targetIndex = currentBook.commands.IndexOf(command);
                    break;
                }
            }
            if(targetIndex == -1) {
                Debug.LogWarning("Tag Not Found. Tag = " + value + ", Book = " + currentBook.name);
                return false;
            } else {
                SetIndex(targetIndex);
                return true;
            }
        }
        public void AddIndex(int value) {
            index += value;
        }
        // コマンド実行系関数
        public void Run() {
            locked = false;
            running = true;
        }
        public void Run(int index) {
            SetIndex(index);
            Run();
        }
        public void Run(string tag) {
            if(currentBook.commands.Find(x => x.tag == tag) == null)
            {
                CommandBook targetBook = books.Find(book => book.commands.Find(command => command.tag == tag) != null);
                if(targetBook == null)
                {
                    Debug.LogWarning("Target Book Not Found. Tag = " + tag);
                    return;
                }
                SwitchBook(targetBook.name, false);
            }
            if(SetIndexByTag(tag))
            {
                Run();
            }
        }
        public void Stop() {
            running = false;
        }
        // 制御関数
        public void If(bool a, bool b, bool equal) {
            if((a == b) == equal) {}
            else {ifSkip = true;}
        }
        public void If(int a, int b, bool equal) {
            if((a == b) == equal) {}
            else {ifSkip = true;}
        }
        public void If(string a, string b, bool equal) {
            if((a == b) == equal) {}
            else {ifSkip = true;}
        }
        public void If(float a, float b, bool equal) {
            if((a == b) == equal) {}
            else {ifSkip = true;}
        }
        public void If(double a, double b, bool equal) {
            if((a == b) == equal) {}
            else {ifSkip = true;}
        }
        public void EndIf() {}
        // ログ
        public void Log(string log) {
            Debug.Log(log);
        }
    }
}