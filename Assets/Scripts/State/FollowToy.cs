using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))] // Asegurar que tiene collider
public class FollowToy : Humano
{
    private IAEye vision;
    private Health targetToy;
    private bool isPickingUp = false;
    private TypeState previousState;
    private float pickupTime = 1.5f;
    private Coroutine pickupCoroutine;

    private void Awake()
    {
        typestate = TypeState.FollowToy;
        LocadComponent();
        vision = gameObject.GetComponent<IAEye>();

        // Configurar collider para detección
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true; // Usar trigger para mejor detección
    }

    public override void Enter()
    {
        isPickingUp = false;
        targetToy = null;
        previousState = _StateMachine.currentState.typestate;
    }

    public override void Execute()
    {
        if (isPickingUp) return; // No hacer nada mientras recoge

        if (targetToy == null)
        {
            FindNewToy();
            return;
        }

        _Movement.MoveToLocation(targetToy.transform.position);
    }

    private void FindNewToy()
    {
        vision.ScanForToys();
        targetToy = vision.currentToyInSight;

        if (targetToy == null)
        {
            _StateMachine.ChangeState(previousState);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPickingUp || targetToy == null) return;

        if (other.CompareTag("Toy") && other.gameObject == targetToy.gameObject)
        {
            pickupCoroutine = StartCoroutine(PickupToy());
        }
    }

    private IEnumerator PickupToy()
    {
        isPickingUp = true;
        _Movement.Stop(); // Detener el movimiento

        // Aquí puedes añadir animación o efectos
        Debug.Log("Comenzando recogida de juguete...");

        yield return new WaitForSeconds(pickupTime);

        if (targetToy != null)
        {
            Destroy(targetToy.gameObject);
            Debug.Log("Juguete recogido!");
        }

        // Regresar al estado anterior
        _StateMachine.ChangeState(previousState);
    }

    public override void Exit()
    {
        if (pickupCoroutine != null)
        {
            StopCoroutine(pickupCoroutine);
        }

        if (vision != null)
        {

        }

        // Restablecer valores
        isPickingUp = false;
        targetToy = null;
    }
}