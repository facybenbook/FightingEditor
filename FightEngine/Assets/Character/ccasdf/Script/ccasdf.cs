using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ccasdf : CharacterBase
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
