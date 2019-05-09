using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void alter()
    {

        GameManager.Instance.a = 5;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
