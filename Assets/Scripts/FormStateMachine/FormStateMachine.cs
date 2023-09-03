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
            _currentState?.Exit();
            _currentState = _states[typeof(T)];
            _currentState.Enter(this);
        }
    }
}