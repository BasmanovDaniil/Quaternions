using UnityEngine;
using System.Collections.Generic;

public class Quaternions : MonoBehaviour
{
    public GUIText leftEuler;
    public GUIText leftQuaternion;
    public GUIText rightEuler;
    public GUIText rightQuaternion;

    public Transform vectorArrow;
    public Transform quaternionArrow;
    public Transform vectorGoal;
    public Transform quaternionGoal;

    public Material xTrailD;
    public Material yTrailD;
    public Material zTrailD;
    public Material xTrail;
    public Material yTrail;
    public Material zTrail;

    public Camera cam;

    private const float lineWidth = 0.01f;

    private LineRenderer vectorX;
    private LineRenderer vectorY;
    private LineRenderer vectorZ;
    private LineRenderer quaternionX;
    private LineRenderer quaternionY;
    private LineRenderer quaternionZ;
    private List<Vector3> vectorXPoints;
    private List<Vector3> vectorYPoints;
    private List<Vector3> vectorZPoints;
    private List<Vector3> quaternionXPoints;
    private List<Vector3> quaternionYPoints;
    private List<Vector3> quaternionZPoints;
    private bool follow;
    private MouseOrbit orbit;

    private void Start()
    {
        Cursor.visible = false;
        orbit = cam.GetComponent<MouseOrbit>();

        vectorXPoints = new List<Vector3>();
        vectorYPoints = new List<Vector3>();
        vectorZPoints = new List<Vector3>();
        quaternionXPoints = new List<Vector3>();
        quaternionYPoints = new List<Vector3>();
        quaternionZPoints = new List<Vector3>();

        vectorX = GameObject.Find("X Trail").AddComponent<LineRenderer>();
        vectorY = GameObject.Find("Y Trail").AddComponent<LineRenderer>();
        vectorZ = GameObject.Find("Z Trail").AddComponent<LineRenderer>();
        quaternionX = GameObject.Find("X Trail Dark").AddComponent<LineRenderer>();
        quaternionY = GameObject.Find("Y Trail Dark").AddComponent<LineRenderer>();
        quaternionZ = GameObject.Find("Z Trail Dark").AddComponent<LineRenderer>();

        vectorX.material = xTrailD;
        vectorY.material = yTrailD;
        vectorZ.material = zTrailD;
        quaternionX.material = xTrail;
        quaternionY.material = yTrail;
        quaternionZ.material = zTrail;

        vectorX.startWidth = lineWidth;
        vectorX.endWidth = lineWidth;
        vectorY.startWidth = lineWidth;
        vectorY.endWidth = lineWidth;
        vectorZ.startWidth = lineWidth;
        vectorZ.endWidth = lineWidth;

        quaternionX.startWidth = lineWidth;
        quaternionX.endWidth = lineWidth;
        quaternionY.startWidth = lineWidth;
        quaternionY.endWidth = lineWidth;
        quaternionZ.startWidth = lineWidth;
        quaternionZ.endWidth = lineWidth;

        ResetTrails();
        InvokeRepeating("UpdateTrails", 0.04f, 0.04f);
    }

    private void Update()
    {
        if (Input.GetKey("escape")) Application.Quit();
        if (Input.GetKeyDown("space"))
        {
            vectorArrow.rotation = Quaternion.identity;
            quaternionArrow.rotation = Quaternion.identity;
            vectorGoal.rotation = Quaternion.identity;
            quaternionGoal.rotation = Quaternion.identity;
            follow = false;
            ResetTrails();
        }

        if (Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift"))
        {
            var nextRotation = Quaternion.Euler(Random.value*89, Random.value*180, Random.value*180);
            vectorGoal.rotation = nextRotation;
            quaternionGoal.rotation = nextRotation;
            follow = true;
            ResetTrails();
        }

        if (Input.GetKeyDown("left ctrl") || Input.GetKeyDown("right ctrl"))
        {
            var nextRotation = Random.rotationUniform;
            vectorGoal.rotation = nextRotation;
            quaternionGoal.rotation = nextRotation;
            follow = true;
            ResetTrails();
        }

        if (Input.GetKeyDown("tab"))
        {
            vectorArrow.rotation = Quaternion.identity;
            quaternionArrow.rotation = Quaternion.identity;
            vectorGoal.rotation = Quaternion.identity;
            quaternionGoal.rotation = Quaternion.identity;
            if (vectorArrow.position == Vector3.zero && quaternionArrow.position == Vector3.zero)
            {
                vectorArrow.position = new Vector3(-0.6f, 0, 0);
                vectorGoal.position = new Vector3(-0.6f, 0, 0);
                quaternionArrow.position = new Vector3(0.6f, 0, 0);
                quaternionGoal.position = new Vector3(0.6f, 0, 0);
                vectorArrow.gameObject.SetActive(true);
                vectorGoal.gameObject.SetActive(true);
                ResetTrails();
                orbit.distance = 3;
            }
            else
            {
                vectorArrow.position = Vector3.zero;
                vectorGoal.position = Vector3.zero;
                quaternionArrow.position = Vector3.zero;
                quaternionGoal.position = Vector3.zero;
                vectorArrow.gameObject.SetActive(false);
                vectorGoal.gameObject.SetActive(false);
                ResetTrails();
                orbit.distance = 2;
            }
        }

        if (Input.GetKey("d")) quaternionArrow.rotation *= Quaternion.Euler(+1, 0, 0);
        if (Input.GetKey("a")) quaternionArrow.rotation *= Quaternion.Euler(-1, 0, 0);
        if (Input.GetKey("w")) quaternionArrow.rotation *= Quaternion.Euler(0, +1, 0);
        if (Input.GetKey("s")) quaternionArrow.rotation *= Quaternion.Euler(0, -1, 0);
        if (Input.GetKey("e")) quaternionArrow.rotation *= Quaternion.Euler(0, 0, +1);
        if (Input.GetKey("q")) quaternionArrow.rotation *= Quaternion.Euler(0, 0, -1);

        if (Input.GetKey("d")) vectorArrow.eulerAngles += new Vector3(+1, 0, 0);
        if (Input.GetKey("a")) vectorArrow.eulerAngles += new Vector3(-1, 0, 0);
        if (Input.GetKey("w")) vectorArrow.eulerAngles += new Vector3(0, +1, 0);
        if (Input.GetKey("s")) vectorArrow.eulerAngles += new Vector3(0, -1, 0);
        if (Input.GetKey("e")) vectorArrow.eulerAngles += new Vector3(0, 0, +1);
        if (Input.GetKey("q")) vectorArrow.eulerAngles += new Vector3(0, 0, -1);

        if (follow)
        {
            if (vectorArrow.eulerAngles != vectorGoal.eulerAngles)
            {
                vectorArrow.eulerAngles = Vector3.Slerp(vectorArrow.eulerAngles, vectorGoal.eulerAngles, 0.02f);
            }
            if (quaternionArrow.rotation != quaternionGoal.rotation)
            {
                quaternionArrow.rotation = Quaternion.Slerp(quaternionArrow.rotation, quaternionGoal.rotation, 0.02f);
            }
        }

        leftEuler.text = "<color=#f14121ff>X: " + quaternionArrow.eulerAngles.x.ToString("F0") + "</color>"
                         + "\n<color=#98f145ff>Y: " + quaternionArrow.eulerAngles.y.ToString("F0") + "</color>"
                         + "\n<color=#3d80f1ff>Z: " + quaternionArrow.eulerAngles.z.ToString("F0") + "</color>";
        leftQuaternion.text = "<color=#f14121ff>QX: " + quaternionArrow.rotation.x.ToString("F2") + "</color>"
                              + "\n<color=#98f145ff>QY: " + quaternionArrow.rotation.y.ToString("F2") + "</color>"
                              + "\n<color=#3d80f1ff>QZ: " + quaternionArrow.rotation.z.ToString("F2") + "</color>"
                              + "\nQW: " + quaternionArrow.rotation.w.ToString("F2");
        rightEuler.text = "<color=#f14121ff>X: " + vectorArrow.eulerAngles.x.ToString("F0") + "</color>"
                          + "\n<color=#98f145ff>Y: " + vectorArrow.eulerAngles.y.ToString("F0") + "</color>"
                          + "\n<color=#3d80f1ff>Z: " + vectorArrow.eulerAngles.z.ToString("F0") + "</color>";
        rightQuaternion.text = "<color=#f14121ff>QX: " + vectorArrow.rotation.x.ToString("F2") + "</color>"
                               + "\n<color=#98f145ff>QY: " + vectorArrow.rotation.y.ToString("F2") + "</color>"
                               + "\n<color=#3d80f1ff>QZ: " + vectorArrow.rotation.z.ToString("F2") + "</color>"
                               + "\nQW: " + vectorArrow.rotation.w.ToString("F2");
    }

    private void ResetTrails()
    {
        vectorXPoints.Clear();
        vectorXPoints.Add(vectorArrow.position + vectorArrow.forward*0.5f);
        vectorX.positionCount = 1;
        vectorX.SetPosition(0, vectorXPoints[0]);

        vectorYPoints.Clear();
        vectorYPoints.Add(vectorArrow.position + vectorArrow.up*0.5f);
        vectorY.positionCount = 1;
        vectorY.SetPosition(0, vectorYPoints[0]);

        vectorZPoints.Clear();
        vectorZPoints.Add(vectorArrow.position + vectorArrow.right*0.5f);
        vectorZ.positionCount = 1;
        vectorZ.SetPosition(0, vectorZPoints[0]);

        quaternionXPoints.Clear();
        quaternionXPoints.Add(quaternionArrow.position + quaternionArrow.forward*0.5f);
        quaternionX.positionCount = 1;
        quaternionX.SetPosition(0, quaternionXPoints[0]);

        quaternionYPoints.Clear();
        quaternionYPoints.Add(quaternionArrow.position + quaternionArrow.up*0.5f);
        quaternionY.positionCount = 1;
        quaternionY.SetPosition(0, quaternionYPoints[0]);

        quaternionZPoints.Clear();
        quaternionZPoints.Add(quaternionArrow.position + quaternionArrow.right*0.5f);
        quaternionZ.positionCount = 1;
        quaternionZ.SetPosition(0, quaternionZPoints[0]);
    }

    private void UpdateTrails()
    {
        if (vectorXPoints[vectorXPoints.Count - 1] != vectorArrow.position + vectorArrow.forward*0.5f)
        {
            vectorXPoints.Add(vectorArrow.position + vectorArrow.forward*0.5f);
            vectorX.positionCount = vectorXPoints.Count;
            vectorX.SetPosition(vectorXPoints.Count - 1, vectorXPoints[vectorXPoints.Count - 1]);
        }
        if (vectorYPoints[vectorYPoints.Count - 1] != vectorArrow.position + vectorArrow.up*0.5f)
        {
            vectorYPoints.Add(vectorArrow.position + vectorArrow.up*0.5f);
            vectorY.positionCount = vectorYPoints.Count;
            vectorY.SetPosition(vectorYPoints.Count - 1, vectorYPoints[vectorYPoints.Count - 1]);
        }
        if (vectorZPoints[vectorZPoints.Count - 1] != vectorArrow.position + vectorArrow.right*0.5f)
        {
            vectorZPoints.Add(vectorArrow.position + vectorArrow.right*0.5f);
            vectorZ.positionCount = vectorZPoints.Count;
            vectorZ.SetPosition(vectorZPoints.Count - 1, vectorZPoints[vectorZPoints.Count - 1]);
        }

        if (quaternionXPoints[quaternionXPoints.Count - 1] != quaternionArrow.forward*0.5f)
        {
            quaternionXPoints.Add(quaternionArrow.position + quaternionArrow.forward*0.5f);
            quaternionX.positionCount = quaternionXPoints.Count;
            quaternionX.SetPosition(quaternionXPoints.Count - 1, quaternionXPoints[quaternionXPoints.Count - 1]);
        }
        if (quaternionYPoints[quaternionYPoints.Count - 1] != quaternionArrow.up*0.5f)
        {
            quaternionYPoints.Add(quaternionArrow.position + quaternionArrow.up*0.5f);
            quaternionY.positionCount = quaternionYPoints.Count;
            quaternionY.SetPosition(quaternionYPoints.Count - 1, quaternionYPoints[quaternionYPoints.Count - 1]);
        }
        if (quaternionZPoints[quaternionZPoints.Count - 1] != quaternionArrow.right*0.5f)
        {
            quaternionZPoints.Add(quaternionArrow.position + quaternionArrow.right*0.5f);
            quaternionZ.positionCount = quaternionZPoints.Count;
            quaternionZ.SetPosition(quaternionZPoints.Count - 1, quaternionZPoints[quaternionZPoints.Count - 1]);
        }
    }
}