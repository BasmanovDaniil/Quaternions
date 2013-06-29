using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 10;
    public float xSpeed = 250;
    public float ySpeed = 120;
    public float yMinLimit = -20;
    public float yMaxLimit = 80;

    private Transform tr;
    private float x;
    private float y;

    private void Start()
    {
        tr = transform;
        x = tr.eulerAngles.x;
        y = tr.eulerAngles.y;
        if (rigidbody != null) rigidbody.freezeRotation = true;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        y = ClampAngle(y, yMinLimit, yMaxLimit);
        tr.rotation = Quaternion.Euler(y, x, 0);
        tr.position = tr.rotation * new Vector3(0, 0, -distance) + target.position;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}