using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawnTrigger : MonoBehaviour
{
    [SerializeField] float zombieNoticingDistance = 40;
    List<GameObject> visibleZombies = new List<GameObject>();

    void Update()
    {
        foreach (GameObject z in visibleZombies)
        {
            if (Vector3.Distance(Car.Instance.transform.position, z.transform.position) <= zombieNoticingDistance)
            {
                z.GetComponent<Zombie>().SetTarget(Car.Instance.GetZombieTarget());
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
                Destroy(other.gameObject);
            }
        }
    }

    public void RemoveFromVisibleZombies(Zombie zombie)
    {
        visibleZombies.Remove(zombie.gameObject);
    }
}
