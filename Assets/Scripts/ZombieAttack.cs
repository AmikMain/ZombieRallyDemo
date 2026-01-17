using System;
using System.Collections;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    Animator animator;
    public bool carWithinReach = false;

    void Start()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Car"))
        {
            carWithinReach = true;
            StartCoroutine(HandleAttackAnimation());
        }
    }

    void OnTriggerExit(Collider other)
    {        
        if (other.CompareTag("Car"))
        {
            carWithinReach = false;
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
        if(!carWithinReach) return;
        Car.Instance.GetComponent<Health>().TakeDamage(5, DeathType.Kill);
    }
}
