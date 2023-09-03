namespace FormStateMachine
{
    public interface IFormState
    {
        void Enter(FormStateMachine formStateMachine);
        void Exit();
    }
}