using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const float MOVEMENT_SPEED = 10f;
    private Rigidbody2D rb;
    private Vector3 _startPos;

    private float currMovementSpeed;

    void Start()
    {
        currMovementSpeed = MOVEMENT_SPEED;
        rb = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
    }

    private void DeriveMovementSpeed()
    {
        float movementSpeed = MOVEMENT_SPEED;
        foreach(var multiplier in movementMultipliers)
            movementSpeed *= multiplier.Value;
        
        foreach(var addition in movementAddends)
            movementSpeed += addition.Value;

        currMovementSpeed = movementSpeed;
    }
    private Dictionary<string, float> movementAddends = new Dictionary<string, float>();
    private Dictionary<string, float> movementMultipliers = new Dictionary<string, float>();

    public void AddMovementAddend(string key, float amount){
        movementAddends.Add(key, amount);
        DeriveMovementSpeed();
    }

    public void RemoveMovementAddend(string key){
        movementAddends.Remove(key);
        DeriveMovementSpeed();
    }

    public void AddMovementMultiplier(string key, float amount){
        movementMultipliers.Add(key, amount);
        DeriveMovementSpeed();
    }

    public void RemoveMovementMultiplier(string key){
        movementMultipliers.Remove(key);
        DeriveMovementSpeed();
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

        var step = Time.deltaTime * currMovementSpeed;
        var forces = new Vector2(horizontalInput, verticaltInput) * step;

        rb.AddForce(forces, ForceMode2D.Impulse);
    }
}
