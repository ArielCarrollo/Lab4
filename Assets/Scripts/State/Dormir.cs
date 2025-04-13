using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dormir : Humano
{
    private float sleepRecoveryRate = 0.1f;
    private float energyRecoveryRate = 0.05f;
    private bool movingToBed = true;

    private void Awake()
    {
        typestate = TypeState.Dormir;
        LocadComponent();
    }

    public override void Enter()
    {
        _DataAgent.IsSleeping = false;
        movingToBed = true;
        _Movement.MoveToLocation(WaypointManager.Instance.GetWaypointPosition("Dormitorio"));
    }

    public override void Execute()
    {
        if (movingToBed)
        {
            if (_Movement.IsDone())
            {
                movingToBed = false;
                _DataAgent.IsSleeping = true;
            }
            return;
        }

        // Recuperación de energía (siempre activa)
        _DataAgent.Energy.value = Mathf.Min(1f, _DataAgent.Energy.value + energyRecoveryRate * Time.deltaTime);

        // Recuperación de sueño (solo cuando no se mueve)
        if (!_DataAgent.IsMoving)
        {
            _DataAgent.Sleep.value = Mathf.Min(1f, _DataAgent.Sleep.value + sleepRecoveryRate * Time.deltaTime);
        }

        // Transiciones de estado
        if (_DataAgent.Sleep.value >= 0.95f)
        {
            if (_DataAgent.WC.value < 0.2f)
            {
                _StateMachine.ChangeState(TypeState.Banno);
            }
            else if (_DataAgent.Energy.value < 0.5f)
            {
                _StateMachine.ChangeState(TypeState.Comer);
            }
            else
            {
                _StateMachine.ChangeState(TypeState.Jugar);
            }
        }
    }

    public override void Exit()
    {
        _DataAgent.IsSleeping = false;
    }
}
