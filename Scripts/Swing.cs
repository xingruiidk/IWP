using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public static Swing instance;
    public LineRenderer lineRenderer;
    public Transform cableTip, player;
    public LayerMask layerMask;
    public float spring, damper, ratio;
    private float maxDist = 40;
    private Vector3 cablePoint;
    private SpringJoint joint;
    private Vector3 cablePosition;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !Movement.instance.isGrounded) 
        {
            StartSwing();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            StopSwing();
        }
    }
    private void LateUpdate()
    {
        DrawCable();
    }
    public void StartSwing()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDist, layerMask)) 
        {
            cablePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = cablePoint;
            float distFromPt = Vector3.Distance(player.position, cablePoint);
            if (distFromPt > 15)
            {
                joint.maxDistance = distFromPt * 0.3f;
                joint.minDistance = distFromPt * 0.15f;
            }
            else
            {
                joint.maxDistance = distFromPt * 0.8f;
                joint.minDistance = distFromPt * 0.15f;
            }
            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = ratio;
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = true;
            cablePosition = cableTip.position;
        }
    }
    public void StopSwing()
    {
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        Destroy(joint);
    }
    public void DrawCable()
    {
        if (!joint)
        {
            return;
        }
        cablePosition = Vector3.Lerp(cablePosition, cablePoint, Time.deltaTime * 8);
        lineRenderer.SetPosition(0, cableTip.position);
        lineRenderer.SetPosition(1, cablePoint);
    }
}
