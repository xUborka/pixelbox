using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float movement_speed;
    [SerializeField] private float jump_force;

    [Header("Movement Checks")]
    [SerializeField] private Transform ground_check;
    [SerializeField] private Transform ceiling_check;
    [SerializeField] private LayerMask what_is_ground;
    private float ground_check_radius = 0.1f;

    private Rigidbody2D rigid_body;
    private SpriteRenderer sprite_renderer;
    private float horizontal_movement = 0.0f;
    private bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    bool CanJump(){
        bool can_jump = false;
        var colliders = Physics2D.OverlapCircleAll(ground_check.position, ground_check_radius, what_is_ground);
        foreach (var col in colliders){
            if (col.gameObject != gameObject){
                can_jump = true;
            }
        }
        return can_jump;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal_movement = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")){
            if (CanJump()){
            jumping = true;
            }
        }
    }

    void FixedUpdate(){
        rigid_body.velocity = new Vector2(horizontal_movement * movement_speed, rigid_body.velocity.y);
        if (rigid_body.velocity.x > 0.1f){
            sprite_renderer.flipX = false;
        } else if (rigid_body.velocity.x < -0.1f){
            sprite_renderer.flipX = true;
        }
        if (jumping){
            rigid_body.velocity = new Vector2(rigid_body.velocity.x, 0.0f); // Remove current y velocity
            rigid_body.velocity += Vector2.up * jump_force;
            jumping = false;
        }
    }
}
