using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const float MOVEMENT_SPEED = 10f;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticaltInput = Input.GetAxis("Vertical");

        Debug.Log(horizontalInput.ToString());
        Debug.Log(verticaltInput.ToString());

        var step = Time.deltaTime * MOVEMENT_SPEED;
        var currPos = transform.position;
        currPos += (new Vector3(horizontalInput, verticaltInput, 0f) * step);
        transform.position = currPos;
    }
}
