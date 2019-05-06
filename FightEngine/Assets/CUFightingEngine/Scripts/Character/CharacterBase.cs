using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IEventable
{
    public virtual void FixedUpdateGame()
    {
    }
    public virtual void LateUpdateGame()
    {
    }
    public virtual void UpdateGame()
    {
    }
}
