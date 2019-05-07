using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SuperCU.Pattern;

public class StateNodeEditor : EditorWindow
{
    private Node root;//基底ノード
    private GameObject o = null;//ステート管理オブジェクト
    private TestState state;//オブジェクトのステート
    private StateBase nowState = null;
    //TODO:DictionaryではなくIDで管理する
    private Dictionary<string, Node> nodeDictionary = new Dictionary<string, Node>();
    [MenuItem("スーパーCU格ゲーエン人17号/Node Editor")]
    static void Open()
    {
        StateNodeEditor stage = EditorWindow.GetWindow<StateNodeEditor>();
        stage.autoRepaintOnSceneChange = true;
    }
    protected virtual void OnGUI()
    {
        o = EditorGUILayout.ObjectField(o, typeof(GameObject), true) as GameObject;
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
        if (nowState != state.stateProcessor.State)
        {
            if (state.stateProcessor.State != null)
            {
                if (nowState != null)
                {
                    nodeDictionary[nowState.getStateName()].color = Color.white;
                }
                nowState = state.stateProcessor.State;
                nodeDictionary[nowState.getStateName()].color = Color.red;
            }
        }
        EndWindows();
    }
    //ノード初期化　TODO:最適化
    private void Init()
    {
        state = o.GetComponent<TestState>();
        int id = 0;
        foreach (StateString ss in state.states)
        {
            nodeDictionary.Add(ss.getStateName(), new Node(ss.getStateName(), new Vector2(200, 200),id));
            id++;//ウィンドウID
        }
        root = nodeDictionary[state.states[0].getStateName()];//基底ノード

        //ステートを見て関係を格納
        foreach (StateJudge judge in state.states[0].stateJudges)
        {
            if (nodeDictionary[judge.nextState] != null)
            {
                root.childs.Add(nodeDictionary[judge.nextState]);
            }
        }
        for (int i = 1; i<state.states.Count; i++)
        {
            Node nowNode = nodeDictionary[state.states[i].getStateName()];
            if (state.states[i].stateJudges.Count > 0)
            {
                foreach (StateJudge judge in state.states[i].stateJudges)
                {
                    if (nodeDictionary[judge.nextState] != null)
                    {
                        nowNode.childs.Add(nodeDictionary[judge.nextState]);
                    }
                }
            }
        }
    }
    //ベースとなるノードクラス
    public class Node
    {
        public int id;
        public string name;
        public Rect window;
        public List<Node> childs = new List<Node>();
        public Color color = Color.white;
        //初期化
        public Node(string na, Vector2 position,int d)
        {
            id = d;
            name = na;
            window = new Rect(position, new Vector2(100, 50));
        }
        //描画
        public void Draw()
        {
            //色変え
            GUI.backgroundColor = color;

            window = GUI.Window(id, window, DrawNodeWindow, "Window" + id);
            foreach (var child in childs)
            {
                DrawNodeLine(this.window, child.window); 
            }
        }
        //GUIウィンドウ用関数
        void DrawNodeWindow(int id)
        {
            GUI.DragWindow();
            GUI.Label(new Rect(30, 22, 100, 100), name, EditorStyles.label);
        }
        static void DrawNodeLine(Rect start, Rect end)
        {
            Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
            Vector3 harhPos = (startPos + endPos) / 2;
            Vector3 direction = (endPos - startPos).normalized;
            //色は適当
            Handles.color = Color.white;
            Handles.DrawLine(startPos, endPos);
            Handles.color = Color.white;
            //三角形ポリゴン（矢印）作成
            Handles.DrawAAConvexPolygon(
                harhPos +( new Vector3(-direction.y, direction.x, 0).normalized * 6) + new Vector3(0, 0, -5),
                harhPos + (new Vector3(-direction.y, direction.x, 0).normalized * -6) + new Vector3(0, 0, -5),
                harhPos + (direction * 10)+new Vector3(0,0,-5));
        }
    }
}