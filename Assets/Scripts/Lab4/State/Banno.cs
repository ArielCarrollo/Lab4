using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Banno : Humano
{
    private IAEye vision;
    private float bathroomTime = 3f;
    private float timer = 0f;

    private void Awake()
    {
        typestate = TypeState.Banno;
        LocadComponent();
        vision = this.GetComponent<IAEye>();
    }

    public override void Enter()
    {
        timer = 0f;
        _DataAgent.IsInBathroom = true;
        _Movement.MoveToLocation(WaypointManager.Instance.GetWaypointPosition("Banno"));
    }

    public override void Execute()
    {
        vision.ScanForToys();
        if (vision.currentToyInSight != null)
        {
            _StateMachine.ChangeState(TypeState.FollowToy);
            return;
        }
        if (_DataAgent.IsMoving || !_Movement.IsDone())
        {
            base.Execute();
            return;
        }

        _DataAgent.WC.value = Mathf.Min(1f, _DataAgent.WC.value + 0.1f * Time.deltaTime);

        if (_DataAgent.WC.value >= 0.95f)
        {
            _DataAgent.IsInBathroom = false;
            _DataAgent.WC.value = 1f;

            if (_DataAgent.Energy.value < 0.3f)
            {
                _StateMachine.ChangeState(TypeState.Comer);
            }
            else
            {
                _StateMachine.ChangeState(TypeState.Jugar);
            }
        }

        base.Execute();
    }

    public override void Exit()
    {
        _DataAgent.IsInBathroom = false;
    }
}