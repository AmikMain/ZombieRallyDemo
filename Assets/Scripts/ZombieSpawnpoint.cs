using UnityEngine;

public class ZombieSpawnpoint : MonoBehaviour
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;

    void Start()
    {
        point1.position = Vector3.Lerp(point1.position, this.transform.position, UnityEngine.Random.Range(0f, 1f));
        point2.position = Vector3.Lerp(point2.position, this.transform.position, UnityEngine.Random.Range(0f,1f));
    }

    public void SpawnZombies()
    {
        Instantiate(zombiePrefab, point1.position, Quaternion.identity);
        Instantiate(zombiePrefab, point2.position, Quaternion.identity);
    }
}
