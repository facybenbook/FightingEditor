using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
//コードのテンプレート
public partial class CharacterModelWindow : ScriptableWizard
{
    private string codeTemp;
    private string CodeTemp()
    {
        codeTemp = @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
