using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private bool isFindPlayer = false;
    [SerializeField] private ChaseState chaseState;
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("still works");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            isFindPlayer = true;
        }
    }

    public override State RunCurrentState()
    {
        if (isFindPlayer)
        {
            return chaseState;
        }
        else
        {
            return this;

        }
    }
}
