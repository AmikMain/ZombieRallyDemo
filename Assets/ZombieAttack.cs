using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            animator.SetTrigger("Attack");
        }
    }
}
