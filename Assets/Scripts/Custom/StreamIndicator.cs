using UnityEngine;

public class StreamIndicator : MonoBehaviour
{
    [SerializeField] float lineAnimationSpeed;
    [SerializeField] LineRenderer lineRenderer;

    bool isActive;

    Container container;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        container = GetComponent<Container>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            lineRenderer.SetPosition(1, container.NeckPoint.position);
            lineRenderer.SetPosition(0, container.RaycastHit.point);

            AnimateLine();
        }
    }

    public void SetActive(bool state)
    {
        lineRenderer.enabled = state;
        isActive = state;
    }

    void AnimateLine()
    {
        var lineMaterial = lineRenderer.sharedMaterial;
        var offset = lineMaterial.mainTextureOffset;

        offset.x += Time.deltaTime * lineAnimationSpeed * Mathf.Abs(container.PourOutAngle - container.Angle);

        lineMaterial.mainTextureOffset = offset;
    }
}
