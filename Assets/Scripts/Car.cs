using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField] private Transform zombieTarget;
    [SerializeField] private float tarmacSurfaceDriftMp = 2;
    [SerializeField] private float gravelSurfaceDriftMp = 5;

    public static Car Instance;
    private ZombieSpawnTrigger zombieSpawnTrigger;
    private PrometeoCarController prometeoCarController;
    private TerrainDetection terrainDetection;

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
        terrainDetection.OnTerrainChanged += ChangeTerrainModifiers;
    }

    void OnDisable()
    {
        terrainDetection.OnTerrainChanged -= ChangeTerrainModifiers;
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
