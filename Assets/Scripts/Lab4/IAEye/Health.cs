using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Entity { Child, Toy }
public class Health : MonoBehaviour
{
    [SerializeField]private FollowToy Agent;
    public Entity Entity;
    public Transform AimOffSet;
    // Start is called before the first frame update
    void Start()
    {

    }
}
