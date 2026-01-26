using System;
using UnityEngine;

public class Armor : MonoBehaviour
{
    public string ARMOR_LVL_KEY = "ARMOR_LVL";
    [SerializeField] private ArmorData[] modules;

    void Start()
    {
        //SetArmor(0);
    }

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
}