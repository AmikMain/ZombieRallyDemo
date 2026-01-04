using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform zombieTarget;

    public static Car Instance;
    private ZombieSpawnTrigger zombieSpawnTrigger;
    private PrometeoCarController prometeoCarController;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        Instance = this;
    }

    void Start()
    {
        zombieSpawnTrigger = GetComponentInChildren<ZombieSpawnTrigger>();
        prometeoCarController = GetComponent<PrometeoCarController>();
    }

    public Vector3 GetZombieTarget()
    {
        return zombieTarget.position;
    }

    void Update()
    {
        zombieTarget.position = transform.position;

        if(Input.GetKey(KeyCode.F))
        {
            prometeoCarController.surfaceDriftMultiplier = 10f;
        }
        else
        {
            prometeoCarController.surfaceDriftMultiplier = 0.3f;
        }

    }

    

    public void RemoveFromVisibleZombies(Zombie zombie)
    {
        zombieSpawnTrigger.RemoveFromVisibleZombies(zombie);
    }
}
