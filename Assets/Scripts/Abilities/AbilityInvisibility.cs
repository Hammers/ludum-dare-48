using UnityEngine;

public class AbilityInvisibility : MonoBehaviour
{
    public void Update(){
        if(Input.GetButtonUp("Fire2")){
            Trigger();
        }
    }

    public void Trigger()
    {
        var player = GameObject.Find("PlayerCharacter");
        var detectableSpot = player.transform.Find("DetectableSpot").GetComponent<Collider2D>();
        detectableSpot.enabled = !detectableSpot.enabled;
        var playerSprite = player.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, detectableSpot.enabled ? 1f : 0.5f);
    }
}
