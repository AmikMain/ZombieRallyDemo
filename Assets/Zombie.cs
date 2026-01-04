using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        animator.ResetTrigger("Dead");
        navMeshAgent.isStopped = true;
        isDead = true;
        GetComponentInChildren<RagdollEnabler>().EnableRagdoll();
        GetComponent<CapsuleCollider>().enabled = false;
        Car.Instance.RemoveFromVisibleZombies(this);
        StartCoroutine(BeDestroyed());
    }

    public void SetTarget(Vector3 target)
    {
        if (isDead || navMeshAgent.enabled == false) return;
        navMeshAgent.SetDestination(target);
    }

    IEnumerator BeDestroyed()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
