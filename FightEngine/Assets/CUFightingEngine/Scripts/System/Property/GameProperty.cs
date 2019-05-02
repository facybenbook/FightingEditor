using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace SuperCU
{
    [CreateAssetMenu(menuName = "CUSystem/Create SystemProperty")]
    public class GameProperty : ScriptableObject
    {
        public List<CharacterProperty> characterProperties = new List<CharacterProperty>();       
    }
    
}
