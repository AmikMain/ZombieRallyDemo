using System;
using Unity.VisualScripting;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform zombieTarget;
    [SerializeField] private float fatalSpeed;
    public static Car Instance;
    private ZombieSpawnTrigger zombieSpawnTrigger;

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
    }

    public Vector3 GetZombieTarget()
    {
        return zombieTarget.position;
    }

    void Update()
    {
        zombieTarget.position = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Zombie") && GetComponent<Rigidbody>().linearVelocity.magnitude >= fatalSpeed)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(100);
        }
    }

    public void RemoveFromVisibleZombies(Zombie zombie)
    {
        zombieSpawnTrigger.RemoveFromVisibleZombies(zombie);
    }
}
