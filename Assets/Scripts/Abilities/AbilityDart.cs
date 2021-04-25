using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Dart")]
public class AbilityDart : Ability
{
    [SerializeField] private GameObject dartPrefab;
    private const float SPAWN_DISTANCE = 1f;

    // Update is called once per frame
    public override void Trigger(Transform shooter)
    {
        var forward = shooter.transform.up.normalized;
        var dart = Instantiate(dartPrefab, shooter.transform.position + (forward * SPAWN_DISTANCE), shooter.transform.rotation);
    }
}
