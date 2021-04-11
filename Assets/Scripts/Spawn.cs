using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<Transform> spawn_points;
    private List<Vector2> explode_points;
    private List<bool> explode_bools;
    [SerializeField] private float spawn_time;
    [SerializeField] private GameObject box_prefab;
    [SerializeField] private LayerMask box_layer;

    private List<GameObject> boxes;
    
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        boxes = new List<GameObject>();
        explode_points = new List<Vector2>();
        explode_bools = new List<bool>();
        foreach(var pt in spawn_points){
            Vector2 tmp_pt = new Vector2(pt.position.x, 0);
            explode_points.Add(tmp_pt);
            explode_bools.Add(false);
        }
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawn_time){
            SpawnBox();
            timer = 0.0f;
        }

        DestroyCheck();
        LinedUpCheck();
    }

    void LinedUpCheck(){
        // Left Side
        // TODO: Do not freeze higher levels, if below is not frozen
        for (int multiplier = 0; multiplier < 4; multiplier++){
            for (int i = 0; i < explode_points.Count; i++){
                bool found = false;
                foreach(var t_box in boxes){
                    if (Vector2.Distance(t_box.transform.position, explode_points[i] + Vector2.up * multiplier) < 0.1f){
                        t_box.GetComponent<Rigidbody2D>().mass = 100f; 
                        found = true;
                    }
                }
                if (!found){
                    break;
                }
            }
        }
        // Right Side
        for (int multiplier = 0; multiplier < 4; multiplier++){
            for (int i = 0; i < explode_points.Count; i++){
                bool found = false;
                foreach(var t_box in boxes){
                    if (Vector2.Distance(t_box.transform.position, explode_points[explode_points.Count - i - 1] + Vector2.up * multiplier) < 0.1f){
                        t_box.GetComponent<Rigidbody2D>().mass = 100f; 
                        found = true;
                    }
                }
                if (!found){
                    break;
                }
            }
        }
    }

    void DestroyCheck(){
        List<int> to_destroy = new List<int>();
        for(int i = 0; i < boxes.Count; i++){
            for(int j = 0; j < explode_points.Count; j++){
                if (Vector2.Distance(boxes[i].transform.position, explode_points[j]) < 0.1f){
                    explode_bools[j] = true;
                    to_destroy.Add(i);
                }
            }
        }
        bool all_true = true;
        foreach(var b in explode_bools){
            if (!b){
                all_true = false;
                break;
            }
        }
        to_destroy.Reverse();
        if (all_true){
            foreach(var dest in to_destroy){
                var obj = boxes[dest];
                boxes.RemoveAt(dest);
                obj.GetComponent<BoxScript>().DestroyBox();
            }
        }

        for(int i = 0; i < explode_bools.Count; i++){
            explode_bools[i] = false;
        }
    }

    void SpawnBox(){
        List<Vector2> valid_spawn_points = new List<Vector2>();
        foreach(var spawn_pt in spawn_points){
            Vector2 pt = new Vector2(spawn_pt.position.x, spawn_pt.position.y - 1.0f);
            bool valid = true;
            var colliders = Physics2D.OverlapCircleAll(pt, 0.1f, box_layer);
            foreach (var col in colliders){
                valid = false;
            }
            if (valid){
                valid_spawn_points.Add(spawn_pt.position);
            }
        }
        if (valid_spawn_points.Count >= 1){
            int spawn_idx = Mathf.RoundToInt(Random.Range(0.0f, valid_spawn_points.Count - 1));
            GameObject box = Instantiate(box_prefab, valid_spawn_points[spawn_idx], Quaternion.identity);
            boxes.Add(box);
        } else {
            // Cannot spawn, but player is not dead --> game over?

        }
    }
}
