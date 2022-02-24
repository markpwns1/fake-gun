using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTemporary : MonoBehaviour
{
    public float glanceFear = 0.1f;
    public float hitSus = 0.3f;
    public float scareDistance = 15.0f;
    public float fearCone = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            foreach (var enemy in Enemy.allEnemies)
            {
                if(Physics.Raycast(Camera.main.transform.forward, Camera.main.transform.forward, out RaycastHit hit, scareDistance)) {
                    if(hit.transform.gameObject.tag == "Enemy") {
                        enemy.Sus(hitSus);
                    }
                }
                else {
                    var to = enemy.transform.position - transform.position;
                    if(to.magnitude < scareDistance && Vector3.Angle(Camera.main.transform.forward, to) < fearCone) {
                        enemy.Spook(glanceFear);
                    }
                }
            }
        }
    }
}
