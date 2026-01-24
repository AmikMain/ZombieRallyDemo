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
        OnLapStart += CursorSetUnactive;
        OnLapReload += CursorSetActive;
    }

    void OnDisable()
    {
        OnLapStart -= CursorSetUnactive;
        OnLapReload -= CursorSetActive;
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

    public void CursorSetActive()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void CursorSetUnactive()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
