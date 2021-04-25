using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Invisibility")]
public class AbilityInvisibility : Ability
{
    public override void Trigger(Transform target)
    {
        var detectableSpot = target.transform.Find("DetectableSpot").GetComponent<Collider2D>();
        detectableSpot.enabled = !detectableSpot.enabled;
        var playerSprite = target.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, detectableSpot.enabled ? 1f : 0.5f);
    }
}
