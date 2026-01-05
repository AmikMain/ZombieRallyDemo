using UnityEngine;

[CreateAssetMenu(fileName = "FrontModuleData", menuName = "Scriptable Objects/FrontModuleData")]
public class FrontModuleData : ScriptableObject
{
    public FrontModuleType frontModuleType;
    public float fatalSpeed;
    public bool constantDamage;
    public GameObject prefab;
}
