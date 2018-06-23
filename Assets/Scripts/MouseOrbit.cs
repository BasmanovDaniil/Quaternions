using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 10;
    public float xSpeed = 250;
    public float ySpeed = 120;
    public float yMinLimit = -20;
    public float yMaxLimit = 80;

    private float x;
    private float y;

    private void Start()
    {
        x = transform.eulerAngles.x;
        y = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        x += Input.GetAxis("Mouse X")*xSpeed*Time.deltaTime;
        y -= Input.GetAxis("Mouse Y")*ySpeed*Time.deltaTime;
        y = ClampAngle(y, yMinLimit, yMaxLimit);
        transform.rotation = Quaternion.Euler(y, x, 0);
        transform.position = transform.rotation*new Vector3(0, 0, -distance) + target.position;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
