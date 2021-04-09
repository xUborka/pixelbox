using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    private Rigidbody2D rigid_body;
    private float fall_multiplier = 1.5f;
    private float jump_multiplier = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void FixedUpdate(){
        if (rigid_body.velocity.y < 0.0f){
            rigid_body.velocity += Vector2.up * Physics2D.gravity.y * fall_multiplier * Time.fixedDeltaTime;
        } else if (rigid_body.velocity.y > 0.0f && !Input.GetButton("Jump")){
            rigid_body.velocity += Vector2.up * Physics2D.gravity.y * jump_multiplier * Time.fixedDeltaTime;
        }
    }
}
