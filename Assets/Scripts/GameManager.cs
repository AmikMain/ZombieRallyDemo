using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action OnLapStart;
    public event Action OnLapReload;
    public GameObject zombieParent;
    public bool canReloadLap;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        zombieParent = FindAnyObjectByType<ZombieParent>().gameObject;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Invoke(nameof(StartLap), 2f);
    }

    void Update()
    {
        CheckLapReload();
    }

    private void CheckLapReload()
    {
        if(!canReloadLap) return;
        if(Input.anyKeyDown)
        {
            ReloadLap();
        }
    }

    public void StartLap()
    {
        OnLapStart?.Invoke();
    }

    public void ReloadLap()
    {
        canReloadLap = false;
        OnLapReload?.Invoke();

        Zombie[] zombies = zombieParent.GetComponentsInChildren<Zombie>();
        foreach (Zombie zombie in zombies)
        {
            Destroy(zombie.gameObject);
        }
    }

}
