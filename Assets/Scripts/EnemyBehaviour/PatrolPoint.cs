using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public enum PointType{
        LookAt,
        LookAtAndMoveTo,
        MoveTo,
    }
    public PointType pointType;
    public float delay;
}
