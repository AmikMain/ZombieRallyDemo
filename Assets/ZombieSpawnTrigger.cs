using UnityEngine;

public class ZombieSpawnTrigger : MonoBehaviour
{
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
    }
}
