using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperCU.FightingEngine;

public class Sadaw : CharacterBase
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
