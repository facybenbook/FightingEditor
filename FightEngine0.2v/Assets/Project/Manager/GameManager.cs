using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperCU.FightingEngine;
using SuperCU.Generic;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField]
    private List<IEventable> updateList = new List<IEventable>();
    protected override void Awake()
    {
        base.Awake();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void AddUpdate(IEventable ev)
    {
        updateList.Add(ev);
    }
    private void Start()
    {
    }
    private void Update()
    {
        foreach (IEventable temp in updateList)
        {
            temp.UpdateGame();
        }
    }
}
