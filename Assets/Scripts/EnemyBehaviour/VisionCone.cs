using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private float angle = 45f;
    private float halfAngle;
    private Transform target;
    [SerializeField] private Image cone;
    private Camera cam;

    private Color defaultColour = new Color(0.2429245f, 0.5f, 0.2667981f, 0.5f);
    private Color alertedColour = new Color(1, 0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        halfAngle = angle * 0.5f;
        target = GameObject.Find("PlayerCharacter").transform;
        cam = Camera.main;

        float angleRatio = angle / 360f;
        cone.fillAmount = angleRatio;
        cone.rectTransform.Rotate(new Vector3(0, 0, halfAngle));
        cone.color = defaultColour;
    }

    private void ResolveSeenState(bool playerIsSeen){
        if(playerIsSeen){
            cone.color = alertedColour;
        }
        else cone.color = defaultColour;
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