using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] private List<FormDescription> effectiveForms;
    
    public IEnumerable<FormDescription> GetForms() => effectiveForms;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<EnemyAi>(out var enemyAi)) return;
        
        enemyAi.OnZoneChange(this);
    }
}