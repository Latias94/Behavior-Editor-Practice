using System.Collections.Generic;
using UnityEngine;

namespace Behavior
{
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
        public StateActions[] onState;
        public StateActions[] onEnter;
        public StateActions[] onExit;
        public List<Transition> transitions = new List<Transition>();

        public void OnEnter(StateManager states)
        {
            ExecuteActions(states, onEnter);
        }

        public void Tick(StateManager states)
        {
            CheckTransitions(states);
            ExecuteActions(states, onState);
        }

        public void OnExit(StateManager states)
        {
            ExecuteActions(states, onExit);
        }

        public void CheckTransitions(StateManager states)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if(transitions[i].disable)
                    continue;
                // 满足条件的话 就进入下一个状态
                if (transitions[i].condition.CheckCondition(states))
                {
                    if (transitions[i].targetState != null)
                    {
                        states.currentState = transitions[i].targetState;
                        OnExit(states);
                        states.currentState.OnEnter(states);
                    }
                    return;
                }
            }
        }

        public void ExecuteActions(StateManager states, StateActions[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != null)
                {
                    list[i].Execute(states);
                }
            }
        }

        public Transition AddTransition()
        {
            Transition returnVal = new Transition();
            transitions.Add(returnVal);
            return returnVal;
        }
    }
}