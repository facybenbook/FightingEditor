using UnityEngine;
namespace SuperCU.FightingEngine
{
    public class CharacterBase : MonoBehaviour, IEventable
    {
        public virtual void FixedUpdateGame()
        {
        }
        public virtual void LateUpdateGame()
        {
        }
        public virtual void UpdateGame()
        {
        }
    }
}
