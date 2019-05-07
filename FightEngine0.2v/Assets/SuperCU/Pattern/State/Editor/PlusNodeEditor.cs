using UnityEditor;
using UnityEngine;
using UnityEditor.Graphs;

public class PlusNodeEditor : StateNodeEditor
{
    static PlusNodeEditor example;

    Graph stateMachineGraph;
    GraphGUI stateMachineGraphGUI;

    [MenuItem("Window/Example")]
    static void Do()
    {
        example = GetWindow<PlusNodeEditor>();
    }

    void OnEnable()
    {
        if (stateMachineGraph == null)
        {
            stateMachineGraph = ScriptableObject.CreateInstance<Graph>();
            stateMachineGraph.hideFlags = HideFlags.HideAndDontSave;
        }
        if (stateMachineGraphGUI == null)
        {
            stateMachineGraphGUI = (GetEditor(stateMachineGraph));
        }
    }

    void OnDisable()
    {
        example = null;
    }

    protected override void OnGUI()
    {
        if (example && stateMachineGraphGUI != null)
        {
            stateMachineGraphGUI.BeginGraphGUI(example, new Rect(0, 0, example.position.width, example.position.height));

            // ノード描画
            //base.OnGUI();

            stateMachineGraphGUI.EndGraphGUI();
        }

    }

    GraphGUI GetEditor(Graph graph)
    {
        GraphGUI graphGUI = CreateInstance("GraphGUI") as GraphGUI;
        graphGUI.graph = graph;
        graphGUI.hideFlags = HideFlags.HideAndDontSave;
        return graphGUI;
    }
}
