using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderLeg : MonoBehaviour
{
    private NavMeshAgent agent;
    public float footstepDistance = 0.2f;
    public float movementSpeed = 2.0f;
    public float footLead = 0.2f;
    public float strideHeight;
    public float closeEnough;
    public Transform footRayPos;
    public Transform ik;
    public Vector3 ikSeekPos = Vector3.zero;
    public Vector3 lastSeekPos = Vector3.one;
    public Vector3 intermediatePos;
    public bool intermediate = false;

    // Update is called once per frame
    void Start() {
        footstepDistance *= Random.Range(0.9f, 1.1f);
        movementSpeed *= Random.Range(0.9f, 1.1f);
        ik.parent = null;
        agent = transform.root.GetComponent<NavMeshAgent>();
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
    void Update()
    {
        if(intermediate) {
            if(Vector3.Distance(intermediatePos, ik.position) > closeEnough) {
                ik.position = Vector3.Lerp(ik.position, intermediatePos, movementSpeed * Time.deltaTime);
            }
            else {
                intermediate = false;
            }
        }
        else {
            if(Vector3.Distance(ikSeekPos, ik.position) < closeEnough) {
                if(Physics.Raycast(footRayPos.position, -footRayPos.up, out RaycastHit hit)) {
                    if(Vector3.Distance(hit.point, ikSeekPos) > footstepDistance) {
                        intermediatePos = Vector3.Lerp(ik.position, hit.point, 0.5f) + footRayPos.up * strideHeight;
                        lastSeekPos = ikSeekPos;
                        ikSeekPos = hit.point + agent.velocity * footLead;
                        intermediate = true;
                    }
                }

            }
            
            ik.position = Vector3.Lerp(ik.position, ikSeekPos, movementSpeed * Time.deltaTime);
            // + Vector3.up * InverseLerp(lastSeekPos, ikSeekPos, ik.position) * strideHeight;
        }

        
    }
}
