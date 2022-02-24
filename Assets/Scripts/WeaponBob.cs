using UnityEngine;
using System.Collections;

public class WeaponBob : MonoBehaviour
{
    public static readonly int maxI = 90;

    public Vector2 speed;
    public Vector2 distance;

    [HideInInspector]
    public bool isAiming = false;
    [HideInInspector]
    public bool isCrouching = false;

    private PlayerMovement movement;
    private float i;

    void Start()
    {
        movement = GameObject.FindObjectOfType<PlayerMovement>();
    }
	
    // Update is called once per frame
    void LateUpdate()
    {
        if(!movement.IsMoving) {
            i = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 0.1f);
            return;
        }

        i += Time.deltaTime;
        if(i >= maxI) {
            i = 0f;
        }

        float mult = 1f;
        if(movement.IsSprinting()) {
            mult = 1.5f;
        }
        if(isCrouching) {
            mult = 0.5f;
        }
        if(isAiming) {
            mult *= 0.1f;
        }

        /*bool resetSpeed = false;
        if(movement.isSprinting && !wasSprintingLastFrame) {
            speed.x *= sprintMultiplier;
            speed.y *= sprintMultiplier;
        }else if(!movement.isSprinting && wasSprintingLastFrame) {
            speed.x /= sprintMultiplier;
            speed.y /= sprintMultiplier;
        }*/

        Vector3 localPos = transform.localPosition;
        localPos.x = Mathf.Sin(i * speed.x) * (distance.x * Mathf.Min(1f, mult));
        localPos.y = Mathf.Abs(Mathf.Sin(i * speed.y) * (distance.y * Mathf.Min(1f, mult)));
        transform.localPosition = Vector3.Lerp(transform.localPosition, localPos, 0.1f);
    }
}
