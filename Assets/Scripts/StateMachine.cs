using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State[] states;
    public State currentState = null;
    public TypeState Startstate = TypeState.Jugar; // Estado inicial siempre es Jugar

    void Start()
    {
        states = GetComponents<State>();
        ChangeState(Startstate);
    }

    public void ChangeState(TypeState type)
    {
        foreach (var state in states)
        {
            if (state.typestate == type)
            {
                if (currentState != null)
                    currentState.Exit();

                state.Enter();
                currentState = state;
                state.enabled = true;
            }
            else
            {
                state.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
}
