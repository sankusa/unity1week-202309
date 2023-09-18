## SerializableStateMachine
### 概要
シリアライズに対応したステートマシン。  
JsonUtilityでJson形式にしてセーブ&ロード、といったことが可能。  

### 依存アセット
・UniRx

### 使い方
①ステートを作る  
```
[System.Serializable]
public class State1 : StateBase
{
    [SerializeField] int number;

    public override void OnEnter()
    {
        Debug.Log(number);
    }
}

[System.Serializable]
public class State2 : StateBase
{
    [SerializeField] string text;

    public override void OnExit()
    {
        Debug.Log(text);
    }
}
```

②ステートの列挙型を作る  
```
public enum StateType : StateBase
{
    State1,
    State2
}

```

③  
```
public class TestMonoBehaviour : MonoBehavioiur
{
    // 各ステート・ステートマシン毎に[SerializeField]変数を作る。
    [SerializeField] State1 state1 = new State1();
    [SerializeField] State1 state2 = new State2();
    [SerializeField] StateMachine<StateType> stateMachine = new StateMachine(StateType.State1);

    void Awake()
    {
        // ステートマシン構築
        stateMachine.AddState(StateType.State1, state1);
        stateMachine.AddState(StateType.State2, state2);
    }

    void Update()
    {
        // 必須
        stateMachine.Update();
    }

    public void ChangeStateToState2()
    {
        // ステート遷移
        stateMachine.ChangeState(StateType.State2)
    }
}

```

### クラス説明
#### StateBaseクラス
全てのステートの基底クラス。  

StateBase・・・基底型  
StateBase\<StateTypeEnum\>・・・StateBaseにサブステートの概念を追加  
StateBase\<StateTypeEnum,TriggerTypeEnum\>・・・さらにトリガー遷移を追加  
※上記は全て[Serializable]属性を持つ  
※StateTypeEnum、TriggerTypeEnumはそれぞれサブステートの列挙型、トリガーの列挙型。

#### StateMachineクラス  
StateMachine<StateTypeEnum>・・・ステートマシン(基本)
StateMachine<StateTypeEnum,TriggerTypeEnum>・・・ステートマシン(トリガー遷移付き)  
※上記は全て[Serializable]属性を持つ(変数「StateType currentState」がシリアライズ可能)
※StateTypeEnum、TriggerTypeEnumはそれぞれサブステートの列挙型、トリガーの列挙型。

全てのステートはStateBase型のコレクションとして保有する。  
※汎用性の観点から各ステート(StateBaseの子クラス)への依存はしない。

が、StateBase型変数ではステート固有の変数がシリアライズできない。  
よって、各ステートはStateMachine外部で[SerializeField]として保有しておき、実行時にデシリアライズされた各ステートを使ってStateMachineを構築する。

#### Transitionクラス
Transition<StateTypeEnum,TriggerTypeEnum>

トリガー遷移情報を持つ。(トリガーの種類と遷移先)

### 要件
以下の特徴を持ったStateMachine 
- 現在のState及び各Stateの固有の変数をシリアライズできる(Json形式でセーブ・ロードをするため)
- 使い回せる
- ステートがサブステートを持てる