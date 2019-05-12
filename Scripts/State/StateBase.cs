using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//ステートの基底クラス
[System.Serializable]
public abstract class StateBase
{
    //デリゲート
    public delegate void ExecuteState();
    public delegate void PlayUpdateState();
    //ステートの名前
    public string stateName = null;
    //次のステート情報
    public List<NextStateJudge> nextStateJudge = new List<NextStateJudge>();

    //ステート移行時に再生するデリゲート
    public ExecuteState execDelegate;
    //ステート移行時に常時プレイするデリゲート
    public PlayUpdateState playUpdateDelegate;

    //実行処理
    public virtual void Execute()
    {
        if (execDelegate != null)
        {
            execDelegate();
        }
    }
    //常時実行用処理
    public virtual void PlayUpdate()
    {
        if(playUpdateDelegate != null)
        {
            playUpdateDelegate();
        }
    }
    //ステート名を取得するメソッド
    public abstract string getStateName();

}
//以下ステートの種類 

//通常ステート
[System.Serializable]
public class NomalState : StateBase
{
    public override string getStateName()
    {
        return stateName;
    }
}


