using UnityEngine;

[CreateAssetMenu(fileName = "ArmorData", menuName = "Scriptable Objects/ArmorData")]
public class ArmorData : ScriptableObject
{
    public int armorLevel;
    public int additionalHealth;
    public GameObject prefab;
    public int price;
}