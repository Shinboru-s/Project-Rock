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
    private int controlIndex = 0;

    [SerializeField] private Transform tempTransform;


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
            FinishSeach();
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
        controlIndex = 0;

        //var colliders = Physics.OverlapSphere(transform.position, radius);
        //foreach (var collider in colliders)
        //{
        //    Debug.Log("Bulunan obje adi: " + collider.gameObject.name);

        //    if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Obstacle"))
        //    {
        //        Debug.Log("Kontrol listesine eklenen obje adi: " + collider.gameObject.name);

        //        controlPoints.Add(collider.gameObject);
        //        //sound wave effect
        //    }
        //}
        var objects = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var item in objects)
        {
            if(Vector2.Distance(item.transform.position, transform.position) <= radius)
            {
                Debug.Log("Kontrol listesine eklenen obje adi: " + item.gameObject.name);
                controlPoints.Add(item.gameObject);
                //sound wave effect
            }
        }
        //player
        if (Vector2.Distance(player.transform.position, transform.position) <= radius)
        {
            controlPoints.Add(player);
        }
    }

    private void CheckObstacles()
    {
        if (controlIndex >= controlPoints.Count)
        {
            FinishSeach();
        }
        else
        {
            RayControl(controlPoints[controlIndex]);
            controlIndex++;
        } 
        
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

        isScreamEventStart = true;
        yield return new WaitForSeconds(3f);
        transform.parent.parent.gameObject.GetComponent<EnemyAnimationController>().ChangeAnimation("scream");
        FindObjectOfType<AudioManager>().Play("BansheeScream");

        SearchObstacles();
        yield return new WaitForSeconds(2f);
        if (controlPoints != null)
        {
            canTeleport = true;
        }
        else
        {
            FinishSeach();
        }

    }

    IEnumerator ObstacleCheckEvent()
    {
        CheckObstacles();
        if (canTeleport == false)
            yield break;

        transform.parent.parent.gameObject.GetComponent<EnemyAnimationController>().ChangeAnimation("teleport");
        FindObjectOfType<AudioManager>().Play("BansheeTeleport");
        yield return new WaitForSeconds(0.66f);

        tempTransform.position = new Vector3(teleportPosition.x, teleportPosition.y, 0f);

        transform.parent.parent.gameObject.GetComponent<EnemyAIController>().SetAITarget(tempTransform);
        transform.parent.parent.position = teleportPosition;
        yield return new WaitForSeconds(3f);
        activeTeleportEvent = null;
    }

    private void FinishSeach()
    {
        canTeleport = false;
        isScreamEventStart = false;
        activeTeleportEvent = null;
    }
}
