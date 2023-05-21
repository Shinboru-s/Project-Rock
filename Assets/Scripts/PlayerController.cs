using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public GameObject soundWave;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float throwForce = 10f;
    Rigidbody2D rb;
    Vector2 movement;
     private bool isFaceRight = true;
     private bool isSoundWaving = false;
     private Coroutine test;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();

        if ((movement.x < 0 && isFaceRight) || (movement.x > 0 && isFaceRight == false)){
            Flip();
        }
                
        animator.SetFloat("Speed", Mathf.Abs(movement.x) + Mathf.Abs(movement.y));
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    
    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        isFaceRight = !isFaceRight;
    }

    void Move() 
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
        if (movement != Vector2.zero && isSoundWaving == false)
        {
           test = StartCoroutine(waitForNextSpawn());       
        }

        else if (movement == Vector2.zero && isSoundWaving == true)
        {
            StopCoroutine(test);
            isSoundWaving = false;
        }
    }

    IEnumerator spawnSoundWave()
    {
        GameObject newObject = Instantiate(soundWave, transform.parent);
        newObject.transform.position = transform.position;
        yield return new WaitForSeconds(0.8f);
        Destroy(newObject);
    }

    IEnumerator waitForNextSpawn()
    {
        isSoundWaving = true;
        StartCoroutine(spawnSoundWave());
        yield return new WaitForSeconds(1f);
        isSoundWaving = false;
        yield return null;
    }
}