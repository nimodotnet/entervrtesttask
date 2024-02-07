using UnityEngine;

public class Container : MonoBehaviour
{
    public float MaxSurfaceDistance => maxStreamHeight;
    public float PourOutAngle => pourOutAngle;
    public float Angle => angle;

    public Transform NeckPoint => neckPoint;
    public Substance Substance => substance;

    public RaycastHit RaycastHit => raycastHit;


    [SerializeField] float maxStreamHeight;
    [SerializeField] float pourOutAngle;

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
        if (Physics.Raycast(neckPoint.position, Vector3.down, out raycastHit, maxStreamHeight, interactionLayer))
        {
            streamIndicator.SetActive(true);
            substanceStream.CheckForSubstance(raycastHit, substance.Name);
        }
        else
            streamIndicator.SetActive(false);
    }
}

