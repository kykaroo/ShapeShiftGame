using System;
using UnityEngine;

namespace FormStateMachine
{
    public class FormFactory : MonoBehaviour
    {
        [SerializeField] private Transform[] rootsForForms;

        public T CreateForm<T>(int playerId) where T : MonoBehaviour
        {
            T formPrefab = Resources.Load<T>($"FormsPrefabs/{Convert.ToString(typeof(T).Name)}");
            T form = Instantiate(formPrefab, rootsForForms[playerId]);
            return form;
        }
    }
}