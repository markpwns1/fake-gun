using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform followPos;
    public Vector3 offset;

    public float lerpSpeed;

    void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, followPos.position, lerpSpeed) + offset;

/*
        var euler = transform.eulerAngles;
        euler.y = Mathf.LerpAngle(euler.y, followPos.eulerAngles.y, 1f * Time.deltaTime);
        transform.eulerAngles = euler; */
    }
}