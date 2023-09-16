using System;
using System.Collections.Generic;

namespace FormStateMachine
{
    public class FormStateMachine
    {
        private readonly Dictionary<Type, IFormState> _states;
        private IFormState _currentState;

        public FormStateMachine(Dictionary<Type, IFormState> states)
        {
            _states = states;
        }

        public void SetState<T>() where T : IFormState
        {
            if (_currentState ==_states[typeof(T)]) return;
            
            _currentState?.Exit();
            _currentState = _states[typeof(T)];
            _currentState.Enter(this);
        }

        public IFormState GetCurrentState()
        {
            return _currentState;
        }

        public Dictionary<Type, IFormState> GetStates()
        {
            return _states;
        }

        public void SetState(IFormState form)
        {
            _currentState?.Exit();
            _currentState = _states[form.GetType()];
            _currentState.Enter(this);
        }
    }
}