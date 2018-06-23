using UnityEngine;
using UnityEngine.UI;

public class Quaternions : MonoBehaviour
{
    public MouseOrbit orbit;
    [Header("Arrows")]
    public Transform vectorArrow;
    public Transform vectorGoal;
    public GameObject vectorModel;
    public Transform quaternionArrow;
    public Transform quaternionGoal;
    public GameObject quaternionModel;
    [Header("UI")]
    public Text leftEuler;
    public Text leftQuaternion;
    public Text rightEuler;
    public Text rightQuaternion;
    [Header("Trails")]
    public TrailRenderer vectorX;
    public TrailRenderer vectorY;
    public TrailRenderer vectorZ;
    public TrailRenderer quaternionX;
    public TrailRenderer quaternionY;
    public TrailRenderer quaternionZ;

    private const string eulerFormat = "<color=#f14121ff>X: {0:F0}°</color>\n" +
                                       "<color=#98f145ff>Y: {1:F0}°</color>\n" +
                                       "<color=#3d80f1ff>Z: {2:F0}°</color>";
    private const string quaternionFormat = "<color=#f14121ff>QX: {0:F2}</color>\n" +
                                            "<color=#98f145ff>QY: {1:F2}</color>\n" +
                                            "<color=#3d80f1ff>QZ: {2:F2}</color>\n" +
                                            "QW: {3:F2}";
    private const float offset = 0.6f;
    private const float followSpeed = 2;

    private bool follow;
    private bool joined;

    private void Start()
    {
        ResetTrails();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetRotation();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SetGoal(Quaternion.Euler(Random.value*89, Random.value*180, Random.value*180));
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            SetGoal(Random.rotationUniform);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (joined)
            {
                joined = false;
                vectorArrow.position = new Vector3(-offset, 0, 0);
                vectorGoal.position = new Vector3(-offset, 0, 0);
                quaternionArrow.position = new Vector3(offset, 0, 0);
                quaternionGoal.position = new Vector3(offset, 0, 0);
                vectorModel.gameObject.SetActive(true);
                vectorGoal.gameObject.SetActive(true);
                orbit.distance = 3;
            }
            else
            {
                joined = true;
                vectorArrow.position = Vector3.zero;
                vectorGoal.position = Vector3.zero;
                quaternionArrow.position = Vector3.zero;
                quaternionGoal.position = Vector3.zero;
                vectorModel.gameObject.SetActive(false);
                vectorGoal.gameObject.SetActive(false);
                orbit.distance = 2;
            }

            ResetRotation();
        }

        if (Input.GetKey(KeyCode.D)) quaternionArrow.rotation *= Quaternion.Euler(+1, 0, 0);
        if (Input.GetKey(KeyCode.A)) quaternionArrow.rotation *= Quaternion.Euler(-1, 0, 0);
        if (Input.GetKey(KeyCode.W)) quaternionArrow.rotation *= Quaternion.Euler(0, +1, 0);
        if (Input.GetKey(KeyCode.S)) quaternionArrow.rotation *= Quaternion.Euler(0, -1, 0);
        if (Input.GetKey(KeyCode.E)) quaternionArrow.rotation *= Quaternion.Euler(0, 0, +1);
        if (Input.GetKey(KeyCode.Q)) quaternionArrow.rotation *= Quaternion.Euler(0, 0, -1);

        if (Input.GetKey(KeyCode.D)) vectorArrow.eulerAngles += new Vector3(+1, 0, 0);
        if (Input.GetKey(KeyCode.A)) vectorArrow.eulerAngles += new Vector3(-1, 0, 0);
        if (Input.GetKey(KeyCode.W)) vectorArrow.eulerAngles += new Vector3(0, +1, 0);
        if (Input.GetKey(KeyCode.S)) vectorArrow.eulerAngles += new Vector3(0, -1, 0);
        if (Input.GetKey(KeyCode.E)) vectorArrow.eulerAngles += new Vector3(0, 0, +1);
        if (Input.GetKey(KeyCode.Q)) vectorArrow.eulerAngles += new Vector3(0, 0, -1);

        if (follow)
        {
            if (vectorArrow.eulerAngles != vectorGoal.eulerAngles)
            {
                vectorArrow.eulerAngles = Vector3.Lerp(vectorArrow.eulerAngles, vectorGoal.eulerAngles, Time.deltaTime*followSpeed);
            }
            if (quaternionArrow.rotation != quaternionGoal.rotation)
            {
                quaternionArrow.rotation = Quaternion.Lerp(quaternionArrow.rotation, quaternionGoal.rotation, Time.deltaTime*followSpeed);
            }
        }

        leftEuler.text = string.Format(eulerFormat,
            quaternionArrow.eulerAngles.x, quaternionArrow.eulerAngles.y, quaternionArrow.eulerAngles.z);
        leftQuaternion.text = string.Format(quaternionFormat,
            quaternionArrow.rotation.x, quaternionArrow.rotation.y, quaternionArrow.rotation.z, quaternionArrow.rotation.w);

        rightEuler.text = string.Format(eulerFormat,
            vectorArrow.eulerAngles.x, vectorArrow.eulerAngles.y, vectorArrow.eulerAngles.z);
        rightQuaternion.text = string.Format(quaternionFormat,
            vectorArrow.rotation.x, vectorArrow.rotation.y, vectorArrow.rotation.z, vectorArrow.rotation.w);
    }

    private void ResetTrails()
    {
        vectorX.Clear();
        vectorY.Clear();
        vectorZ.Clear();
        quaternionX.Clear();
        quaternionY.Clear();
        quaternionZ.Clear();
    }

    private void ResetRotation()
    {
        vectorArrow.rotation = Quaternion.identity;
        quaternionArrow.rotation = Quaternion.identity;
        vectorGoal.rotation = Quaternion.identity;
        quaternionGoal.rotation = Quaternion.identity;
        follow = false;
        ResetTrails();
    }

    private void SetGoal(Quaternion rotation)
    {
        vectorGoal.rotation = rotation;
        quaternionGoal.rotation = rotation;
        follow = true;
        ResetTrails();
    }
}
