using System;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const float MOVEMENT_SPEED = 10f;
    private Rigidbody2D rb;
    private Vector3 _startPos;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
    }

    private void OnEnable()
    {
        GameManager.Instance.NewSessionStarted += OnSessionRestarted;
    }

    private void OnDisable()
    {
        GameManager.Instance.NewSessionStarted += OnSessionRestarted;
    }
    
    private void OnSessionRestarted()
    {
        transform.position = _startPos;
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticaltInput = Input.GetAxis("Vertical");

        var step = Time.deltaTime * MOVEMENT_SPEED;
        var forces = new Vector2(horizontalInput, verticaltInput) * step;

        rb.AddForce(forces, ForceMode2D.Impulse);
    }
}
