using UnityEngine;

public class ZombieRagdollTrigger : MonoBehaviour
{
    [SerializeField] private float fatalSpeed = 10;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Zombie") && GetComponentInParent<Rigidbody>().linearVelocity.magnitude >= fatalSpeed)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(100);
        }
    }
}
