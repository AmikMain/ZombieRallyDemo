using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    GameStats gameStats;

    public event Action OnDieByKilling;

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

    void OnEnable()
    {
        gameStats = GameStats.Instance;

        OnDieByKilling += gameStats.HandleZombieKill;
    }

    void OnDisable()
    {
        OnDieByKilling -= gameStats.HandleZombieKill;

        Car.Instance.RemoveFromVisibleZombies(this);
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
    }
    
    void HandleDeath(DeathType type)
    {
        if(type == DeathType.Culling)
        {
            Destroy(this.gameObject);
            Debug.Log("Culling zombie " + UnityEngine.Random.Range(0f , 1f).ToString());
        }
        else if (DeathType.Kill == type)
        {
            isDead = true;
            
            animator.ResetTrigger("Dead");

            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
            
            GetComponentInChildren<RagdollEnabler>().EnableRagdoll();

            GetComponent<CapsuleCollider>().enabled = false;

            Car.Instance.RemoveFromVisibleZombies(this);

            OnDieByKilling?.Invoke();

            StartCoroutine(BeDestroyed());
        }
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

public enum DeathType
{
    Culling,
    Kill
}
