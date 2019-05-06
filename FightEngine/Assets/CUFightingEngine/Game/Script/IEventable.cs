using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventable
{
    void UpdateGame();
    void LateUpdateGame();
    void FixedUpdateGame();
}
