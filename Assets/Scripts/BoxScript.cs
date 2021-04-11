using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem explode_particle;
    private Rigidbody2D rigid_body;

    
    // Start is called before the first frame update
    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
    }

    public void DestroyBox(){
        Instantiate<ParticleSystem>(explode_particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (rigid_body.velocity.magnitude <= 0.1f){
            Vector2 snap_position = new Vector2(Mathf.Round(rigid_body.position.x), Mathf.Round(rigid_body.position.y));
            if (Vector2.Distance(snap_position, rigid_body.position) < 0.2f){
                rigid_body.position = snap_position;
            }
        }
    }
}
