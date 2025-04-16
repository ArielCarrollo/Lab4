using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Comer : Humano
{
    private IAEye vision;
    private float eatingTime = 5f;
    private float timer = 0f;

    private void Awake()
    {
        typestate = TypeState.Comer;
        LocadComponent();
        vision = this.GetComponent<IAEye>();
    }

    public override void Enter()
    {
        timer = 0f;
        _Movement.MoveToLocation(WaypointManager.Instance.GetWaypointPosition("Comedor"));
    }

    public override void Execute()
    {
        vision.ScanForToys();
        if (vision.currentToyInSight != null)
        {
            _StateMachine.ChangeState(TypeState.FollowToy);
            return;
        }
        if (_DataAgent.IsMoving || !_Movement.IsDone()) return;

        timer += Time.deltaTime;
        _DataAgent.Energy.value = Mathf.Min(1f, _DataAgent.Energy.value + 0.5f * Time.deltaTime);

        if (timer >= eatingTime || _DataAgent.Energy.value >= 0.95f)
        {
            if (_DataAgent.WC.value < 0.2f)
            {
                _StateMachine.ChangeState(TypeState.Banno);
            }
            else if (_DataAgent.Sleep.value < 0.3f)
            {
                _StateMachine.ChangeState(TypeState.Dormir);
            }
            else
            {
                _StateMachine.ChangeState(TypeState.Jugar);
            }
        }
    }

    public override void Exit()
    {
        // Lógica de limpieza si es necesaria
    }
}

