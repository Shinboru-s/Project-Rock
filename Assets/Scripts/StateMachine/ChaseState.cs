using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private IdleState idleState;
    [SerializeField] private PatrolState patrolState;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }


    public override State RunCurrentState()
    {
        
        if (Vector2.Distance(player.position, transform.position) <= 5f) 
        {
            StopAllCoroutines();
            transform.parent.parent.gameObject.GetComponent<EnemyAIController>().SetAITarget(player);
            return this;
        }
        else
        {
            transform.parent.parent.gameObject.GetComponent<EnemyAIController>().SetAITarget(null);

            if (patrolState != null) 
            {
                transform.parent.parent.gameObject.GetComponent<EnemyAnimationController>().ChangeAnimation("walk");
                return patrolState;
            }
            else
            {
                transform.parent.parent.gameObject.GetComponent<EnemyAnimationController>().ChangeAnimation("idle");
                return idleState;
            }
        }

    }

}
