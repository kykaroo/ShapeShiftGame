namespace FormStateMachine
{
    public class FormStateBase : IFormState
    {
       public void Enter()
        {
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