using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cable : MonoBehaviour
{
    public static Cable instance;
    public Transform cableTransform;
    public TMP_Text cooldownTxt;
    public LineRenderer lineRenderer;
    public Transform follow;
    private float maxDist = 40;
    private Rigidbody rb;
    public float delay;
    private Vector3 cableHitPoint;
    public float cableCooldown = 0.5f;
    private float lastCableTime;
    private bool grappling;
    private Vector3 setVel;
    private float overshootY;

    private Vector3 nigga;
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerInput();
        if (cableCooldown > 0)
        {
            cableCooldown -= Time.deltaTime;
        }
    }
    private void LateUpdate()
    {
        if (grappling) 
        {
            lineRenderer.SetPosition(0, cableTransform.position);
        }
    }
    public void ShootCable()
    {
        if (cableCooldown > 0) return;
        grappling = true;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDist, Movement.instance.floorMask))
        {
            cableHitPoint = hit.point;
            Debug.Log("Hit Point: " + cableHitPoint);
            Invoke(nameof(ExecuteCable), delay);
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, cableHitPoint);
        }
        else
        {
        }
    }

    public void ExecuteCable()
    {
        Vector3 lowestPt = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        float cPointOnY = cableHitPoint.y - lowestPt.y;
        float highestPt = cPointOnY + overshootY;
        if (cPointOnY < 0)
            highestPt = overshootY;
        CablePullPlayer(cableHitPoint, highestPt);
        Invoke(nameof(StopCable), 1);
    }
    public void StopCable()
    {
        grappling = false;
        cableCooldown = lastCableTime;
        lineRenderer.enabled = false;
    }

    public void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !Movement.instance.isGrounded)
        {
            ShootCable();
        }
    }
    private void SetVelocity()
    {
        rb.velocity = setVel;
    }
    public void CablePullPlayer(Vector3 target, float trajectH)
    {
        setVel = CalculateForce(transform.position, target, trajectH);
        Invoke(nameof(SetVelocity), 0.1f);
    }
    public Vector3 CalculateForce(Vector3 start, Vector3 end, float trajHeight)
    {
        float grav = Physics.gravity.y;
        float displacement = end.y - start.y;
        Vector3 displacementXZ = new Vector3(end.x - start.x, 0,end.z - start.z);
        Vector3 velY = Vector3.up * Mathf.Sqrt(-2 * grav * trajHeight);
        Vector3 velXZ = displacementXZ / (Mathf.Sqrt(-2 * trajHeight / grav) + Mathf.Sqrt(2 * (displacement - trajHeight) / grav));
        return velXZ + velY;
    }
}
