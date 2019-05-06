using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperCU.FightingEngine
{
    [CreateAssetMenu(menuName = "CUSystem/Create SystemProperty")]
    public class GameProperty : ScriptableObject
    {
        public List<CharacterProperty> characterProperties = new List<CharacterProperty>();
    }
}
