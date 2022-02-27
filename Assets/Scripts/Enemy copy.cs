
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class Enemy : MonoBehaviour
// {
//     public const int ORBIT_POS_COUNT = 10;
//     public static Vector3[] orbitPositions = new Vector3[ORBIT_POS_COUNT];
//     public static Enemy[] orbitSlots = new Enemy[ORBIT_POS_COUNT];
//     public static List<Enemy> alertedEnemies = new List<Enemy>();
//     public static List<Enemy> allEnemies = new List<Enemy>();
//     public static bool initialisedOrbitPositions = false;
//     public float closeEnough = 0.1f;
//     public float walkSpeed = 3.5f;

//     [Range(0.0f, 1.0f)]
//     public float followSpeed = 0.8f;
//     public float orbitRadiusMin = 3.0f;
//     public float orbitRadiusMax = 4.0f;
//     public float faceDistance;
//     public float pathfindDistance;
//     public float pathfindInterval = 1.0f;
//     public float spreadIntervalMin = 1.0f;
//     public float spreadIntervalMax = 2.0f;
//     public float shookDistance = 5.0f;
//     public float attackIntervalMin = 1.0f;
//     public float attackIntervalMax = 2.0f;
//     public int damageMin = 8;
//     public int damageMax = 16;
//     public ParticleSystem exclamationMarks;
//     public ParticleSystem questionMarks;
//     public Transform[] fleeLocations;
//     private bool fleeing = false;
//     private GameObject player;
//     private NavMeshAgent agent;
//     private Vector3 destination;
//     private bool alerted = false;
//     private bool pathCooldown = false;
//     private int currentOrbitSlot = -1;
//     private float fear = 0.0f;
//     private bool attacking = false;
//     private float lastFear = 0.0f;
//     private Vector3 fleePos;
//     private Health playerHealth;
//     // Start is called before the first frame update

//     void Awake() {
//         if(initialisedOrbitPositions) return;

//         initialisedOrbitPositions = true;
//         for (int i = 0; i < ORBIT_POS_COUNT; i++)
//         {
//             var pos = Vector3.zero;
//             var theta = 2.0f * Mathf.PI * (i / (float) ORBIT_POS_COUNT);
//             pos.x = Mathf.Cos(theta);
//             pos.z = Mathf.Sin(theta);
//             pos *= Random.Range(orbitRadiusMin, orbitRadiusMax);
//             orbitPositions[i] = pos;
//         }
//     }

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         player = GameObject.FindGameObjectWithTag("Player");
//         playerHealth = GameObject.FindObjectOfType<Health>();
//         allEnemies.Add(this);
//         agent.updateRotation = false;
//         AlertToPlayer(); // todo: alert when spotted
//         StartCoroutine(Spread());
//         StartCoroutine(AttackLoop());
//     }

//     void AlertToPlayer() {
//         if(FindNewOrbitPos()) {
//             alerted = true;
//             alertedEnemies.Add(this);
//         }
//     }

//     int GetIdealOrbitSlot() {
//         int minI = -1;
//         float minDist = 10000.0f;
//         for (int i = 0; i < ORBIT_POS_COUNT; i++)
//         {
//             var dist = Vector3.Distance(GetOrbitPos(i), IgnoreY(transform.position));
//             if(orbitSlots[i] == null && dist < minDist) {
//                 minDist = dist;
//                 minI = i;
//             }
//         }
//         return minI;
//     }

//     bool FindNewOrbitPos() {
//         var i = GetIdealOrbitSlot();

//         if(i != -1) {
//             destination = orbitPositions[i];
//             orbitSlots[i] = this;
//             currentOrbitSlot = i;
//             return true;
//         }
//         return false;
//     }

//     void SetOrbitPos(int slot) {
//         if(currentOrbitSlot != -1) {
//             orbitSlots[currentOrbitSlot] = null;
//         }

//         currentOrbitSlot = slot;

//         if(slot != -1) {
//             orbitSlots[currentOrbitSlot] = this;
//             destination = orbitPositions[currentOrbitSlot];
//         }
//     }

//     Vector3 GetFinalDestination() {
//         return player.transform.position + destination;
//     }

//     Vector3 GetOrbitPos(int i) {
//         return IgnoreY(player.transform.position + orbitPositions[i]);
//     }

//     IEnumerator RequestUpdatePath() {
//         if(!pathCooldown) {
//             pathCooldown = true;
//             agent.SetDestination(GetFinalDestination());
//             yield return new WaitForSeconds(pathfindInterval);
//             pathCooldown = false;
//         }
//     }

//     Vector3 IgnoreY(Vector3 v) {
//         v.y = 0;
//         return v;
//     }

//     int mod(int x, int m) {
//         return (x % m + m) % m;
//     }

//     IEnumerator Spread() {
//         while(true) {

//             yield return new WaitUntil(() => {
//                 if(fleeing || !alerted) return false;
//                 var dest = GetFinalDestination();
//                 var destIgnoreY = IgnoreY(dest);
//                 var posIgnoreY = IgnoreY(transform.position);
//                 var distFromDest = Vector3.Distance(destIgnoreY, posIgnoreY);

//                 return distFromDest < pathfindDistance;
//             });

//             yield return new WaitForSeconds(Random.Range(spreadIntervalMin, spreadIntervalMax));

//             int distLeft = 0;
//             for (int i = mod(currentOrbitSlot - 1, ORBIT_POS_COUNT); i != currentOrbitSlot; i = mod(i - 1, ORBIT_POS_COUNT))
//             {
//                 if(orbitSlots[mod(i, ORBIT_POS_COUNT)] == null)
//                     distLeft++;
//                 else break;
//             }

//             int distRight = 0;
//             for (int i = (currentOrbitSlot + 1) % ORBIT_POS_COUNT; i != currentOrbitSlot; i = (i + 1) % ORBIT_POS_COUNT)
//             {
//                 if(orbitSlots[i % ORBIT_POS_COUNT] == null)
//                     distRight++;
//                 else break;
//             }

//             if(distLeft > 0 || distRight > 0) {
//                 if(distLeft - distRight > 1) {
//                     SetOrbitPos(mod(currentOrbitSlot - 1, ORBIT_POS_COUNT));
//                 }
//                 else if(distRight - distLeft > 1) {
//                     SetOrbitPos((currentOrbitSlot + 1) % ORBIT_POS_COUNT);
//                 }
//             }
//         }
//     }

//     IEnumerator AttackLoop() {
//         while(true) {
//             yield return new WaitUntil(() => {
//                 if(fleeing || !alerted) return false;

//                 var dest = GetFinalDestination();
//                 var destIgnoreY = IgnoreY(dest);
//                 var posIgnoreY = IgnoreY(transform.position);
//                 var distFromDest = Vector3.Distance(destIgnoreY, posIgnoreY);

//                 return distFromDest < closeEnough;
//             });

//             yield return new WaitForSeconds(Random.Range(attackIntervalMin, attackIntervalMax));

//             playerHealth.health -= Random.Range(damageMin, damageMax);
//             Debug.Log(playerHealth.health);
//             // attacking = true;
//             // yield return new WaitForSeconds(0.5f);
//             // attacking = false;
//         }
//     }

//     void Update() {
//         agent.speed = Mathf.MoveTowards(agent.speed, walkSpeed, 1.0f * Time.deltaTime);
        
//     }

//     void LateUpdate() {
//         var deltaFear = fear - lastFear;

//         if(deltaFear > 0.0f) {
//             exclamationMarks.Emit(Mathf.CeilToInt(deltaFear / 0.15f));
//         }
//         else if(deltaFear < 0.0f) {
//             questionMarks.Emit(Mathf.CeilToInt(Mathf.Abs(deltaFear / 0.1f)));
//         }

//         lastFear = fear;
//     }

//     // Update is called once per frame
//     void FixedUpdate()
//     {
//         if(!fleeing && alerted) {
//             var dest = GetFinalDestination();
//             var destIgnoreY = IgnoreY(dest);
//             var posIgnoreY = IgnoreY(transform.position);
//             var playerIgnoreY = IgnoreY(player.transform.position);
//             var distFromDest = Vector3.Distance(destIgnoreY, posIgnoreY);

            
//             if(distFromDest > pathfindDistance) {
//                 if(distFromDest > shookDistance) {
//                     orbitSlots[currentOrbitSlot] = null;
//                     currentOrbitSlot = -1;

//                     FindNewOrbitPos();
//                 }

//                 agent.isStopped = false;
//                 StartCoroutine(RequestUpdatePath());
//             }
//             else if(distFromDest > closeEnough) {
//                 agent.isStopped = true;
//                 agent.Move(
//                     (destIgnoreY - posIgnoreY).normalized 
//                     * Mathf.Min(distFromDest, agent.speed * followSpeed * Time.deltaTime));
//             }

//             if(Vector3.Distance(posIgnoreY, playerIgnoreY) < faceDistance) {
//                 agent.updateRotation = false;
//                 agent.transform.forward = playerIgnoreY - posIgnoreY;
//             }
//             else {
//                 agent.updateRotation = true;
//             }
//         }
//         else if(fleeing) {
//             if(Vector3.Distance(fleePos, transform.position) < 2.0f) {
//                 Destroy(gameObject);
//             }
//         }
//     }

//     public void Spook(float amount) {
//         if(fear < -1.0f) return;

//         fear += amount;
//         if(fear < 1.0f) {
//             agent.speed = 0.0f;
//         }
//         else {
//             Flee();
//         }
//     }

//     void Flee() {
//         fleeing = true;
//         agent.speed = walkSpeed;
//         agent.isStopped = false;
//         agent.updatePosition = true;
//         agent.updateRotation = true;
//         fleePos = fleeLocations[Random.Range(0, fleeLocations.Length)].position;
//         agent.SetDestination(fleePos);
        
//         Debug.Log("FLEE!!");
//     }

//     public void Sus(float amount) {
//         if(fear < -1.0f) return;
//         fear -= amount;
//     }
// }
