using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ability/SingleEMP")]
public class AbilityEMP : ActiveAbility
{
    [SerializeField] private float disabledTime;
    public override void Trigger(Transform target)
    {
        AbilityManager.instance.RunCoroutine(PerformEmp(target));
    }

    private IEnumerator PerformEmp(Transform target)
    {
        var vision = target.GetComponent<Vision>();
        var patrolling = target.GetComponent<Patrolling>();
        var sprite = target.GetComponent<SpriteRenderer>();
        if(vision != null) vision.enabled = false;
        if(patrolling != null) patrolling.enabled = false;
        if(sprite) sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        yield return new WaitForSeconds(disabledTime);
        if(vision != null) vision.enabled = true;
        if(patrolling != null) patrolling.enabled = true;
        if(sprite) sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
    }
}
