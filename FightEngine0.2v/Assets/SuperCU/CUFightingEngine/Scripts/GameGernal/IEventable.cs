namespace SuperCU.FightingEngine
{
    public interface IEventable
    {
        void UpdateGame();
        void LateUpdateGame();
        void FixedUpdateGame();
    }
}