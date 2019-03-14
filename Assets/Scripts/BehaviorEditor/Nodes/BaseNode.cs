using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using State = Behavior.State;

namespace BehaviorEditor
{
    [System.Serializable]
    public class BaseNode
    {
        public DrawNode drawNode;

        public Rect windowRect;
        public string windowTitle;

        [HideInInspector] public bool collapse;

        // 方便对比之前是否折叠的状态，Graph 需要保存 StateNode 的折叠状态
        [HideInInspector] public bool previousCollapse;
        [HideInInspector] public bool isDuplicate;
        [HideInInspector] public State previousState;
        [HideInInspector] public State currentState;

        [HideInInspector] public List<BaseNode> dependencies = new List<BaseNode>();

        [HideInInspector] public SerializedObject serializedState;
        [HideInInspector] public ReorderableList onStateList;
        [HideInInspector] public ReorderableList onEnterList;
        [HideInInspector] public ReorderableList onExitList;

        public void DrawWindow()
        {
            if (drawNode != null)
            {
                drawNode.DrawWindow(this);
            }
        }

        public void DrawCurve()
        {
            if (drawNode != null)
            {
                drawNode.DrawCurve(this);
            }
        }
    }
}