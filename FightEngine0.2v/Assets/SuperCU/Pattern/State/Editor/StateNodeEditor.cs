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
	private static StateNodeEditor stage;
    private StateMode stateMode = StateMode.Nomal;
    private Node root;//基底ノード
    private GameObject o = null;//ステート管理オブジェクト
	private GameObject objBuffer;
	private List<StateString> stateBuffer;
	private StateMachineMonoBehaiviour stateClass;//オブジェクトのステート
    private StateBase nowState = null;
    private List<StateString> states = new List<StateString>();
    //TODO:DictionaryではなくIDで管理する
    private Dictionary<string, Node> nodeDictionary = new Dictionary<string, Node>();

	[MenuItem("スーパーCU格ゲーエン人17号/Node Editor")]
    static void Open()
    {
        stage = EditorWindow.GetWindow<StateNodeEditor>();
        stage.autoRepaintOnSceneChange = true;
        stage.wantsMouseMove = true;
    }
    protected virtual void OnGUI()
    {
        o = EditorGUILayout.ObjectField(o, typeof(GameObject), false) as GameObject;
        EditorGUILayout.LabelField("アタッチするものはStateのついたプレハブです");
        if (root == null && o != null )
        {
            Init();
        }
        if(o == null)
        {
            return;
        }
		GUILayoutOption[] gs = { GUILayout.Width(500), GUILayout.Height(20) };
		if (GUILayout.Button("更新",gs))
		{
			Init();
		}
		GUILayoutOption[] posS = {GUILayout.Width(500), GUILayout.Height(20) };
		if (GUILayout.Button("位置初期化", posS))
		{
			int i = 0;
			foreach (StateString s in states)
			{
				s.nodePosition = new Vector2(200+i, 200);
				i += 100;
			}
			Init();
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
		//ウインドウ内どこをクリックしてもコンテキストメニューを表示
		//右クリックメニュー
		Rect contextRect = new Rect(0, 0, Screen.width, Screen.height);

		if (Event.current.type == EventType.ContextClick)
		{
			Vector2 mousePos = Event.current.mousePosition;
			if (contextRect.Contains(mousePos))
			{
				// Now create the menu, add items and show it
				GenericMenu menu = new GenericMenu();

				menu.AddItem(new GUIContent("new StateCreate"), false, CallbackCreate, "item 1");

				menu.ShowAsContext();

				Event.current.Use();
			}
		}
		EndWindows();
	}
	void CallbackCreate(object obj)
	{
		Debug.Log("Selected: " + obj);
	}
	//ノード初期化　TODO:最適化
	private void Init()
    {
		//ステートがアニメーションならば
		if (o.GetComponent<AnimationState>() != null)
		{
			var cla = o.GetComponent<AnimationState>();
			states = cla.characterProperty.States;
			stateMode = StateMode.Animation;
		}
		//ノーマル（ＵＩとか）
		else if (o.GetComponent<StateMachineMonoBehaiviour>() != null)
		{
			var cla = o.GetComponent<StateMachineMonoBehaiviour>();
			states = cla.states;
		}
		else
		{
			return;
		}
		if (states.Count == 0)
		{
			return;
		}

		nodeDictionary = new Dictionary<string, Node>();
        //playModeStateChangedイベントにメソッド登録
        EditorApplication.playModeStateChanged += OnChangedPlayMode;//プレイモードの変更時メソッド
        stateClass = o.GetComponent<StateMachineMonoBehaiviour>();
        int id = 0;
        foreach (StateString ss in states)
        {
            nodeDictionary.Add(ss.getStateName(), new Node(ss.getStateName(), ss.nodePosition, id, ss, o));
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
		stateBuffer = states;
    }
    //プレイモードが変更された
    void OnChangedPlayMode(PlayModeStateChange isNowPlayng)
    {
		if(isNowPlayng == PlayModeStateChange.EnteredPlayMode)
		{
			if (GameObject.Find(o.name) != null)
			{
				objBuffer = o;
				o = GameObject.Find(o.name);
				Init();
			}
		}
        //停止時にノードの色を戻す
        if (isNowPlayng == PlayModeStateChange.ExitingPlayMode)
        {
            nodeDictionary[nowState.getStateName()].color = Color.white;
			if (objBuffer != null)
			{
				o = objBuffer;
				Init();
				objBuffer = null;
			}
        }
    }

}
