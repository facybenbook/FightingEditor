//-----------------------------------------------------
//  入力されたキー情報を取得
//----------------------------------------------------
//  作成者:高野
//  更新日:04/27/2019
//　更新日:05/08/2019 名前空間内に入れstaticに変更　by村上
//----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SuperCU.Generic
{
    public static class GetKeyboardValue
    {
        public static string DownKeyCheck()
        {
            if (Input.anyKeyDown)
            {
                foreach (var code in Input.inputString)
                {
                    Debug.Log(code);
                    return code.ToString();
                }
            }
            return " ";
        }
        public static string KeyCheck()
        {
            if (Input.anyKey)
            {
                foreach (var code in Input.inputString)
                {
                    Debug.Log(code);
                    return code.ToString();
                }
            }
            return " ";
        }

    }
}
