using UnityEngine;
using TMPro;

public class InformationDisplay : MonoBehaviour, IViewInteractable
{
    [SerializeField] string textToShow;
    [SerializeField] float fontSizeMultiplayer;

    [SerializeField] Vector3 offset;

    [SerializeField] TextMeshPro textMeshPro;

    bool isOnView;

    Transform camera;

    Vector3 startScale;

    private void Awake()
    {
        textMeshPro = Instantiate(textMeshPro);
        startScale = textMeshPro.transform.localScale;
    }

    public void Interact(GameObject camera)
    {
        isOnView = true;
        this.camera = camera.transform;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isOnView)
        {
            textMeshPro.transform.localScale = Vector3.Lerp(textMeshPro.transform.localScale, startScale, 12f * Time.deltaTime);

            textMeshPro.transform.rotation = Quaternion.LookRotation(-(camera.position - transform.position));

            textMeshPro.transform.position = transform.position + offset;

            textMeshPro.fontSize *= fontSizeMultiplayer;

            if (!textMeshPro.enabled)
            {
                textMeshPro.enabled = true;
            }

            textMeshPro.text = textToShow;

            isOnView = false;
        }
        else
        {
            textMeshPro.transform.localScale = Vector3.Lerp(textMeshPro.transform.localScale, Vector3.zero, 12f * Time.deltaTime);
        }
    }


}
