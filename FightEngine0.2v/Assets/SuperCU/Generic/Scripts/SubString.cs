//
// 17CU0235 村上一真
// 拡張string
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperCU.Generic
{
    public static class SubString
    {
        //一文字目を大文字化
        public static string OneUpper(string s)
        {
            string temp = s[0].ToString().ToUpper();
            for (int i = 1; i < s.Length; i++)
            {
                temp += s[i].ToString();
            }
            return temp;
        }
    }
}
