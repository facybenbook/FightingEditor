#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace SuperCU.FightingEngine
{

    //コードのテンプレート
    public partial class CharacterModelWindow : EditorWindow
    {
        private string codeTemp;
        private string CodeTempCharacter()
        {
            codeTemp = @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperCU.FightingEngine;

public class #CLASS_NAME# : CharacterBase
{
    void Start()
    {
        GameManager.Instance.AddUpdate(this);
    }
    public override void UpdateGame()
    {
        base.UpdateGame();
        //ここに処理
    }
}
";
            return codeTemp;
        }
    }
}
#endif
