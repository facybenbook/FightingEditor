using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SuperCU.Generic;

using UnityEditor;

      
namespace SuperCU.Pattern
{
    //リストで使用
    [System.Serializable]
    public struct StateJudge
    {
        public InputMode inputMode;
        public string nextState;//次に入れ替わることの出来るステート
        public string key;//入力キー
        public int priority;//優先度
    }

    public class StatePrpcessor
    {
        //ステート本体
        private StateBase _State = null;
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
        public void Play()
        {
            State.Play();
        }
    }
    [System.Serializable]
    //ステートのクラス
    public abstract class StateBase
    {
        //デリゲート
        public delegate void executeState();
        public delegate void playState();
        public string stringName = null;

        public executeState execDelegate;

        //if NodeEditorにのみ使用
        public int stringNumber;
        public UnityEvent playDelegate;
        //public playState playDelegate;//ステートに移行されたときにプレイするイベント
        public List<StateJudge> stateJudges = new List<StateJudge>();

        //実行処理
        public virtual void Execute()
        {
            if (execDelegate != null)
            {
                execDelegate();
            }
        }
        public virtual void Play()
        {
            if(playDelegate != null)
            {
                Debug.Log("再生されたよ");
                playDelegate.Invoke();
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
