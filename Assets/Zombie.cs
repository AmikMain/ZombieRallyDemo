using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public bool isDead = false;
    NavMeshAgent navMeshAgent;
    Health healthComponent;
    Animator animator;
    Collider myCollider;

    void Awake()
    {
        healthComponent = GetComponent<Health>();

        healthComponent.OnDie += HandleDeath;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
    }
    
    void HandleDeath()
    {
        animator.SetTrigger("Dead");
        navMeshAgent.isStopped = true;
        isDead = true;
        Car.Instance.RemoveFromVisibleZombies(this);
        StartCoroutine(BeDestroyed());
    }

    public void SetTarget(Vector3 target)
    {
        if (isDead) return;
        navMeshAgent.SetDestination(target);
    }

    IEnumerator BeDestroyed()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
