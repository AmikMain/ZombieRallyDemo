using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawnTrigger : MonoBehaviour
{
    [SerializeField] float zombieNoticingDistance = 40;
    [SerializeField] float zombieMoanDistance = 5;
    [SerializeField] float zombieSpawnRadius = 150;
    [SerializeField] float initialRadius = .5f;
    List<GameObject> visibleZombies = new List<GameObject>();
    SphereCollider sphereCollider;

    void Start()
    {
        GameManager.Instance.OnLapStart += SetZombieSpawnSphereRadius;
        GameManager.Instance.OnLapReload += HandleLapReload;

        sphereCollider = GetComponent<SphereCollider>();

        sphereCollider.radius = initialRadius;
    }

    void OnDisable()
    {
        GameManager.Instance.OnLapStart -= SetZombieSpawnSphereRadius;
        GameManager.Instance.OnLapReload -= HandleLapReload;
    }

    void Update()
    {
        foreach (GameObject z in visibleZombies)
        {
            float distance = Vector3.Distance(Car.Instance.transform.position, z.transform.position);

            if (distance <= zombieNoticingDistance)
            {
                z.GetComponent<Zombie>().SetTarget(Car.Instance.GetZombieTarget());
            }

            if(distance <= zombieMoanDistance)
            {
                if (Random.Range(0f, 1f) > 0.7)
                {
                    if (z.GetComponent<Zombie>().moanAudioSource != null)
                    {
                        z.GetComponent<Zombie>().moanAudioSource.Play(); 
                    }
                     
                }
                else
                {
                    z.GetComponent<Zombie>().moanAudioSource = null;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieSpawnpoint"))
        {
            ZombieSpawnpoint spawnpoint = other.GetComponent<ZombieSpawnpoint>();
            if (spawnpoint != null)
            {
                spawnpoint.SpawnZombies();
            }
        }
        else if (other.CompareTag("Zombie"))
        {
            visibleZombies.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Zombie z = other.GetComponent<Zombie>();
            visibleZombies.Remove(other.gameObject);

            if (z != null && z.isDead == false)
            {
                z.GetComponent<Health>().TakeDamage(1000, DeathType.Culling);
            }
        }
    }

    void SetZombieSpawnSphereRadius()
    {
        sphereCollider.radius = zombieSpawnRadius;
    }

    void HandleLapReload()
    {
        sphereCollider.radius = initialRadius;

        visibleZombies = new List<GameObject>();
    }

    public void RemoveFromVisibleZombies(Zombie zombie)
    {
        visibleZombies.Remove(zombie.gameObject);
    }
}
