using System;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public string ARMOR_LVL_KEY = "ARMOR_LVL";
    [SerializeField] private ArmorData[] modules;

    public event Action OnArmorLevelUpdated;

    private void SetArmor(int lvl)
    {
        SpawnArmorPrefabs(lvl);

        ChangeHealth(lvl);
    }

    private void ChangeHealth(int lvl)
    {
        Transform parent = this.transform.parent;

        Health health = parent.GetComponentInChildren<Health>();

        int totalHealthAdditon = 0;

        for(int i = 0; i <= lvl; i++ )
        {
            totalHealthAdditon += GetModuleByLevel(lvl - i).additionalHealth;
        }

        health.AddToMaxHealth(totalHealthAdditon); 
    }

    private void SpawnArmorPrefabs(int lvl)
    {
        //clean everything first
        Transform[] oldArmorTransforms = GetComponentsInChildren<Transform>();

        foreach (Transform t in oldArmorTransforms)
        {
            if (t == transform) continue;
            Destroy(transform.gameObject);
        }

        //spawn new ones

        for(int i = 0; i <= lvl; i++ )
        {
            GameObject prefab = GetModuleByLevel(lvl - i).prefab;
            
            Instantiate(prefab, this.gameObject.transform);
        }

    }

    public ArmorData GetModuleByLevel(int lvl)
    {
        return System.Array.Find(modules, m => m.armorLevel == lvl); // Analyse
    }

    public void BuyArmor()
    {
        int avaliliableMoney = PlayerPrefs.GetInt(GameStats.Instance.COIN_BANK_AMOUNT , 0);

        int currentArmorLevel = PlayerPrefs.GetInt(ARMOR_LVL_KEY, -1);

        int nextArmorPrice = GetNextArmorLevelPrice();

        if (nextArmorPrice <= avaliliableMoney)
        {
            SetArmor(currentArmorLevel + 1);

            int moneyLeft = avaliliableMoney - nextArmorPrice;

            PlayerPrefs.SetInt(GameStats.Instance.COIN_BANK_AMOUNT, moneyLeft);

            OnArmorLevelUpdated?.Invoke();
        }
    }

    public int GetNextArmorLevelPrice()
    {
        Debug.Log(PlayerPrefs.HasKey(ARMOR_LVL_KEY));
        int currentFrontModuleLevel = PlayerPrefs.GetInt(ARMOR_LVL_KEY, -1);
        Debug.Log(currentFrontModuleLevel);
        int nextFrontModulePrice = GetModuleByLevel(currentFrontModuleLevel + 1).price;

        return nextFrontModulePrice;
    }
}