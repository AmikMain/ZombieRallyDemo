using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TMP_Text lapPercentUIText;
    [SerializeField] TMP_Text zombiesKilledUIText;

    private GameStats gameStats;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        gameStats = GameStats.Instance;
    }

    void Update()
    {
        UpdateLapPercentUI();
        UpdateZombiesKilledUI();
    }

    private void UpdateZombiesKilledUI()
    {
        zombiesKilledUIText.text = gameStats.GetZombiesKilled().ToString();
    }

    private void UpdateLapPercentUI()
    {
        lapPercentUIText.text = Mathf.RoundToInt(gameStats.GetLapPercent()).ToString() + "%"; 
    }
}
