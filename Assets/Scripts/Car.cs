using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{
    public static Car Instance;
    public event Action OnCarDied;

    [SerializeField] private Transform zombieTarget;
    [SerializeField] private float tarmacSurfaceDriftMp = 2;
    [SerializeField] private float gravelSurfaceDriftMp = 5;
    [SerializeField] private Vector3 lapSpawnPoint;
    [SerializeField] private Vector3 garagePoint;
    [SerializeField] private ParticleSystem RRTarmacParticles;
    [SerializeField] private ParticleSystem RLTarmacParticles;
    [SerializeField] private ParticleSystem RRGravelParticles;
    [SerializeField] private ParticleSystem RLGravelParticles;
    [SerializeField] private AudioSource tarmacAudio;
    [SerializeField] private AudioSource gravelAudio;
    [SerializeField] private AudioSource collisionAudio;
    [SerializeField] private float treeCoollisionVelocity1 = 5;
    [SerializeField] private float treeCoollisionVelocity2 = 15;

    private ZombieSpawnTrigger zombieSpawnTrigger;
    private PrometeoCarController prometeoCarController;
    private TerrainDetection terrainDetection;
    private Health health;
    bool isDead = false;
    

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        Instance = this;
    }

    void OnDisable()
    {
        GameManager.Instance.OnLapStart -= TeleportToLapStartDelayed;
        GameManager.Instance.OnLapReload -= TeleportToGarage;
        terrainDetection.OnTerrainChanged -= ChangeTerrainModifiers;
        health.OnDie -= HandleDeath;
    }

    void Start()
    {
        zombieSpawnTrigger = GetComponentInChildren<ZombieSpawnTrigger>();
        prometeoCarController = GetComponent<PrometeoCarController>();

        terrainDetection = GetComponentInChildren<TerrainDetection>();
        health = GetComponent<Health>();

        GameManager.Instance.OnLapStart += TeleportToLapStartDelayed;
        GameManager.Instance.OnLapReload += TeleportToGarage;
        terrainDetection.OnTerrainChanged += ChangeTerrainModifiers;
        health.OnDie += HandleDeath;
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
        if(isDead) return;

        Debug.Log($"OnCarDied listeners: {OnCarDied?.GetInvocationList().Length}");
        OnCarDied?.Invoke();
        isDead = true;
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

            prometeoCarController.RRWParticleSystem.Stop();
            prometeoCarController.RLWParticleSystem.Stop();
            prometeoCarController.RRWParticleSystem = RRTarmacParticles;
            prometeoCarController.RLWParticleSystem = RLTarmacParticles;

            prometeoCarController.tireScreechSound = tarmacAudio;
        }
        else if (type == TerrainType.Gravel)
        {
            prometeoCarController.surfaceDriftMultiplier = gravelSurfaceDriftMp;

            prometeoCarController.RRWParticleSystem.Stop();
            prometeoCarController.RLWParticleSystem.Stop();
            prometeoCarController.RRWParticleSystem = RRGravelParticles;
            prometeoCarController.RLWParticleSystem = RLGravelParticles;

            prometeoCarController.tireScreechSound = gravelAudio;
        }
    }

    void TeleportToGarage()
    {
        isDead = false;

        transform.position = garagePoint;

        transform.rotation = Quaternion.identity;

        health.ResetHealth();
    }
    
    public void RemoveFromVisibleZombies(Zombie zombie)
    {
        zombieSpawnTrigger.RemoveFromVisibleZombies(zombie);
    }
}
