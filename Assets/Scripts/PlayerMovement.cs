using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    [SerializeField] private LayerMask what_is_danger;
    private float ground_check_radius = 0.1f;
    private float ceiling_check_radius = 0.1f;

    private Rigidbody2D rigid_body;
    private SpriteRenderer sprite_renderer;
    private float horizontal_movement = 0.0f;
    private bool jumping = false;
    private bool alive;
    private bool on_ground = true;

    [SerializeField] private GameObject game_over_screen;


    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        rigid_body = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void KillPlayer(){
        game_over_screen.SetActive(true);
        rigid_body.velocity = new Vector2(0.0f, 0.0f);
        alive = false;
        Invoke("Restart", 2.0f);
    }

    void check_on_ground(){
        bool tmp_on_ground = false;
        var colliders = Physics2D.OverlapCircleAll(ground_check.position, ground_check_radius, what_is_ground);
        foreach (var col in colliders){
            if (col.gameObject != gameObject){
                tmp_on_ground = true;
            }
        }
        on_ground = tmp_on_ground;
    }

    // Update is called once per frame
    void Update()
    {
        check_on_ground();
        horizontal_movement = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")){
            if (on_ground){
            jumping = true;
            }
        }
        DeathCheck();
    }

    void DeathCheck(){
        var colliders = Physics2D.OverlapCircleAll(ceiling_check.position, ceiling_check_radius, what_is_danger);
        foreach (var collider in colliders){
            if (on_ground && Mathf.Abs(collider.GetComponent<Rigidbody2D>().velocity.y) > 1.0f){
                KillPlayer();
            }
        }
    }

    void FixedUpdate(){
        if (alive){
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
}
