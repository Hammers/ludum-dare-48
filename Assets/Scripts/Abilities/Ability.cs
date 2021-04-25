using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public Sprite icon;
    public abstract void Trigger(Transform source);
}
