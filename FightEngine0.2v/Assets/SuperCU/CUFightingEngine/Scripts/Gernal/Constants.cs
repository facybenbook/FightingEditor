using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperCU.FightingEngine
{
#if UNITY_EDITOR
    public static class ConstantsEditor
    {
        //パス定数
        public const string CHARACTER_VIEW_SCENE_PATH = "Assets/SuperCU/CUFightingEngine/Scenes/CharacterView.unity";//キャラクタビューのシーンパス
        public const string CHARACTER_PARANT_RESOURCES_PATH = "Character/CharacterParant";//キャラクタの親オブジェクトのパス(Resources)
        public const string GAME_PROPERTY_RESOURCES_PATH = "Property/GameProperty";

        //座標定数
        public const float CHARACTER_VIEW_PLACE = -300; //キャラクタビューの場所（Ｘ軸）
    }
#endif
}
