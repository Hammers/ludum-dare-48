using UnityEngine;

public class DartBehaviour : MonoBehaviour
{
    [SerializeField] private ActiveAbility abilityToTrigger;
    [SerializeField] private float speed;

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            abilityToTrigger?.Trigger(collision.transform);
        Destroy(gameObject);
    }

    
}
