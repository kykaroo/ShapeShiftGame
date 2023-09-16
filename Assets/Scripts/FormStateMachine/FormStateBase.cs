namespace FormStateMachine
{
    public class FormStateBase : IFormState
    {
        protected FormStateMachine StateMachine;
        
        public void Enter(FormStateMachine formStateMachine)
        {
            StateMachine = formStateMachine;
            OnEnter();
        }

        public void Exit()
        {
            OnExit();
        }
        
        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        public virtual string GetFormName()
        {
            return null;
        }
    }
}