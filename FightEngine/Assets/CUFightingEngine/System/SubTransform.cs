//
// 17CU0235 村上一真
// Transformの拡張クラス
//
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace SuperCU
{
    public static class SubTransform
    {
        //子の子も含めた子を全部返す(親は返さない)
        static public List<Transform> ChildClass(Transform obj)
        {
            Transform child = obj;
            List<Transform> children = new List<Transform>();
            //子要素がいなければ終了
            if (child.childCount == 0)
            {
                return null;
            }
            foreach (Transform ob in child)
            {
                children.Add(ob);
                List<Transform> lis = ChildClass(ob);
                if (lis != null)
                {
                    foreach (Transform transform in lis)
                    {
                        children.Add(transform);
                    }
                }
            }
            return children;
        }
    }
}