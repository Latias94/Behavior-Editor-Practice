using Behavior;
using UnityEditor;
using UnityEngine;

namespace BehaviorEditor
{
    public class TransitionNode
    {
//        public bool isDuplicate;
//        public Condition targetCondition;
//        public Condition previousCondition;
//        public Transition transition;
//        public StateNode enterState;
//        public StateNode targetState;
//
//        public void Init(StateNode enterState, Transition transition)
//        {
//            this.enterState = enterState;
//            this.transition = transition;
//        }
//
//        public override void DrawWindow()
//        {
//            EditorGUILayout.LabelField("");
//            targetCondition =
//                (Condition) EditorGUILayout.ObjectField(targetCondition, typeof(Condition), false);
//            if (targetCondition == null)
//            {
//                EditorGUILayout.LabelField("No Condition!");
//            }
//            else
//            {
//                if (isDuplicate)
//                {
//                    EditorGUILayout.LabelField("Duplicate Condition!");
//                }
////                else
////                {
////                    if (transition != null)
////                    {
////                        transition.disable = EditorGUILayout.Toggle("Disable", transition.disable);
////                    }
////                }
//            }
//
//            if (previousCondition != targetCondition)
//            {
//                isDuplicate = BehaviorEditor.currentGraph.IsTransitionDuplicate(this);
//                if (!isDuplicate)
//                {
//                    BehaviorEditor.currentGraph.SetNode(this);
//                }
//
//                previousCondition = targetCondition;
//            }
//        }
//
//        public override void DrawCurve()
//        {
//            if (enterState)
//            {
//                Rect rect = windowRect;
//                rect.y += windowRect.height * .5f;
//                rect.width = 1;
//                rect.height = 1;
//
//                BehaviorEditor.DrawNodeCurve(enterState.windowRect, rect, true, Color.black);
//            }
//        }

    }
}