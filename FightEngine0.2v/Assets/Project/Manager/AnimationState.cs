//キャラクタごとにステートを入れる
//MEMO:エディタに派生クラスを作ったら記述する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : StateMachineMonoBehaiviour
{ 
    public CharacterProperty characterProperty;

    protected override void Start()
    {
        //ステートの処理
        InitState();
        //ここに処理
    }
    public override void UpdateGame()
    {
        //ここに処理
        base.UpdateGame();
    }
    private void InitState()
    {
        if (characterProperty != null)
        {
            states = characterProperty.States;
        }
        base.Start();
    }
}
