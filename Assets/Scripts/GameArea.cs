using System;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    public static event Action OnPlayerLeftGameArea;
    public static List<GameArea> currentGameAreas = new List<GameArea>();

    void OnTriggerEnter(Collider other)
    {
       if (!other.CompareTag("Car")) return;

        currentGameAreas.Add(this); 
    }

    void OnTriggerExit(Collider other)
    {
        
        if (!other.CompareTag("Car")) return;

        currentGameAreas.Remove(this);

        if (currentGameAreas.Count == 0) OnPlayerLeftGameArea?.Invoke();
    }
}
