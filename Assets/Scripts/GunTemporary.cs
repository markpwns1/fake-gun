using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTemporary : MonoBehaviour
{
    [Header("Sus")]
    public float glanceFear = 0.1f;
    public float hitSus = 0.3f;
    public float noRecoilSus = 0.1f;
    [Header("Recoil")]
    public float visibleRecoil;
    public float recoilTimeLimit = 1.0f;
    public float requiredRecoilDegrees = 5.0f;
    private float shotAngle = 0.0f;

    [Header("Reload")]
    public int clipSize = 6;
    public float baseReloadFailSus = 0.1f;
    public float reloadFailSusIncrease = 0.05f;

    [Header("Misc")]
    public float scareDistance = 15.0f;
    public float fearCone = 15.0f;
    // private bool hasntRecoiled = false;

    private float tickingReloadFailSus;
    [HideInInspector]
    public int bulletsShot = 0;
    private Coroutine recoilWaitCoroutine = null;
    // public float 

    private WeaponSway sway;
    // Start is called before the first frame update
    void Start()
    {
        sway = GameObject.FindObjectOfType<WeaponSway>();
        tickingReloadFailSus = baseReloadFailSus;
    }

    void FailedToRecoil() {
        foreach (var enemy in Enemy.alertedEnemies)
        {
            enemy.Sus(noRecoilSus);
        }
        Debug.Log("Failed recoil");
    }

    IEnumerator WaitForRecoil() {
        yield return new WaitForSeconds(recoilTimeLimit);
        FailedToRecoil();
        recoilWaitCoroutine = null;
    }

    float GetHorizonAngle()
    {
        return Mathf.Abs(Vector3.SignedAngle(Vector3.up, Camera.main.transform.up, Camera.main.transform.forward));
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(recoilWaitCoroutine != null) {
                StopCoroutine(recoilWaitCoroutine);
                FailedToRecoil();
                recoilWaitCoroutine = null;
            }

            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, scareDistance)) {
                
                Debug.Log(hit.collider.gameObject.name);
                Debug.Log(hit.collider.gameObject.tag);
                if(hit.collider.gameObject.tag == "Enemy") {
                    Debug.Log("HIT");
                    hit.collider.gameObject.GetComponent<Enemy>().Sus(hitSus);
                }
            }

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward);

            foreach (var enemy in Enemy.allEnemies)
            {
                
                var to = enemy.transform.position - transform.position;
                if(to.magnitude < scareDistance && Vector3.Angle(Camera.main.transform.forward, to) < fearCone) {
                    enemy.Spook(glanceFear);
                }
                
            }
            
            sway.AddRecoil(visibleRecoil);

            shotAngle = GetHorizonAngle();
            recoilWaitCoroutine = StartCoroutine(WaitForRecoil());

            bulletsShot++;
            if(bulletsShot > clipSize) {
                foreach (var enemy in Enemy.alertedEnemies)
                {
                    enemy.Sus(tickingReloadFailSus);
                }
                Debug.Log("Sussy baka! " + tickingReloadFailSus);
                tickingReloadFailSus += reloadFailSusIncrease;
            }
        }

        if(recoilWaitCoroutine != null && GetHorizonAngle() > shotAngle + requiredRecoilDegrees) {
            Debug.Log("Successful recoil");
            StopCoroutine(recoilWaitCoroutine);
            recoilWaitCoroutine = null;
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            bulletsShot = 0;
            tickingReloadFailSus = baseReloadFailSus;
        }
    }
}
