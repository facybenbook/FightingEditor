using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CharacterView : SceneView
{
    public static void Open()
    {
        var  window = ScriptableObject.CreateInstance<CharacterView>();
        window.Show();
        window.pivot = new Vector3(0, 5, -10);
        window.rotation = Quaternion.identity;
        //ほぼカメラ位置
        window.LookAt( window.pivot, window.rotation, 1);
        
    }
}
