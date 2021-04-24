using UnityEngine;

public class Vision : MonoBehaviour
{
    public enum VisionState{
        Normal,
        Seen,
        Searching,
        Alert
    }

    [SerializeField] private GameObject visionConePrefab;
    public float angle = 45f;
    public float distance = 20f;
    private float halfAngle;
    private Transform target;
    private Camera cam;

    private const float SEARCH_TIME = 5f;
    private const float ALERT_TIME = 2f;
    private float searchingTime = 0f;
    private float alertTime = 0f;

    public VisionState state = VisionState.Normal;

    // Start is called before the first frame update
    void Start()
    {
        halfAngle = angle * 0.5f;
        target = GameObject.Find("PlayerCharacter").transform;
        cam = Camera.main;

        float angleRatio = angle / 360f;
        var cone = GameObject.Instantiate(visionConePrefab, Vector3.zero, Quaternion.identity);
        cone.GetComponent<VisionCone>().vision = this;
    }

    private void ResolveSeenState(bool playerIsSeen){
        switch(state){
            case VisionState.Normal:
                if (playerIsSeen)
                    state = VisionState.Seen;
                break;
            case VisionState.Seen:
                if(playerIsSeen){
                    alertTime += Time.deltaTime;
                    if(alertTime >= ALERT_TIME)
                        state = VisionState.Alert;
                }
                else
                    state = VisionState.Searching;
                break;
            case VisionState.Searching:
                if(playerIsSeen){
                    state = VisionState.Seen;
                }
                else
                {
                    searchingTime += Time.deltaTime;
                    if(searchingTime >= SEARCH_TIME)
                        state = VisionState.Normal;
                }
                break;
            case VisionState.Alert:
                Debug.Log("You dead, bro");
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPos = new Vector2(target.position.x, target.position.y);

        var currAngle = Vector2.Angle(transform.up.normalized, (targetPos - pos).normalized);

        if(currAngle <= halfAngle){
            Debug.Log("Found player..");
            // Check for any blocking objects between this and the target
            RaycastHit2D hit = Physics2D.Raycast(pos, targetPos - pos);
            if(hit.collider == null){
                //Debug.Log("Nothing between Player...");
                ResolveSeenState(true);
            }
            else if(hit.collider.tag == "Player"){
                //Debug.Log("Found Player...");
                ResolveSeenState(true);
            }
            else{
                //Debug.Log($"Blocked sight by {hit.collider.name}...");
                ResolveSeenState(false);
            }
        }
        else {
            //Debug.Log("Not the right angle..");
            ResolveSeenState(false);
        }
    }
}