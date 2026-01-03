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
            visibleZombies.Remove(other.gameObject);
            Destroy(other.gameObject);
        }  
    }

    IEnumerator DestroyZombieAfterFrame(Collider other)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(other.gameObject);
        Debug.Log("Should destroy zombie");
    }
}
