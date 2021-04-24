using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    public int Width => width;
    public int Height => height;
}