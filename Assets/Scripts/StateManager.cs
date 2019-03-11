using Behavior;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public float health;
    public State currentState;

    [HideInInspector] public float delta;
    [HideInInspector] public Transform mTransform;

    private void Start()
    {
        mTransform = this.mTransform;
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Tick(this);
        }
    }
}