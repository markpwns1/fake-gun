using UnityEngine;
using System.Collections;

public class WeaponSway : MonoBehaviour
{
    public float velocityMultiplier = 0.0005f;

    public Vector3 maxSway;
    public float swayScale;
    public float restSpeed;
    public float recoilRestSpeed;

    [HideInInspector]
    public float recoil;
    [HideInInspector]
    public bool isAiming = false;

    private float generalMultiplier = 1f;
    //private float backwardsMovement = 0f;
    private PlayerMovement movement;

    void Start() {
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update() {
        
        if(isAiming) {
            generalMultiplier = 0.5f;
        }else{
            generalMultiplier = 1f;
        }

        Vector3 localPos = transform.localPosition;
        Vector3 velocity = Camera.main.transform.InverseTransformDirection(movement.GetRigidbody().velocity);

        localPos.x -= velocity.x * velocityMultiplier + Input.GetAxis("Mouse X") * swayScale * generalMultiplier;
        localPos.y -= velocity.y * 5f * velocityMultiplier + Input.GetAxis("Mouse Y") * swayScale * generalMultiplier;

        localPos.x = Mathf.Clamp(Mathf.Lerp(localPos.x, 0, restSpeed), -maxSway.x * generalMultiplier, maxSway.x * generalMultiplier);
        localPos.y = Mathf.Clamp(Mathf.Lerp(localPos.y, 0, restSpeed), -maxSway.y * generalMultiplier, maxSway.y * generalMultiplier);
        localPos.z = Mathf.Clamp(Mathf.Lerp(localPos.z, recoil, restSpeed), -maxSway.z * generalMultiplier, maxSway.z * generalMultiplier);
        localPos.z -= velocity.z * velocityMultiplier;

        recoil = Mathf.Lerp(recoil, 0, recoilRestSpeed);

        transform.localPosition = localPos;
    }

    public void AddRecoil(float amount) {
        recoil -= amount * generalMultiplier;
    }
}
