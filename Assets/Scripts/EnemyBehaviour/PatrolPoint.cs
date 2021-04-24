using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public enum PointType{
        LookAt,
        Move
    }
    public PointType pointType;
}
