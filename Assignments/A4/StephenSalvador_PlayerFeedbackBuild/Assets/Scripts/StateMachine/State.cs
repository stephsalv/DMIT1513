using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class State : MonoBehaviour
{
    public virtual void Enter() { }
    public virtual void Exit() { }

    public abstract State RunCurrentState();
}
