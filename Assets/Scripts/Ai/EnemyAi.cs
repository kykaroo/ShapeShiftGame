using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FormStateMachine;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ai
{
    using FormStateMachine = FormStateMachine.FormStateMachine;
    
    public class EnemyAi : MonoBehaviour
    {
        private float _successRate;
        private FormStateMachine _formStateMachine;
        private List<IFormState> _allFormStates;
        private Zone _match;
        private float _minReactionTime;
        private float _maxReactionTime;

        public void Initialize(AiDifficulty difficulty, FormStateMachine formStateMachine)
        {
            _formStateMachine = formStateMachine;
            _allFormStates = _formStateMachine.GetStates().Values.Where(f => f.GetFormName() != null).ToList();
        
            SetDifficulty(difficulty);
        }

        public void SetDifficulty(AiDifficulty difficulty)
        {
            switch (difficulty)
            {
                case AiDifficulty.Easy:
                    _successRate = 0.25f;
                    _minReactionTime = 1;
                    _maxReactionTime = 3;
                    break;
                case AiDifficulty.Medium:
                    _successRate = 0.5f;
                    _minReactionTime = 0.75f;
                    _maxReactionTime = 2;
                    break;
                case AiDifficulty.Hard:
                    _successRate = 0.75f;
                    _minReactionTime = 0.25f;
                    _maxReactionTime = 1.5f;
                    break;
                case AiDifficulty.Insane:
                    _successRate = 1f;
                    _minReactionTime = 0;
                    _maxReactionTime = 0.5f;
                    break;
                default:
                    throw new ArgumentException(nameof(difficulty));
            }
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
            StartCoroutine(RetryFormChange(Random.Range(_minReactionTime, _maxReactionTime)));
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
            StartCoroutine(RetryFormChange(Random.Range(_minReactionTime, _maxReactionTime)));
        }
    }
}
