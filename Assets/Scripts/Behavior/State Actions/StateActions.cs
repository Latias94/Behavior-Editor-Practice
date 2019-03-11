using UnityEngine;

namespace Behavior
{
    public abstract class StateActions : ScriptableObject
    {
        public abstract void Execute(StateManager states);
    }
}