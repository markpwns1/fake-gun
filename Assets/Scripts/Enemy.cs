
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public const int ORBIT_POS_COUNT = 10;
    public static List<Enemy> alertedEnemies = new List<Enemy>();
    public static List<Enemy> allEnemies = new List<Enemy>();
    public static bool initialisedOrbitPositions = false;
    public float closeEnough = 0.1f;
    public float walkSpeed = 3.5f;

    [Range(0.0f, 1.0f)]
    public float followSpeed = 0.8f;
    public float orbitRadiusMin = 3.0f;
    public float orbitRadiusMax = 4.0f;
    public float faceDistance;
    public float pathfindDistance;
    public float pathfindInterval = 1.0f;
    public float spreadIntervalMin = 1.0f;
    public float spreadIntervalMax = 2.0f;
    public float shookDistance = 5.0f;
    public float attackIntervalMin = 1.0f;
    public float attackIntervalMax = 2.0f;
    public float attackDistance = 1.0f;
    public float alertDistance = 15.0f;
    public float scareAwayDistance = 7.5f;
    public int damageMin = 8;
    public int damageMax = 16;
    public ParticleSystem exclamationMarks;
    public ParticleSystem questionMarks;
    public Transform[] fleeLocations;
    public Transform[] waypoints;
    public GameObject angryParticles;
    public GameObject fearParticles;
    private bool fleeing = false;
    private GameObject player;
    private NavMeshAgent agent;
    private Vector3 destination;
    private bool alerted = false;
    private bool pathCooldown = false;
    private int currentOrbitSlot = -1;
    public float fear = 0.0f;
    private bool attacking = false;
    private bool canAttack = true;
    private float lastFear = 0.0f;
    private Vector3 fleePos;
    private Health playerHealth;
    // Start is called before the first frame update

    IEnumerator AttackDelay() {
        canAttack = false;
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = GameObject.FindObjectOfType<Health>();
        allEnemies.Add(this);
        agent.updateRotation = true;
        agent.destination = waypoints[Random.Range(0, waypoints.Length)].position;
        // AlertToPlayer(); // todo: alert when spotted
        // StartCoroutine(AttackLoop());
    }

    void AlertToPlayer() {
        alerted = true;
        alertedEnemies.Add(this);
        exclamationMarks.Emit(1);
    }

    Vector3 GetFinalDestination() {
        return player.transform.position;
    }


    IEnumerator RequestUpdatePath() {
        if(!pathCooldown) {
            pathCooldown = true;
            agent.SetDestination(GetFinalDestination());
            yield return new WaitForSeconds(pathfindInterval);
            pathCooldown = false;
        }
    }

    Vector3 IgnoreY(Vector3 v) {
        v.y = 0;
        return v;
    }

    int mod(int x, int m) {
        return (x % m + m) % m;
    }

    // IEnumerator AttackLoop() {
    //     while(true) {
    //         yield return new WaitUntil(() => {
    //             if(fleeing || !alerted) return false;

    //             var dest = GetFinalDestination();
    //             var destIgnoreY = IgnoreY(dest);
    //             var posIgnoreY = IgnoreY(transform.position);
    //             var distFromDest = Vector3.Distance(destIgnoreY, posIgnoreY);

    //             return distFromDest < closeEnough;
    //         });

    //         yield return new WaitForSeconds(Random.Range(attackIntervalMin, attackIntervalMax));

    //         playerHealth.health -= Random.Range(damageMin, damageMax);
    //         Debug.Log(playerHealth.health);
    //         // attacking = true;
    //         // yield return new WaitForSeconds(0.5f);
    //         // attacking = false;
    //     }
    // }

    void Update() {
        agent.speed = Mathf.MoveTowards(agent.speed, walkSpeed, 1.0f * Time.deltaTime);
        
    }

    void LateUpdate() {
        var deltaFear = fear - lastFear;

        if(deltaFear > 0.0f) {
            exclamationMarks.Emit(Mathf.CeilToInt(deltaFear / 0.15f));
        }
        else if(deltaFear < 0.0f) {
            questionMarks.Emit(Mathf.CeilToInt(Mathf.Abs(deltaFear / 0.1f)));
        }

        lastFear = fear;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dest = GetFinalDestination();
        var destIgnoreY = IgnoreY(dest);
        var posIgnoreY = IgnoreY(transform.position);
        var playerIgnoreY = IgnoreY(player.transform.position);
        var distFromDest = Vector3.Distance(destIgnoreY, posIgnoreY);

        if(!fleeing && alerted) {
            
            if(distFromDest > pathfindDistance) {
                agent.isStopped = false;
                StartCoroutine(RequestUpdatePath());
            }
            else if(distFromDest > closeEnough) {
                agent.isStopped = true;
                agent.Move(
                    (destIgnoreY - posIgnoreY).normalized 
                    * Mathf.Min(distFromDest, agent.speed * followSpeed * Time.deltaTime));
            }

            if(Vector3.Distance(posIgnoreY, playerIgnoreY) < faceDistance) {
                agent.updateRotation = false;
                agent.transform.forward = playerIgnoreY - posIgnoreY;
            }
            else {
                agent.updateRotation = true;
            }

            if(canAttack && Vector3.Distance(posIgnoreY, playerIgnoreY) < attackDistance) {
                StartCoroutine(AttackDelay());
                playerHealth.health -= Random.Range(damageMin, damageMax);
                playerHealth.Hurt();
                // Debug.Log(playerHealth.health);
                agent.speed = 0;
            }

        }
        else if(fleeing) {
            if(Vector3.Distance(fleePos, transform.position) < 2.0f) {
                allEnemies.Remove(this);
                Destroy(gameObject);
            }
        }
        else if(!alerted) {
            var diff = player.transform.position - transform.position;
            if(Vector3.Angle(diff, transform.forward) < 45.0f && Physics.Raycast(transform.position, diff.normalized, out RaycastHit hit, alertDistance)) {
                if(hit.transform.gameObject.tag == "Player") {
                    AlertToPlayer();
                }
            }

            if(Vector3.Distance(transform.position, agent.destination) < 2.0f) {
                agent.destination = waypoints[Random.Range(0, waypoints.Length)].position;
            }
        }
    }

    public void Spook(float amount) {
        if(!alerted) {
            var diff = player.transform.position - transform.position;
            if(diff.magnitude < scareAwayDistance) {
                Flee();
                return;
            }
            else if(Physics.Raycast(transform.position, diff.normalized, out RaycastHit hit, alertDistance) && hit.collider.gameObject.tag == "Player") {
                AlertToPlayer();
            }
        }

        if(fear < -1.0f) return;

        fear += amount;
        if(fear < 1.0f) {
            agent.speed = 0.0f;
        }
        else {
            Flee();
        }
    }

    void Flee() {
        fleeing = true;
        agent.speed = walkSpeed;
        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;
        fleePos = fleeLocations[Random.Range(0, fleeLocations.Length)].position;
        agent.SetDestination(fleePos);
        alertedEnemies.Remove(this);
        fearParticles.SetActive(true);
        
        Debug.Log("FLEE!!");
    }

    public void Sus(float amount) {
        if(fear < -1.0f) return;
        fear -= amount;

        if(fear < -1.0f) {
            angryParticles.SetActive(true);
        }
    }
}
