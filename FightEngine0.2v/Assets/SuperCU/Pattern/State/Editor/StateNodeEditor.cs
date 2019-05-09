using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SuperCU.Pattern;
using System.Reflection;
using System;

public class StateNodeEditor : EditorWindow
{
    enum StateMode
    {
        Nomal,
        Animation,
    }
    private StateMode stateMode = StateMode.Nomal;
    private Node root;//基底ノード
    private GameObject o = null;//ステート管理オブジェクト
    private StateMachineMonoBehaiviour stateClass;//オブジェクトのステート
    private StateBase nowState = null;
    private List<StateString> states = new List<StateString>();
    //TODO:DictionaryではなくIDで管理する
    private Dictionary<string, Node> nodeDictionary = new Dictionary<string, Node>();
    [MenuItem("スーパーCU格ゲーエン人17号/Node Editor")]
    static void Open()
    {
        StateNodeEditor stage = EditorWindow.GetWindow<StateNodeEditor>();
        stage.autoRepaintOnSceneChange = true;
        stage.wantsMouseMove = true;
    }
    protected virtual void OnGUI()
    {
        o = EditorGUILayout.ObjectField(o, typeof(GameObject), false) as GameObject;
        EditorGUILayout.LabelField("アタッチするものはStateのついたプレハブです");
        if (root == null && o != null)
        {
            Init();
        }
        if(o == null)
        {
            return;
        }

        BeginWindows();
        foreach (KeyValuePair<string,Node> a in nodeDictionary)
        {
            nodeDictionary[a.Key].Draw();
        }
        //現在のステートの色を変える
        if (nowState != stateClass.stateProcessor.State)
        {
            if (stateClass.stateProcessor.State != null)
            {
                if (nowState != null)
                {
                    nodeDictionary[nowState.getStateName()].color = Color.white;
                }
                nowState = stateClass.stateProcessor.State;
                nodeDictionary[nowState.getStateName()].color = Color.red;
            }
            else
            {
                nodeDictionary[nowState.getStateName()].color = Color.white;
            }
        }
        EndWindows();

    }
    //ノード初期化　TODO:最適化
    private void Init()
    {
        //playModeStateChangedイベントにメソッド登録
        EditorApplication.playModeStateChanged += OnChangedPlayMode;
        //ステートがアニメーションならば
        if (o.GetComponent<AnimationState>() != null)
        {
            var cla = o.GetComponent<AnimationState>();
            states = cla.characterProperty.States;
            stateMode = StateMode.Animation;
        }
        //ノーマル（ＵＩとか）
        else
        {
            var cla = o.GetComponent<StateMachineMonoBehaiviour>();
            states = cla.states;
        }
        stateClass = o.GetComponent<StateMachineMonoBehaiviour>();
        int id = 0;
        foreach (StateString ss in states)
        {
            nodeDictionary.Add(ss.getStateName(), new Node(ss.getStateName(), new Vector2(200, 200), id, ss, o));
            id++;//ウィンドウID
        }
        root = nodeDictionary[states[0].getStateName()];//基底ノード

        //ステートを見て関係を格納
        foreach (StateJudge judge in states[0].stateJudges)
        {
            if (nodeDictionary[judge.nextState] != null)
            {
                root.childs.Add(nodeDictionary[judge.nextState]);
            }
        }
        for (int i = 1; i < states.Count; i++)
        {
            Node nowNode = nodeDictionary[states[i].getStateName()];
            if (states[i].stateJudges.Count > 0)
            {
                foreach (StateJudge judge in states[i].stateJudges)
                {
                    if (nodeDictionary[judge.nextState] != null)
                    {
                        nowNode.childs.Add(nodeDictionary[judge.nextState]);
                    }
                }
            }
        }
    }
    //プレイモードが変更された
    void OnChangedPlayMode(PlayModeStateChange isNowPlayng)
    {
        //停止時にノードの色を戻す
        if (isNowPlayng == PlayModeStateChange.ExitingPlayMode)
        {
            nodeDictionary[nowState.getStateName()].color = Color.white;
        }
    }

}
