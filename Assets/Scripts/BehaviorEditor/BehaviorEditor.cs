using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviorEditor
{
    public class BehaviorEditor : EditorWindow
    {
        #region Variables

        private static List<BaseNode> Windows = new List<BaseNode>();
        private Vector3 mousePosition;
        private bool makeTransition;
        private bool clickedOnWindow;
        private BaseNode selectedNode;

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

        #endregion

        #region GUI Methods

        private void OnGUI()
        {
            Event e = Event.current;
            mousePosition = e.mousePosition;
            UserInput(e);
            DrawWindows();
        }

        private void OnEnable()
        {
        }

        void DrawWindows()
        {
            BeginWindows();
            foreach (BaseNode n in Windows)
            {
                n.DrawCurve();
            }

            for (int i = 0; i < Windows.Count; i++)
            {
                Windows[i].windowRect = GUI.Window(i, Windows[i].windowRect, DrawNodeWindows, Windows[i].windowTitle);
            }

            EndWindows();
        }

        void DrawNodeWindows(int id)
        {
            Windows[id].DrawWindow();
            GUI.DragWindow();
        }

        void UserInput(Event e)
        {
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
            for (int i = 0; i < Windows.Count; i++)
            {
                // 鼠标在里面
                if (Windows[i].windowRect.Contains(mousePosition))
                {
                    clickedOnWindow = true;
                    selectedNode = Windows[i];
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
            menu.AddItem(new GUIContent("Add State"), false, ContextCallback, UserActions.AddState);
            menu.AddItem(new GUIContent("Add Comment"), false, ContextCallback, UserActions.CommentNode);
            menu.ShowAsContext();
            e.Use();
        }

        void ModifyNodes(Event e)
        {
            GenericMenu menu = new GenericMenu();

            if (selectedNode is StateNode)
            {
                StateNode stateNode = (StateNode) selectedNode;
                if (stateNode.currentState != null)
                {
                    menu.AddSeparator("");
                    menu.AddItem(new GUIContent("Add Transition"), false, ContextCallback,
                        UserActions.AddTransitionNode);
                }
                else
                {
                    menu.AddDisabledItem(new GUIContent("Add Transition"));
                }

                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
            }

            if (selectedNode is TransitionNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
            }

            if (selectedNode is CommentNode)
            {
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Delete"), false, ContextCallback, UserActions.DeleteNode);
            }

            menu.ShowAsContext();
            e.Use();
        }

        void ContextCallback(object o)
        {
            UserActions a = (UserActions) o;
            switch (a)
            {
                case UserActions.AddState:
                    StateNode stateNode = CreateInstance<StateNode>();
                    stateNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 300);
                    stateNode.windowTitle = "State";
                    Windows.Add(stateNode);

                    break;

                case UserActions.AddTransitionNode:
                    if (selectedNode is StateNode)
                    {
                        StateNode from = (StateNode) selectedNode;
                        Transition transition = from.AddTransition();
                        AddTransitionNode(from.currentState.transitions.Count, transition, from);
                    }

                    break;

                case UserActions.CommentNode:
                    CommentNode commentNode = CreateInstance<CommentNode>();
                    commentNode.windowRect = new Rect(mousePosition.x, mousePosition.y, 200, 100);
                    commentNode.windowTitle = "Comment";
                    Windows.Add(commentNode);
                    break;

                case UserActions.DeleteNode:
                    if (selectedNode is StateNode)
                    {
                        StateNode target = (StateNode) selectedNode;
                        target.ClearReferences();
                        Windows.Remove(target);
                    }

                    if (selectedNode is TransitionNode)
                    {
                        TransitionNode target = (TransitionNode) selectedNode;
                        Windows.Remove(target);
                        if (target.enterState.currentState.transitions.Contains(target.targetTransition))
                            target.enterState.currentState.transitions.Remove(target.targetTransition);
                    }

                    if (selectedNode is CommentNode)
                    {
                        Windows.Remove(selectedNode);
                    }

                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Helper Methods

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
            TransitionNode transitionNode = CreateInstance<TransitionNode>();
            transitionNode.Init(from, transition);
            transitionNode.windowRect = new Rect(fromRect.x + 200 + 100, fromRect.y + fromRect.height * .7f, 200, 80);
            transitionNode.windowTitle = "Condition Check";
            Windows.Add(transitionNode);
            from.dependencies.Add(transitionNode);
            return transitionNode;
        }

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
                if (Windows.Contains(list[i]))
                    Windows.Remove(list[i]);
            }
        }

        #endregion
    }
}