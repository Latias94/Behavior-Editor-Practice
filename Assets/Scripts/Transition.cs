using Behavior;

[System.Serializable]
public class Transition
{
    public Condition condition;
    public State targetState;
    public bool disable;
}