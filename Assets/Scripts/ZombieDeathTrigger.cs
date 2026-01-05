using UnityEngine;

public class ZombieDeathTrigger : MonoBehaviour, IDamageTrigger
{
    [SerializeField] private float fatalSpeed = 10;

    public void DealDamage(Collider other, int amount)
    {
        other.gameObject.GetComponent<Health>().TakeDamage(amount);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Zombie") && GetComponentInParent<Rigidbody>().linearVelocity.magnitude >= fatalSpeed)
        {
            DealDamage(other, 100);
        }
    }
}



