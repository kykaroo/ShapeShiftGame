using UnityEngine;

namespace FormStateMachine.Forms
{
    [CreateAssetMenu(menuName = "Forms/FormDescription", fileName = "Form Description", order = 0)]
    public class FormDescription : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Priority { get; private set; }
    }
}