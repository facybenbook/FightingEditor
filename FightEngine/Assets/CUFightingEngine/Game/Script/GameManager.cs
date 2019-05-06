using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField]
    private List<IEventable> updateList = new List<IEventable>();

    public void AddUpdate(IEventable ev)
    {
        updateList.Add(ev);
    }
    private void Start()
    {
    }
    private void Update()
    {
        foreach(IEventable temp in updateList)
        {
            temp.UpdateGame();
        }
    }

}
