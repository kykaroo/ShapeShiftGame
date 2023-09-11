using System;
using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public event Action OnLevelComplete;
    private void OnTriggerEnter(Collider other)
    {
        OnLevelComplete?.Invoke();
    }
}
