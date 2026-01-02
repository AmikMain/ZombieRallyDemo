using UnityEngine;

public class ZombieSpawnpoint : MonoBehaviour
{
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;

    public void SpawnZombies()
    {
        Debug.Log("Spawning zombies between " + point1.position + " and " + point2.position + ".");
        // Add your zombie spawning logic here
    }
}
