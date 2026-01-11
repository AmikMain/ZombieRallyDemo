using System;
using System.Linq.Expressions;
using UnityEngine;

public class TerrainDetection : MonoBehaviour
{
    public event Action<TerrainType> OnTerrainChanged;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gravel"))
        {
            OnTerrainChanged?.Invoke(TerrainType.Gravel);    
        }
        else if (other.CompareTag("Tarmac"))
        {
            OnTerrainChanged?.Invoke(TerrainType.Tarmac);
        }
    }
}

public enum TerrainType
{
    Gravel,
    Tarmac,
    None,
}