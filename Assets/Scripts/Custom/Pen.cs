using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pen : NetworkBehaviour
{
    public static Pen Instance { get; private set; }

    [SerializeField] LineRenderer lineRendererPrefab;

    LineRenderer activeLine;
    [SerializeField] LayerMask penLayer;
    [SerializeField] float distanceThreshold;

    [SerializeField] Color color;
    [SerializeField] float width;
    // [SerializeField] Camera playerCamera;
    //[SerializeField] Transform indexFinger;
    [SerializeField] float smoothness;
    int currentOrder;
    [SerializeField] Transform crosshair;

    [SerializeField] Transform penDumper;

    List<LineRenderer> spawnedLines;
    [SerializeField] InputAction drawButton;
    [SerializeField] InputActionReference drawButtonReference;
    [SerializeField] bool isRayStarted;

    private void Awake()
    {
        Instance = this;

        spawnedLines = new List<LineRenderer>();
        drawButton.Enable();
    }


    // Update is called once per frame
    void Update()
    {
        //if (playerCamera == null)
        //{

        //    return;
        //}

        if (!IndexFingerMarker.Instance)
            return;

        var indexFinger = IndexFingerMarker.Instance.transform;


        penDumper.position = Vector3.Lerp(penDumper.position, indexFinger.position, smoothness * Time.deltaTime);
        penDumper.rotation = Quaternion.Lerp(penDumper.rotation, indexFinger.rotation, smoothness * Time.deltaTime);

        var ray = new Ray(penDumper.position, penDumper.forward);
        var hit = new RaycastHit();

        if (!Physics.Raycast(ray, out hit, 999f, penLayer.value))
        {
            crosshair.gameObject.SetActive(false);
            return;
        }

        crosshair.position = hit.point;
        crosshair.rotation = Quaternion.LookRotation(hit.normal);

        if (!crosshair.gameObject.activeSelf)
        {
            crosshair.gameObject.SetActive(true);
        }

        if (drawButton.ReadValue<float>() > 0f)
        {
            if (!isRayStarted)
            {
                CreateLine(hit.point);
                isRayStarted = true;
            }

            if (activeLine != null)
            {
                var last = activeLine.GetPosition(activeLine.positionCount - 1);
                var distance = Vector3.Distance(hit.point, last);

                if (distance > distanceThreshold)
                {
                    UpdateLine(hit.point);
                }

                //crosshair.position = hit.point;
                //crosshair.rotation = Quaternion.LookRotation(hit.normal);
            }
        }
        else if (isRayStarted)
        {
            EndLine();
            isRayStarted = false;
        }
    }

    void CreateLine(Vector3 startPoint)
    {
        activeLine = Instantiate(lineRendererPrefab);
        activeLine.positionCount = 1;
        activeLine.SetPosition(0, startPoint);
        var gradient = new Gradient();
        gradient.SetKeys(
        new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
        new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f), new GradientAlphaKey(1f, 1.0f) }
    );

        activeLine.colorGradient = gradient;
        activeLine.widthMultiplier = width;
        activeLine.sortingOrder = currentOrder++;
    }
    void UpdateLine(Vector3 newPoint)
    {
        activeLine.positionCount += 1;
        activeLine.SetPosition(activeLine.positionCount - 1, newPoint);
    }

    void EndLine()
    {
        if (!activeLine)
            return;

        var positions = new Vector3[activeLine.positionCount];
        activeLine.GetPositions(positions);
        RPC_CreateLineNetwork(positions, activeLine.widthMultiplier, Runner.LocalPlayer.PlayerId);
        activeLine = null;
        //  crosshair.gameObject.SetActive(false);
    }

    void DrawLine(Vector3[] points, float width)
    {
        var line = Instantiate(lineRendererPrefab);
        line.positionCount = points.Length;
        line.SetPositions(points);
        line.widthMultiplier = width;
        line.sortingOrder = currentOrder++;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    void RPC_CreateLineNetwork(Vector3[] points, float width, int senderId)
    {
        if (senderId == Runner.LocalPlayer.PlayerId)
            return;

        DrawLine(points, width);
    }
}
