using System.Collections.Generic;
using BehaviorEditor;
using UnityEngine;

namespace Behavior
{
    [CreateAssetMenu]
    public class BehaviorGraph : ScriptableObject
    {
        public List<Saved_StateNode> savedStateNodes = new List<Saved_StateNode>();
        Dictionary<StateNode, Saved_StateNode> stateNodesDict = new Dictionary<StateNode, Saved_StateNode>();
        Dictionary<State, StateNode> stateDicts = new Dictionary<State, StateNode>();

        public void Init()
        {
            stateNodesDict.Clear();
            stateDicts.Clear();
        }

        #region State Nodes

        public void SetStateNode(StateNode node)
        {
            Saved_StateNode s = GetSavedState(node);
            if (s == null)
            {
                s = new Saved_StateNode();
                savedStateNodes.Add(s);
                stateNodesDict.Add(node, s);
            }

            s.state = node.currentState;
            s.position = new Vector2(node.windowRect.x, node.windowRect.y);
        }

        public void ClearStateNode(StateNode node)
        {
            Saved_StateNode s = GetSavedState(node);
            if (s != null)
            {
                savedStateNodes.Remove(s);
                stateNodesDict.Remove(node);
            }
        }

        Saved_StateNode GetSavedState(StateNode node)
        {
            Saved_StateNode r;
            stateNodesDict.TryGetValue(node, out r);
            return r;
        }

        StateNode GetStateNode(State state)
        {
            StateNode r;
            stateDicts.TryGetValue(state, out r);
            return r;
        }

        #endregion
    }

    [System.Serializable]
    public class Saved_StateNode
    {
        public State state;
        public Vector2 position;
    }

    [System.Serializable]
    public class Saved_Transition
    {
    }
}