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
    public float distance = 1f;
    private float halfAngle;
    private Transform target;
    private Camera cam;

    private const float SEARCH_TIME = 10f;
    private const float ALERT_TIME = 5f;
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

    public VisionState ResolveSeenState(bool playerIsSeen, float deltaTime)
    {
        Patrolling patrolScript = null;
        switch(state){
            case VisionState.Normal:
                if (playerIsSeen){
                    state = VisionState.Seen;
                    patrolScript = GetComponent<Patrolling>();
                    if(patrolScript != null)
                        patrolScript.Hunt(GameObject.Find("PlayerCharacter").transform);
                }
                break;
            case VisionState.Seen:
                if(playerIsSeen){
                    alertTime += Time.deltaTime;
                    if(alertTime >= ALERT_TIME){
                        GameObject.Find("PlayerCharacter").GetComponent<CharacterDeath>().Trigger();
                        state = VisionState.Alert;
                    }
                }
                else{
                    patrolScript = GetComponent<Patrolling>();
                    if(patrolScript != null)
                        patrolScript.StopHunting();
                    state = VisionState.Searching;
                }
                break;
            case VisionState.Searching:
                if(playerIsSeen){
                    searchingTime = 0f;
                    state = VisionState.Seen;
                    patrolScript = GetComponent<Patrolling>();
                    if(patrolScript != null)
                        patrolScript.Hunt(GameObject.Find("PlayerCharacter").transform);
                }
                else
                {
                    searchingTime += Time.deltaTime;
                    if(searchingTime >= SEARCH_TIME){
                        searchingTime = 0f;
                        state = VisionState.Normal;
                    }
                }
                break;
            case VisionState.Alert:
                break;
        }
        return state;
    }
}