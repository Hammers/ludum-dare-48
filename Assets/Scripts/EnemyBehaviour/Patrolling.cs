using UnityEngine;

public class Patrolling : MonoBehaviour
{
    enum PatrolState{
        None,
        Turning,
        Moving,
        Hunting
    }
    // Start is called before the first frame update
    [SerializeField] private PatrolPoint[] patrolPoints;
    private PatrolState state = PatrolState.None;
    private int targetPatrolPointIndex = 0;
    private PatrolPoint targetPatrolPoint;
    private Rigidbody2D rb;

    // Turning
    [SerializeField] private float rotationSpeed = 2.5f;
    [SerializeField] private float huntSpeed = 5f;
    [SerializeField] private float searchSpeedModifier = 1.5f;
    private Vector3 startRot;
    private Vector3 targetRot;
    private float rotProgress = 0f;
    private Transform huntTarget;
    private float delayTimer;

    // Moving
    [SerializeField] private float movementSpeed = 100f;

    void Start()
    {
        Debug.Log("Start...");
        rb = GetComponent<Rigidbody2D>();
        targetPatrolPoint = patrolPoints[targetPatrolPointIndex];
        state = PatrolState.Turning;
        PrepareToTurn();
    }

    private void GetNextTarget()
    {
        targetPatrolPointIndex++;
        if (targetPatrolPointIndex >= patrolPoints.Length)
            targetPatrolPointIndex = 0;
        targetPatrolPoint = patrolPoints[targetPatrolPointIndex];
        
        Debug.Log("Now turning to next target...");
        state = PatrolState.Turning;
        PrepareToTurn();
    }

    private void PrepareToTurn()
    {
        startRot = transform.up;
        targetRot = (new Vector3(targetPatrolPoint.transform.position.x, targetPatrolPoint.transform.position.y, 0f) - transform.position).normalized;
        rotProgress = 0f;
    }

    private void TurnToTarget()
    {
        var progressStep = Time.deltaTime * rotationSpeed;
        if(GetComponent<Vision>().state == Vision.VisionState.Searching)
            progressStep *= searchSpeedModifier;

        rotProgress += progressStep;
        if(rotProgress >= 1f){
            if (targetPatrolPoint.pointType == PatrolPoint.PointType.Move){
                transform.up = targetRot;
                state = PatrolState.Moving;
            }
            else{
                state = PatrolState.None;
            }
        }
        else{

            transform.up = Vector3.Slerp(startRot, targetRot, rotProgress);
        }
    }
    private void TurnToHuntTarget()
    {
        Quaternion targetRot = Quaternion.LookRotation(transform.up, huntTarget.position - transform.position);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRot, huntSpeed * Time.deltaTime);
        if(Quaternion.Angle(transform.rotation, targetRot) <= 1f)
            rotation = targetRot;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotation.eulerAngles.z));
    }

    private void MoveToTarget()
    {
        if(Vector3.Distance(transform.position, targetPatrolPoint.transform.position) > 0.01f){
            var forces = targetPatrolPoint.transform.position - transform.position;
            forces.Normalize();
            var forceStrength = Time.deltaTime * movementSpeed;
            if(GetComponent<Vision>().state == Vision.VisionState.Searching)
                forceStrength *= searchSpeedModifier;

            forces *= forceStrength;
            rb.AddForce(forces);
        }
        else
            state = PatrolState.None;
    }
    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case PatrolState.None:
                if(targetPatrolPoint != null
                && targetPatrolPoint.delay > 0
                && delayTimer < targetPatrolPoint.delay){
                    delayTimer += Time.deltaTime;
                }
                else{
                    delayTimer = 0f;
                    GetNextTarget();
                }
                break;
            case PatrolState.Turning:
                TurnToTarget();
                break;
            case PatrolState.Moving:
                MoveToTarget();
                break;
            case PatrolState.Hunting:
                TurnToHuntTarget();
                break;
        }
    }

    public void Hunt(Transform target)
    {
        Debug.Log("Hunting...");
        state = PatrolState.Hunting;
        huntTarget = target;
    }

    public void StopHunting()
    {
        Debug.Log("Stop Hunting...");
        if(state == PatrolState.Hunting)
            state = PatrolState.Turning;
        huntTarget = null;
    }
}
