using UnityEngine;

[CreateAssetMenu(fileName = "EngineData", menuName = "Scriptable Objects/EngineData")]
public class EngineData : ScriptableObject
{
    public int level;
    public int speedAddition;
    public int accelerationAddtion;
    public int price;
}
