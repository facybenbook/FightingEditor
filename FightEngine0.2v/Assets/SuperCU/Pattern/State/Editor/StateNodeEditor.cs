using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using SuperCU.Pattern;

public class StateNodeEditor : EditorWindow
{
    private Node root;
    private GameObject o;
    //TODO:DictionaryではなくIDで管理する
    private Dictionary<string, StateString> stateDictionary = new Dictionary<string, StateString>();
    private Dictionary<string, Node> nodeDictionary = new Dictionary<string, Node>();
    [MenuItem("スーパーCU格ゲーエン人17号/Node Editor")]
    static void Open()
    {
        EditorWindow.GetWindow<StateNodeEditor>();
    }

    protected virtual void OnGUI()
    {
        o = EditorGUILayout.ObjectField(o, typeof(GameObject), true) as GameObject;
        if (this.root == null && o != null)
        {
            this.Init();
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
        EndWindows();

    }
    //ノード入れ（最適化できてません）
    private void Init()
    {
        TestState state = o.GetComponent<TestState>();
        int id = 0;
        foreach (StateString ss in state.states)
        {
            nodeDictionary.Add(ss.getStateName(), new Node(ss.getStateName(), new Vector2(200, 200),id));
            id++;
            ss.stateJudges.Sort((a, b) => b.priority - a.priority);//優先度順にソート(高ければ高い、降順),いらない
        }
        root = nodeDictionary[state.states[0].getStateName()];//基底ノード
        Vector2 x = new Vector2( 50,50);
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

    public class Node
    {
        public int id;
        public string name;
        public Rect window;
        public List<Node> childs = new List<Node>();

        public Node(string na, Vector2 position,int d)
        {
            id = d;
            name = na;
            this.window = new Rect(position, new Vector2(100, 50));
        }

        public void Draw()
        {
            this.window = GUI.Window(this.id, this.window, DrawNodeWindow, "Window" + this.id);
            foreach (var child in this.childs)
            {
                DrawNodeLine(this.window, child.window); 
            }
        }

        void DrawNodeWindow(int id)
        {
            GUI.DragWindow();
            GUI.Label(new Rect(30, 22, 100, 100), name, EditorStyles.label);
        }
        static Vector2 a = new Vector2();

        static void DrawNodeLine(Rect start, Rect end)
        {
            Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
            Vector3 harhPos = (startPos + endPos) / 2;
            Vector3 direction = (endPos - startPos).normalized;
            Vector2.SmoothDamp(harhPos, endPos,ref a,10);
            //色は適当
            Handles.color = Color.white;
            Handles.DrawLine(startPos, endPos);
            Handles.color = Color.red;
            //三角形ポリゴン（矢印）作成
            Handles.DrawAAConvexPolygon(
                harhPos +( new Vector3(-direction.y, direction.x, 0).normalized * 10) + new Vector3(0, 0, -5),
                harhPos + (new Vector3(-direction.y, direction.x, 0).normalized * -10) + new Vector3(0, 0, -5),
                harhPos + (direction * 10)+new Vector3(0,0,-5));
        }
    }
}