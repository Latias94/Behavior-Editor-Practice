using UnityEngine;

namespace BehaviorEditor
{
    public class BaseNode: ScriptableObject
    {
        public Rect windowRect;
        public string windowTitle;
        
        public virtual void DrawWindow()
        {
            
        }

        public virtual void DrawCurve()
        {
            
        }
    }
}