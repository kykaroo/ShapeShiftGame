using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FormStateMachine;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private AiDifficulty _difficulty;
    private float _successRate;
    private FormStateMachine.FormStateMachine _formStateMachine;
    private List<IFormState> _allFormStates;
    private Zone _match;
    
    public void Initialize(AiDifficulty difficulty, FormStateMachine.FormStateMachine formStateMachine)
    {
        _difficulty = difficulty;
        _formStateMachine = formStateMachine;
        _allFormStates = _formStateMachine.GetStates().Values.Where(f => f.GetFormName() != null).ToList();

        _successRate = _difficulty switch
        {
            AiDifficulty.Easy => 0.25f,
            AiDifficulty.Medium => 0.5f,
            AiDifficulty.Hard => 0.75f,
            AiDifficulty.Insane => 1f,
            _ => _successRate
        };
    }
    
    private void ChangeForm()
    {
        var random = Random.Range(0f, 1f);
        if (_successRate >= random)
        {
            var formDescription = _match.GetForms()
                .Where(f => _formStateMachine.GetStates().Any(form => form.Value.GetFormName() == f.Name))
                .OrderBy(f => f.Priority)
                .First();

            var form = _formStateMachine.GetStates().First(f => f.Value.GetFormName() == formDescription.Name).Value;

            if (_formStateMachine.GetCurrentState() == form) return;
            
            _formStateMachine.SetState(form);
            return;
        }
        
        ChangeToRandomForm();
        StartCoroutine(RetryFormChange(Random.Range(1f, 3f)));
    }

    private void ChangeToRandomForm()
    {
        var randomForm = _allFormStates[Random.Range(0, _allFormStates.Count)];
        _formStateMachine.SetState(randomForm);
    }

    private IEnumerator RetryFormChange(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeForm();
    }

    public void OnZoneChange(Zone match)
    {
        StopAllCoroutines();
        _match = match;
        StartCoroutine(RetryFormChange(Random.Range(0f, 1f)));
    }
}
