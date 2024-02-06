using UnityEngine;

public class Container : MonoBehaviour
{
    public float Angle => angle;
    public float PourOutAngle => pourOutAngle;
    public float MaxSurfaceDistance => maxSurfaceDistance;

    public Transform NeckPoint => neckPoint;
    public Substance Substance => substance;

    public RaycastHit RaycastHit => raycastHit;


    [SerializeField] float pourOutAngle;
    [SerializeField] float maxSurfaceDistance;

    [SerializeField] Transform neckPoint;
    [SerializeField] Substance substance;

    [SerializeField] LayerMask interactionLayer;

    float angle;

    SubstanceStream substanceStream;
    StreamIndicator streamIndicator;

    RaycastHit raycastHit;

    private void Awake()
    {
        streamIndicator = GetComponent<StreamIndicator>();
        substanceStream = GetComponent<SubstanceStream>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        angle = Vector3.Angle(Vector3.up, transform.up);

        if (angle > pourOutAngle)
            CheckForSurface();
        else
            streamIndicator.SetActive(false);
    }

    void CheckForSurface()
    {
        if (Physics.Raycast(neckPoint.position, Vector3.down, out raycastHit, maxSurfaceDistance, interactionLayer))
        {
            streamIndicator.SetActive(true);
            substanceStream.CheckForSubstance(raycastHit, substance.Name);
        }
        else
            streamIndicator.SetActive(false);
    }
}

