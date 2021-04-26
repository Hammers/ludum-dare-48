using System;
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
    [SerializeField] public float _searchTime = 10f;
    [SerializeField] private float _alertTime = 2f;
    public float angle = 45f;
    public float distance = 1f;
    private float halfAngle;

    public float currentSearchingTime = 0f;
    private float currentAlertTime = 0f;
    private CharacterDeath _character;
    private Patrolling _patrolling;
    public VisionState state = VisionState.Normal;

    // Start is called before the first frame update
    void Start()
    {
        halfAngle = angle * 0.5f;
        _character = FindObjectOfType<CharacterDeath>();
        _patrolling = GetComponent<Patrolling>();
        var cone = GameObject.Instantiate(visionConePrefab, Vector3.zero, Quaternion.identity);
        cone.GetComponent<VisionCone>().vision = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.NewSessionStarted += OnNewSession;
    }

    private void OnDisable()
    {
        GameManager.Instance.NewSessionStarted -= OnNewSession;
    }
    
    private void OnNewSession()
    {
        currentSearchingTime = 0f;
        currentAlertTime = 0f;
        if (_patrolling != null)
        {
            _patrolling.StopHunting();
        }

        state = VisionState.Normal;
    }

    public VisionState ResolveSeenState(bool playerIsSeen, float deltaTime)
    {
        switch(state){
            case VisionState.Normal:
                if (playerIsSeen){
                    state = VisionState.Seen;
                    if(_patrolling != null)
                        _patrolling.Hunt(_character.transform);
                }
                break;
            case VisionState.Seen:
                if(playerIsSeen){
                    currentAlertTime += Time.deltaTime;
                    if(currentAlertTime >= _alertTime){
                        _character.Trigger();
                        state = VisionState.Alert;
                    }
                }
                else{
                    if(_patrolling != null)
                        _patrolling.StopHunting();
                    state = VisionState.Searching;
                }
                break;
            case VisionState.Searching:
                if(playerIsSeen){
                    currentSearchingTime = 0f;
                    state = VisionState.Seen;
                    if(_patrolling != null)
                        _patrolling.Hunt(_character.transform);
                }
                else
                {
                    if (currentAlertTime > 0f)
                    {
                        currentAlertTime -= Time.deltaTime;
                    }
                    else
                    {
                        currentAlertTime = 0f;
                    }
                    currentSearchingTime += Time.deltaTime;
                    if(currentSearchingTime >= _searchTime){
                        currentSearchingTime = 0f;
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