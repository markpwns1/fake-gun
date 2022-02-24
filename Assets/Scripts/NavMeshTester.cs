using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit)) {
                var agents = GameObject.FindObjectsOfType<NavMeshAgent>();
                foreach (var agent in agents)
                {
                    agent.updateRotation = false;
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
