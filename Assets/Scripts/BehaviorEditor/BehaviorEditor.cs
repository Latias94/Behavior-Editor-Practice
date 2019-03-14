using System;
using System.Collections.Generic;
using Behavior;
using UnityEditor;
using UnityEngine;

namespace BehaviorEditor
{
    public class BehaviorEditor : EditorWindow
    {
        #region Variables

        private Vector3 mousePosition;
        private bool makeTransition;
        private bool clickedOnWindow;
        private BaseNode selectedNode;
        public static BehaviorGraph currentGraph;

        enum UserActions
        {
            AddState,
            AddTransitionNode,
            DeleteNode,
            CommentNode
        }

        #endregion

        #region Init

        [MenuItem("Behavior Editor/Editor")]
        static void ShowEditor()
        {
            BehaviorEditor editor = GetWindow<BehaviorEditor>();
            editor.minSize = new Vector2(800, 600);
        }

        private void OnEnable()
        {
        }

        #endregion

        #region GUI Methods

        private void OnGUI()
        {
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
        }

        void DrawWindows()
        {
            BeginWindows();
            EditorGUILayout.LabelField(" ", GUILayout.Width(100));
            EditorGUILayout.LabelField("Assign Graph:", GUILayout.Width(100));
            currentGraph =
                (BehaviorGraph) EditorGUILayout.ObjectField(currentGraph, typeof(BehaviorGraph), false,
                    GUILayout.Width(200));

            if (currentGraph != null)
            {
                foreach (BaseNode n in currentGraph.windows)
                {
                    n.DrawCurve();
                }

                for (int i = 0; i < currentGraph.windows.Count; i++)
                {
                    currentGraph.windows[i].windowRect = GUI.Window(i, currentGraph.windows[i].windowRect,
                        DrawNodeWindows,
                        currentGraph.windows[i].windowTitle);
                }
            }

            EndWindows();
        }

        void DrawNodeWindows(int id)
        {
            currentGraph.windows[id].DrawWindow();
            GUI.DragWindow();
        }

        void UserInput(Event e)
        {
            if (currentGraph == null) return;
            if (e.button == 1 && !makeTransition)
            {
                if (e.type == EventType.MouseDown)
                {
                    RightClick(e);
                }
            }

            if (e.button == 0 && !makeTransition)
            {
                if (e.type == EventType.MouseDown)
                {
                }
            }
        }

        void RightClick(Event e)
        {
            selectedNode = null;
            clickedOnWindow = false;
            for (int i = 0; i < currentGraph.windows.Count; i++)
            {
                // 找到鼠标所在的窗体
                if (currentGraph.windows[i].windowRect.Contains(mousePosition))
                {
                    clickedOnWindow = true;
                    selectedNode = currentGraph.windows[i];
                    break;
                }
            }

            if (!clickedOnWindow)
            {
                AddNewNodes(e);
            }
            else
            {
                ModifyNodes(e);
            }
        }

        void AddNewNodes(Event e)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddSeparator("");
            if (currentGraph != null)
            {
                menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UserActions.AddState);
                menu.AddItem(new GUIContent("Add Comment"), false, ContextCallback, UserActions.CommentNode);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Add State"));
                menu.AddDisabledItem(new GUIContent("Add Comment"));
            }

            menu.ShowAsContext();
            e.Use();
        }

        void ModifyNodes(Event e)
        {
//            GenericMenu menu = new GenericMenu();
//
//            if (selectedNode is StateNode)
//            {
//                StateNode stateNode = (StateNode) selectedNode;
//                if (stateNode.currentState != null)
//                {
//                    menu.AddSeparator("");
//                    menu.AddItem(new GUIContent("Add Transition"), false, ContextCallback,
//                        UserActions.AddTransitionNode);
//                }
//                else
//                {
//                    menu.AddDisabledItem(new GUIContent("Add Transition"));
//                }
//
//                menu.AddSeparator("");
//                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
//            }
//
//            if (selectedNode is TransitionNode)
//            {
//                menu.AddSeparator("");
//                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
//            }
//
//            if (selectedNode is CommentNode)
//            {
//                menu.AddSeparator("");
//                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
//            }
//
//            menu.ShowAsContext();
//            e.Use();
        }

        void ContextCallback(object o)
        {
//            UserActions a = (UserActions) o;
//            switch (a)
//            {
//                case UserActions.AddState:
//                    AddStateNode(mousePosition);
//                    break;
//
//                case UserActions.AddTransitionNode:
//                    if (selectedNode is StateNode)
//                    {
//                        StateNode from = (StateNode) selectedNode;
////                        Transition transition = from.AddTransition();
//                        AddTransitionNode(from.currentState.transitions.Count, null, from);
//                    }
//
//                    break;
//
//                case UserActions.CommentNode:
//                    AddCommentNode(mousePosition);
//                    break;
//
//                case UserActions.DeleteNode:
//                    if (selectedNode is StateNode)
//                    {
//                        StateNode target = (StateNode) selectedNode;
//                        target.ClearReferences();
//                        currentGraph.windows.Remove(target);
//                    }
//
//                    if (selectedNode is TransitionNode)
//                    {
//                        TransitionNode target = (TransitionNode) selectedNode;
//                        currentGraph.windows.Remove(target);
////                        if (target.enterState.currentState.transitions.Contains(target.targetCondition))
////                            target.enterState.currentState.transitions.Remove(target.targetCondition);
//                    }
//
//                    if (selectedNode is CommentNode)
//                    {
//                        currentGraph.windows.Remove(selectedNode);
//                    }
//
//                    break;
//                default:
//                    break;
//            }
        }

        #endregion

        #region Helper Methods

        /*   public static StateNode AddStateNode(Vector2 pos)
           {
               StateNode stateNode = CreateInstance<StateNode>();
   //            stateNode.windowRect = new Rect(pos.x, pos.y, 200, 300);
   //            stateNode.windowTitle = "State";
   //            currentGraph.windows.Add(stateNode);
   ////            currentGraph.SetStateNode(stateNode);
               return stateNode;
           }
   
           public static CommentNode AddCommentNode(Vector2 pos)
           {
               CommentNode commentNode = CreateInstance<CommentNode>();
               commentNode.windowRect = new Rect(pos.x, pos.y, 200, 100);
               commentNode.windowTitle = "Comment";
               currentGraph.windows.Add(commentNode);
               return commentNode;
           }
   
   
           // 通过索引来计算 TransitionNode 的位置
           public static TransitionNode AddTransitionNode(int index, Transition transition, StateNode from)
           {
               Rect fromRect = from.windowRect;
               fromRect.x += 50;
               float targetY = fromRect.y - fromRect.height;
               if (from.currentState != null)
               {
                   targetY += index * 100;
               }
   
               fromRect.y = targetY;
               fromRect.x += 200 + 100;
               fromRect.y += fromRect.height * .7f;
               Vector2 position = new Vector2(fromRect.x, fromRect.y);
               return AddTransitionNode(position, transition, from);
           }
   
           // 为了保存上一次 TransitionNode 的位置，重载了函数
           public static TransitionNode AddTransitionNode(Vector2 pos, Transition transition, StateNode from)
           {
               TransitionNode transitionNode = CreateInstance<TransitionNode>();
               transitionNode.Init(from, transition);
               transitionNode.windowRect = new Rect(pos.x, pos.y, 200, 80);
               transitionNode.windowTitle = "Condition Check";
               currentGraph.windows.Add(transitionNode);
               from.dependencies.Add(transitionNode);
               return transitionNode;
           }
   */
        public static void DrawNodeCurve(Rect start, Rect end, bool left, Color curveColor)
        {
            Vector3 startPos = new Vector3(
                left ? start.x + start.width : start.x,
                start.y + start.height * .5f,
                0
            );
            Vector3 endPos = new Vector3(
                end.x + end.width * .5f,
                end.y + end.height * .5f,
                0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;

            Color shadow = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++)
            {
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadow, null, (i + 1) * .5f);
            }

            Handles.DrawBezier(startPos, endPos, startTan, endTan, curveColor, null, 1);
        }

        public static void ClearWindowsFromList(List<BaseNode> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
//                if (currentGraph.windows.Contains(list[i]))
//                    currentGraph.windows.Remove(list[i]);
            }
        }

        #endregion
    }
}