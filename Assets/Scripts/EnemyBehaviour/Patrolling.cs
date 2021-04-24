using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrolling : MonoBehaviour
{
    enum PatrolState{
        None,
        Turning,
        Moving
    }
    // Start is called before the first frame update
    [SerializeField] private Transform[] patrolPoints;
    private PatrolState state = PatrolState.None;
    private int targetPatrolPointIndex = 0;
    private Transform targetPatrolPoint;
    private Rigidbody2D rb;

    // Turning
    private const float ROTATION_SPEED = 2.5f;
    private Vector3 startRot;
    private Vector3 targetRot;
    private float rotProgress;

    // Moving
    private const float MOVEMENT_SPEED = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPatrolPoint = patrolPoints[targetPatrolPointIndex];
        state = PatrolState.Moving;
    }

    private void GetNextTarget()
    {
        targetPatrolPointIndex++;
        if (targetPatrolPointIndex >= patrolPoints.Length)
            targetPatrolPointIndex = 0;
        targetPatrolPoint = patrolPoints[targetPatrolPointIndex];
        
        Debug.Log("Now turning to next target...");
        state = PatrolState.Turning;
        startRot = transform.up;
        targetRot = (targetPatrolPoint.position - transform.position).normalized;
        rotProgress = 0f;
    }

    private void TurnToTarget()
    {
        rotProgress += Time.deltaTime * ROTATION_SPEED;
        if(rotProgress >= 1f){
            transform.up = targetRot;
            state = PatrolState.Moving;
            Debug.Log("Now moving to target...");
        }
        else
            transform.up = Vector3.Slerp(startRot, targetRot, rotProgress);
    }

    private void MoveToTarget()
    {
        if(Vector3.Distance(transform.position, targetPatrolPoint.position) > 0.01f){
            var forces = targetPatrolPoint.position - transform.position;
            forces.Normalize();
            forces *= Time.deltaTime * MOVEMENT_SPEED;
            rb.AddForce(forces);
        }
        else
            state = PatrolState.None;
    }
    // Update is called once per frame
    void Update()
    {
        switch(state){
            case PatrolState.None:
                GetNextTarget();
                break;
            case PatrolState.Turning:
                TurnToTarget();
                break;
            case PatrolState.Moving:
                MoveToTarget();
                break;
        }
    }
}
