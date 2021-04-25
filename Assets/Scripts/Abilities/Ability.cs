using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public Sprite icon;
    public int cost;
    public abstract void Trigger(Transform source);
}
