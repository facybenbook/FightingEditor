using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//インプットの種類
public enum StateInputMode
{
    GetKeyDown,
    GetKey,
}
//リストで使用
[System.Serializable]
public struct NextStateJudge
{
    public StateInputMode eInputMode;
    public string nextState;//次に入れ替わることの出来るステートの名前
    public string key;//入力キー
    public int priority;//優先度
}

