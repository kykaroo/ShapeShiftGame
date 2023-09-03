using System;
using UnityEngine;

namespace FormStateMachine
{
    public class FormFactory : MonoBehaviour
    {
        [SerializeField] private Transform rootForForms;

        public T CreateForm<T>() where T : MonoBehaviour
        {
            T formPrefab = Resources.Load<T>($"FormsPrefabs/{Convert.ToString(typeof(T).Name)}");
            T form = Instantiate(formPrefab, rootForForms);
            return form;
        }
    }
}