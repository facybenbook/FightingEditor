using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

namespace SuperCU.Generic
{
    // Type.GetType()でnullが返ってくる場合のGetType
    public static class SubType
    {
        public static Type GetType(string className)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == className)
                    {
                        return type;
                    }
                }
            }
            return null;
        }
    }
}
