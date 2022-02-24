using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float glanceFear = 0.1f;
    public float hitSus = 0.3f;
    public float scareDistance = 15.0f;
    public float fearCone = 15.0f;
    public float recoil;

    public int magazine = 6;
    public float notReloadSus = 0.3f;
    private int bullets_shot = 0;

    public float recoilRequired = 0.1f;
    public float recoilTime = 0.5f;
    public float notRecoilSus = 0.3f;
    private float recoilTimer;
    private bool recoilStarted;
    private float oldPosition;
    private float newPosition;

    private WeaponSway sway;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sway = GameObject.FindObjectOfType<WeaponSway>();
        recoilTimer = 0.0f;
        recoilStarted = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            bullets_shot++;
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

                if (bullets_shot > magazine) {
                    enemy.Sus(notReloadSus);
                }
                
            }
            sway.AddRecoil(recoil);
            recoilStarted = true;
            oldPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.localRotation.x;
        }

        if (recoilStarted) {
            if (recoilTimer > recoilTime) {
                recoilStarted = false;
                newPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.localRotation.x;
                float recoilDetected = oldPosition - newPosition;
                if (recoilDetected < recoilRequired)
                {
                    foreach (var enemy in Enemy.allEnemies) enemy.Sus(notRecoilSus);
                }
                else {
                    Debug.Log("Cool");
                }
                recoilTimer = 0.0f;
            }
            else {
                recoilTimer += Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            bullets_shot = 0;
        }
    }
}
