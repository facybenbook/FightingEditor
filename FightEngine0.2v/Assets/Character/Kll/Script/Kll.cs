using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SuperCU.FightingEngine;

public class Kll : CharacterBase
{
    public UnityEvent unity;
    void Start()
    {
        GameManager.Instance.AddUpdate(this);
    }
    public override void UpdateGame()
    {
        base.UpdateGame();
        //ここに処理
    }
    public void aaaa()
    {

    }
}
