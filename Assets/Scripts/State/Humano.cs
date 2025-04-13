using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Humano : State
{
    public DataAgent _DataAgent;
    public AgentMovement3D _Movement; // Cambiado a 3D

    public override void LocadComponent()
    {
        base.LocadComponent();
        _DataAgent = GetComponent<DataAgent>();
        _Movement = GetComponent<AgentMovement3D>();
    }
}
