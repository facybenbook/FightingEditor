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
}
";
        return codeTemp;
    }
}
