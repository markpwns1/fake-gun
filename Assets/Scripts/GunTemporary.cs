using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTemporary : MonoBehaviour
{
    [Header("Sus")]
    public float glanceFear = 0.1f;
    public float hitSus = 0.3f;
    public float scareDistance = 15.0f;
    public float noRecoilSus = 0.1f;
    [Header("Recoil")]
    public float visibleRecoil;
    public float recoilTimeLimit = 1.0f;
    public float requiredRecoilDegrees = 5.0f;
    public float fearCone = 15.0f;
    private float shotAngle;
    // private bool hasntRecoiled = false;
    private Coroutine recoilWaitCoroutine = null;
    // public float 

    private WeaponSway sway;
    // Start is called before the first frame update
    void Start()
    {
        sway = GameObject.FindObjectOfType<WeaponSway>();
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

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if(recoilWaitCoroutine != null) {
                StopCoroutine(recoilWaitCoroutine);
                FailedToRecoil();
                recoilWaitCoroutine = null;
            }

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
            
            sway.AddRecoil(visibleRecoil);

            shotAngle = Camera.main.transform.localEulerAngles.x;
            recoilWaitCoroutine = StartCoroutine(WaitForRecoil());
        }

        if(recoilWaitCoroutine != null && Camera.main.transform.localEulerAngles.x > shotAngle + requiredRecoilDegrees) {
            Debug.Log("Successful recoil");
            StopCoroutine(recoilWaitCoroutine);
            recoilWaitCoroutine = null;
        }
    }
}
