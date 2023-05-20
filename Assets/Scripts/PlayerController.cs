using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    Rigidbody2D rb;
    Vector2 movement;
     private bool isFaceRight = true;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        
        if ((movement.x < 0 && isFaceRight) || (movement.x > 0 && isFaceRight == false))
                Flip();
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
    }
}
