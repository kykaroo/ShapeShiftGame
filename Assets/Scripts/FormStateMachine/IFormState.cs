namespace FormStateMachine
{
    public interface IFormState
    {
        void Enter();
        void Exit();
        string GetFormName();
    }
}