using UnityEngine;

public class FollowToy : Humano
{
    private IAEye vision;
    private Health targetToy;
    private bool hasToy = false;

    private void Awake()
    {
        typestate = TypeState.FollowToy;
        LocadComponent();
        vision = gameObject.AddComponent<IAEye>();
    }

    public override void Enter()
    {
        hasToy = false;
        targetToy = null;
    }

    public override void Execute()
    {
        // Si ya tiene el juguete, regresar a la sala
        if (hasToy)
        {
            _Movement.MoveToLocation(WaypointManager.Instance.GetWaypointPosition("SalaDeJuegos"));
            if (_Movement.IsDone())
            {
                _StateMachine.ChangeState(TypeState.Jugar);
            }
            return;
        }

        // Buscar juguete si no tiene uno asignado
        if (targetToy == null)
        {
            vision.ScanForToys();
            targetToy = vision.currentToyInSight;

            if (targetToy == null)
            {
                _StateMachine.ChangeState(TypeState.Jugar);
                return;
            }
        }

        // Mover hacia el juguete
        _Movement.MoveToLocation(targetToy.transform.position);

        // Verificar si llegó al juguete
        if (_Movement.IsDone() && targetToy != null)
        {
            Destroy(targetToy.gameObject);
            hasToy = true;
        }
    }

    public override void Exit()
    {
        if (vision != null)
        {
            Destroy(vision);
        }
    }
}