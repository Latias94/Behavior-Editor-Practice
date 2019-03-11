using Behavior;
using UnityEditor;

namespace BehaviorEditor
{
    public class GraphNode:BaseNode
    {
        private BehaviorGraph previousGraph;
        public override void DrawWindow()
        {
            if (BehaviorEditor.currentGraph == null)
            {
                EditorGUILayout.LabelField("Add Graph to modify:");
            }

            BehaviorEditor.currentGraph =
                (BehaviorGraph) EditorGUILayout.ObjectField(BehaviorEditor.currentGraph, typeof(BehaviorGraph), false);
            if (BehaviorEditor.currentGraph == null)
            {
                if (previousGraph != null)
                {
                    previousGraph = null;
                }
                EditorGUILayout.LabelField("No Graph Assigned");
                return;
            }

            if (previousGraph != BehaviorEditor.currentGraph)
            {
                previousGraph = BehaviorEditor.currentGraph;
                BehaviorEditor.LoadGraph();
            }
        }

        public override void DrawCurve()
        {
            base.DrawCurve();
        }
    }
}