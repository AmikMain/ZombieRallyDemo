using System;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public string CAR_ENGINE_LVL_KEY = "CAR_ENGINE_LVL";
    [SerializeField] private EngineData[] engineLevels;
    [SerializeField] int baseMaxSpeed = 40;
    [SerializeField] int baseAcceleration = 4;

    public int maxSpeed = 0;
    public int acceleration = 0;

    PrometeoCarController carController;

    public event Action OnEngineUpdated;

    void Start()
    {
        carController = transform.parent.GetComponent<PrometeoCarController>();

        maxSpeed = baseMaxSpeed;
        acceleration = baseAcceleration;
    }

    void SetEngineLevel(int lvl)
    {
        maxSpeed = baseMaxSpeed + GetEngineDataByLevel(lvl).speedAddition;
        acceleration = baseAcceleration + GetEngineDataByLevel(lvl).accelerationAddtion;

        carController.maxSpeed = maxSpeed;
        carController.accelerationMultiplier = acceleration;

        PlayerPrefs.SetInt(CAR_ENGINE_LVL_KEY, lvl);
    
        OnEngineUpdated?.Invoke();
    }

    public int GetNextEngineLevelPrice() //nullable int
    {
        int currentLevel = PlayerPrefs.GetInt(CAR_ENGINE_LVL_KEY, -1);

        EngineData nextData = GetEngineDataByLevel(currentLevel + 1);

        if (nextData == null)
        {
            return -1; // следующего уровня нет
        }

        return nextData.price;
    }

    public EngineData GetEngineDataByLevel(int lvl)
    {
        return System.Array.Find(engineLevels, m => m.level == lvl); // Analyse
    }

    public void BuyEngine()
    {
        int avaliliableMoney = PlayerPrefs.GetInt(GameStats.Instance.COIN_BANK_AMOUNT , 0);

        int currentArmorLevel = PlayerPrefs.GetInt(CAR_ENGINE_LVL_KEY, -1);

        int nextArmorPrice = GetNextEngineLevelPrice();

        if(nextArmorPrice == -1) return;

        if (nextArmorPrice <= avaliliableMoney)
        {
            SetEngineLevel(currentArmorLevel + 1);

            int moneyLeft = avaliliableMoney - nextArmorPrice;

            PlayerPrefs.SetInt(GameStats.Instance.COIN_BANK_AMOUNT, moneyLeft);

            OnEngineUpdated?.Invoke();
        }
    }
}
