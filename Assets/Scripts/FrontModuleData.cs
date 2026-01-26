using UnityEngine;

[CreateAssetMenu(fileName = "FrontModuleData", menuName = "Scriptable Objects/FrontModuleData")]
public class FrontModuleData : ScriptableObject
{
    public int lvl;
    public float fatalSpeed;
    public bool constantDamage;
    public GameObject prefab;
    public int price;
}
