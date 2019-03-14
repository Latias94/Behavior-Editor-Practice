using System.Collections.Generic;
using BehaviorEditor;
using UnityEngine;

namespace Behavior
{
    [CreateAssetMenu]
    public class BehaviorGraph : ScriptableObject
    {
        
        public List<BaseNode> windows = new List<BaseNode>();
        #region Checkers

        // 编辑器中是否有重复的 stateNode 出现
        public bool IsStateNodeDuplicate(StateNode node)
        {
            return false;
        }

        #endregion
    }
}