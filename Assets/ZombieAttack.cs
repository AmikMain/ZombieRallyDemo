using System;
using System.Collections;
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
            StartCoroutine(HandleAttackAnimation());
        }
    }

    IEnumerator HandleAttackAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        if (!GetComponentInParent<Zombie>().isDead)
        {
            animator.SetTrigger("Attack");  
        }
        
    }

    public void OnAttack()
    {
        Car.Instance.GetComponent<Health>().TakeDamage(5);
    }
}
