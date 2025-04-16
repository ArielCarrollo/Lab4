using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Jugar : Humano
{
    private IAEye vision;
    private void Awake()
    {
        typestate = TypeState.Jugar;
        LocadComponent();
        vision = this.GetComponent<IAEye>();
    }

    public override void Enter()
    {
        _Movement.MoveToLocation(WaypointManager.Instance.GetWaypointPosition("SalaDeJuegos"));
    }

    public override void Execute()
    {
        if (_DataAgent.IsMoving)
        {
            base.Execute();
            return;
        }
        // Primero verificar si hay juguetes visibles
        vision.ScanForToys();
        if (vision.currentToyInSight != null)
        {
            _StateMachine.ChangeState(TypeState.FollowToy);
            return;
        }


        // Consumir energía
        _DataAgent.DiscountEnergy();

        // Consumir sueño más rápido al jugar
        if (!_DataAgent.IsInBathroom)
        {
            _DataAgent.Sleep.value = Mathf.Max(0, _DataAgent.Sleep.value - Time.deltaTime * 0.02f);
        }

        // Consumir necesidad de baño más rápido al jugar
        _DataAgent.WC.value = Mathf.Max(0, _DataAgent.WC.value - Time.deltaTime * 0.03f);

        // Transiciones de estado simplificadas
        if (_DataAgent.WC.value < 0.15f)
        {
            _StateMachine.ChangeState(TypeState.Banno);
        }
        else if (_DataAgent.Energy.value < 0.25f)
        {
            _StateMachine.ChangeState(TypeState.Comer);
        }
        else if (_DataAgent.Sleep.value < 0.2f)
        {
            _StateMachine.ChangeState(TypeState.Dormir);
        }

        base.Execute();
    }
    public override void Exit()
    {

    }
}
