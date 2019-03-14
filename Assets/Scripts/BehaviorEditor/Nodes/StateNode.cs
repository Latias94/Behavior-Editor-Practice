using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using State = Behavior.State;

namespace BehaviorEditor
{
    [CreateAssetMenu(menuName = "Editor/Nodes/State Node")]
    public class StateNode : DrawNode
    {

        public override void DrawWindow(BaseNode b)
        {
            if (b.currentState == null)
            {
                EditorGUILayout.LabelField("Add state to modify:");
            }
            else
            {
                if (!b.collapse)
                {
//                    windowRect.height = 300;
                }
                else
                {
                    b.windowRect.height = 100;
                }

                b.collapse = EditorGUILayout.Toggle("Collapse", b.collapse);
            }

            b.currentState = (State) EditorGUILayout.ObjectField(b.currentState, typeof(State), false);

            if (b.previousCollapse != b.collapse)
            {
                b.previousCollapse = b.collapse;
//                BehaviorEditor.currentGraph.SetNode(this);
            }

            // 如果改变了 stateNode 中的 state
            if (b.previousState != b.currentState)
            {
                b.serializedState = null;


                b.isDuplicate = BehaviorEditor.currentGraph.IsStateNodeDuplicate(this);
                if (!b.isDuplicate)
                {
//                    BehaviorEditor.currentGraph.SetNode(this);
                    b.previousState = b.currentState;
                    for (int i = 0; i < b.currentState.transitions.Count; i++)
                    {
//                    dependencies.Add(BehaviorEditor.AddTransitionNode(i, currentState.transitions[i], this));
                    }
                }
            }

            if (b.isDuplicate)
            {
                EditorGUILayout.LabelField("State is a duplicate!");
                b.windowRect.height = 100;
                return;
            }

            if (b.currentState != null)
            {
                if (b.serializedState == null)
                {
                    b.serializedState = new SerializedObject(b.currentState);
                    b.onStateList = new ReorderableList(b.serializedState, b.serializedState.FindProperty("onState")
                        , true, true, true, true);
                    b.onEnterList = new ReorderableList(b.serializedState, b.serializedState.FindProperty("onEnter")
                        , true, true, true, true);
                    b.onExitList = new ReorderableList(b.serializedState, b.serializedState.FindProperty("onExit")
                        , true, true, true, true);
                }

                if (!b.collapse)
                {
                    b.serializedState.Update();
                    HandleReordableList(b.onStateList, "On State");
                    HandleReordableList(b.onEnterList, "On Enter");
                    HandleReordableList(b.onExitList, "On Exit");

                    EditorGUILayout.LabelField("");
                    b.onStateList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.onEnterList.DoLayoutList();
                    EditorGUILayout.LabelField("");
                    b.onExitList.DoLayoutList();

                    b.serializedState.ApplyModifiedProperties();

                    float standard = 300;
                    standard += b.onStateList.count * 20;
                    b.windowRect.height = standard;
                }
            }
        }

        private void HandleReordableList(ReorderableList list, string targetName)
        {
            list.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, targetName); };
            list.drawElementCallback = (rect, index, selected, focused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    element, GUIContent.none);
            };
        }

        public override void DrawCurve(BaseNode b)
        {
        }

        public Transition AddTransition()
        {
            return null; //currentState.AddTransition();
        }

        public void ClearReferences()
        {
//            BehaviorEditor.ClearWindowsFromList(dependencies);
//            dependencies.Clear();
        }
    }
}