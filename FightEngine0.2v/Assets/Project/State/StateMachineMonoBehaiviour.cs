using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperCU.Pattern;
using SuperCU.Generic;
using SuperCU.FightingEngine;

public class StateMachineMonoBehaiviour : MonoBehaviour, IEventable
{
    //変更前のステート名
    private string _beforeStateName;

    //ステート本体（プロセッサ）
    public StatePrpcessor stateProcessor = new StatePrpcessor();

    //ステート
    public List<StateString> states = new List<StateString>();

    //TODO:DictionaryではなくIDで管理する
    private Dictionary<string, StateString> stateDictionary = new Dictionary<string, StateString>();

    protected virtual void Start()
    {
        GameManager.Instance.AddUpdate(this);//アップデートリストに追加
        //0がデフォルト
        stateProcessor.State = states[0];
        foreach (StateString ss in states)
        {
            ss.execDelegate = NomalState;
            stateDictionary.Add(ss.getStateName(), ss);//辞書追加
            ss.stateJudges.Sort((a, b) => b.priority - a.priority);//優先度順にソート(高ければ高い、降順)
        }
    }
    public virtual void UpdateGame()
    {
        //TODO:処理が出来てない
        stateProcessor.Execute();
        //ステートの値が変更されたら実行処理を行う
        if (stateProcessor.State == null)
        {
            return;
        }
        if (stateProcessor.State.getStateName() != _beforeStateName)
        {
            Debug.Log(" Now State:" + stateProcessor.State.getStateName());
            _beforeStateName = stateProcessor.State.getStateName();
            stateProcessor.Execute();
        }
    }

    public void NomalState()
    {
        //優先度順にジャッジ
        foreach (StateJudge judge in stateProcessor.State.stateJudges)
        {
            if (GetKeyboardValue.DownKeyCheck() == judge.key)
            {
                stateProcessor.State = stateDictionary[judge.nextState];
            }
        }
    }

    public void LateUpdateGame()
    {
    }
    public void FixedUpdateGame()
    {
    }
}
