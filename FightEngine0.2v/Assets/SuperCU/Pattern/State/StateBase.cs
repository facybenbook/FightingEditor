using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperCU.Pattern
{
    //リストで使用
    [System.Serializable]
    public struct StateJudge
    {
        public string nextState;//次に入れ替わることの出来るステート
        public string key;//入力キー
        public int priority;//優先度
    }

    public class StatePrpcessor
    {
        //ステート本体
        private StateBase _State;
        public StateBase State
        {
            set { _State = value; }
            get { return _State; }
        }

        // 実行ブリッジ
        public void Execute()
        {
            State.Execute();
        }
    }
    [System.Serializable]
    //ステートのクラス
    public abstract class StateBase
    {
        //デリゲート
        public delegate void executeState();
        public string stringName;
        public executeState execDelegate;
        public List<StateJudge> stateJudges = new List<StateJudge>();

        //実行処理
        public virtual void Execute()
        {
            if (execDelegate != null)
            {
                execDelegate();
            }
        }
        //ステート名を取得するメソッド
        public abstract string getStateName();
    }
    [System.Serializable]
    public class StateString : StateBase
    {
        public override string getStateName()
        {
            return stringName;
        }
    }
}
