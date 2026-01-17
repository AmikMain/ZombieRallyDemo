using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform zombieTarget;
    [SerializeField] private float tarmacSurfaceDriftMp = 2;
    [SerializeField] private float gravelSurfaceDriftMp = 5;
    [SerializeField] private Vector3 lapSpawnPoint;

    public static Car Instance;
    private ZombieSpawnTrigger zombieSpawnTrigger;
    private PrometeoCarController prometeoCarController;
    private TerrainDetection terrainDetection;
    private Health health;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        terrainDetection = GetComponentInChildren<TerrainDetection>();
        health = GetComponent<Health>();

        GameManager.Instance.OnLapStart += TeleportToLapStartDelayed;
        terrainDetection.OnTerrainChanged += ChangeTerrainModifiers;
        health.OnDie += HandleDeath;
    }

    void OnDisable()
    {
        GameManager.Instance.OnLapStart += TeleportToLapStartDelayed;
        terrainDetection.OnTerrainChanged -= ChangeTerrainModifiers;
        health.OnDie -= HandleDeath;
    }

    void Start()
    {
        zombieSpawnTrigger = GetComponentInChildren<ZombieSpawnTrigger>();
        prometeoCarController = GetComponent<PrometeoCarController>();
    }

    private void TeleportToLapStartDelayed()
    {
        Invoke(nameof(TeleportToLapStart), .5f );
    }

    private void TeleportToLapStart()
    {
        transform.position = lapSpawnPoint;
    }

    private void HandleDeath(DeathType type)
    {
        Debug.Log("CAR DIED");
    }

    public Vector3 GetZombieTarget()
    {
        return zombieTarget.position;
    }

    void Update()
    {
        zombieTarget.position = transform.position;
    }

    void ChangeTerrainModifiers(TerrainType type)
    {
        if (type == TerrainType.Tarmac)
        {
            prometeoCarController.surfaceDriftMultiplier = tarmacSurfaceDriftMp;
        }
        else if (type == TerrainType.Gravel)
        {
            prometeoCarController.surfaceDriftMultiplier = gravelSurfaceDriftMp;
        }
    }

    
    public void RemoveFromVisibleZombies(Zombie zombie)
    {
        zombieSpawnTrigger.RemoveFromVisibleZombies(zombie);
    }
}
