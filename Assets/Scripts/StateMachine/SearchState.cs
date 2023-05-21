using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : State
{
    [SerializeField] private ChaseState chaseState;
    private GameObject player;

    [SerializeField] private float radius;
    private List<GameObject> controlPoints = new List<GameObject>();
    private Vector2[] rayVectors = new Vector2[4];
    private Vector2 teleportPosition;

    private bool canTeleport = false;
    private bool isScreamEventStart = false;

    private Coroutine activeTeleportEvent;

    void Start()
    {
        rayVectors[0] = Vector2.left;
        rayVectors[1] = Vector2.up;
        rayVectors[2] = Vector2.right;
        rayVectors[3] = Vector2.down;

        player = GameObject.FindGameObjectWithTag("Player");

    }

    public override State RunCurrentState()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= 3f)
        {
            StopAllCoroutines();
            isScreamEventStart = false;
            canTeleport = false;
            transform.parent.parent.gameObject.GetComponent<EnemyAnimationController>().ChangeAnimation("walk");
            return chaseState;
        }
        else
        {
            if (isScreamEventStart == false)
            {
                StartCoroutine(ScreamEvent());
            }
            else if (canTeleport == true && activeTeleportEvent == null)
            {
                activeTeleportEvent = StartCoroutine(ObstacleCheckEvent());
            }
        }
        return this;
    }

    private void SearchObstacles()
    {
        controlPoints.Clear();

        var colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Obstacle"))
            {
                controlPoints.Add(collider.gameObject);
                //sound wave effect
            }
        }
    }

    private void CheckObstacles()
    {
        RayControl(controlPoints[controlPoints.Count - 1]);
        controlPoints.RemoveAt(controlPoints.Count - 1);
    }

    private void RayControl(GameObject obstacle)
    {
        foreach (var rayDir in rayVectors)
        {
            RaycastHit2D hit = Physics2D.Raycast(obstacle.transform.position, rayDir, 1.5f, LayerMask.GetMask("Walls"));
            if (hit.collider == null)
            {
                teleportPosition = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y);
                teleportPosition += rayDir;
                break;
            }

            teleportPosition = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y);
            teleportPosition += Vector2.left;
        }
    }

    IEnumerator ScreamEvent()
    {
        transform.parent.parent.gameObject.GetComponent<EnemyAnimationController>().ChangeAnimation("scream");

        isScreamEventStart = true;
        yield return new WaitForSeconds(3f);
        SearchObstacles();
        yield return new WaitForSeconds(2f);
        canTeleport = true;
    }

    IEnumerator ObstacleCheckEvent()
    {
        transform.parent.parent.gameObject.GetComponent<EnemyAnimationController>().ChangeAnimation("teleport");
        yield return new WaitForSeconds(0.66f);

        CheckObstacles();
        transform.parent.parent.position = teleportPosition;
        yield return new WaitForSeconds(3f);
        activeTeleportEvent = null;
    }
}
