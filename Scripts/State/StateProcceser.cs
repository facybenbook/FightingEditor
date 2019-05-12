//ステートを管理するクラス
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateProcceser
{
    //ステート本体
    private StateBase _State = null;
    public StateBase State
    {
        set { _State = value; }
        get { return _State; }
    }
    // ステート移行時に一度だけ
    public void Execute()
    {
        State.Execute();
    }
    // ステート
    public void PlayUpdate()
    {
        State.PlayUpdate();
    }

}
