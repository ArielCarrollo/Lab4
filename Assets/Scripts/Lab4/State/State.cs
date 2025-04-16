using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum TypeState
{
    Jugar,
    Comer,
    Dormir,
    Banno,
    FollowToy
}
public class State : MonoBehaviour
{
    public TypeState typestate;
    public StateMachine _StateMachine;
    public TextMeshPro  TextState;
    public virtual void LocadComponent()
    {
        _StateMachine = GetComponent<StateMachine>();
    }
    public virtual void Enter()
    {
        
    }
    public virtual void Execute()
    {
        if(TextState!=null)
        TextState.text = typestate.ToString();
    }
    public virtual void Exit()
    {
     
    }
}
