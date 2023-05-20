using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField] private ChaseState chaseState;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    public override State RunCurrentState()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= 5f)
        {
            transform.parent.parent.gameObject.GetComponent<EnemyAnimationController>().ChangeAnimation("chase");
            return chaseState;
        }
        else
        {
            return this;

        }
    }
}
