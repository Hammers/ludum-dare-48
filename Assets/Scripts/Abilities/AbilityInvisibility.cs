using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Invisibility")]
public class AbilityInvisibility : ActiveAbility
{
    public float activeTime;
    
    public override void Trigger(Transform target)
    {
        AbilityManager.instance.RunCoroutine(Perform(target));
    }

    private IEnumerator Perform(Transform target)
    {
        var detectableSpot = target.transform.Find("DetectableSpot").GetComponent<Collider2D>();
        var playerSprite = target.transform.Find("Sprite").GetComponent<SpriteRenderer>();
        detectableSpot.enabled = false;
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0.5f);
        yield return new WaitForSeconds(startingUses);
        detectableSpot.enabled = true;
        playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
    }
}
