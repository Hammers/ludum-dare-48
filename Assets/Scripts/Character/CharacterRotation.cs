using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    private Camera cam;
    private Vector3 updardDirection = new Vector3(0f, 1f, 0f);

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Input.mousePosition;
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -cam.transform.position.y));
        var forward = mouseWorldPos - (Vector2)transform.position;
        forward.Normalize();
        transform.up = forward;
    }
}
