using UnityEngine;

namespace Behavior
{
    [CreateAssetMenu(menuName = "Actions/Test/Add Health")]
    public class ChangeHealth : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.health += 10;
        }
    }
}