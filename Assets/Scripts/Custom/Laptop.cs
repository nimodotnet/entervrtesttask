using UnityEngine;
using UnityEngine.Events;

public class Laptop : MonoBehaviour, IViewInteractable
{
    public UnityEvent<bool> StateChanged => stateChanged;

    [SerializeField] UnityEvent<bool> stateChanged;

    enum UIState
    {
        Enabled,
        Disabled
    }

    [SerializeField] float timeToActivate;

    [SerializeField] UIState currentState;

    bool isOnView;
    float timer;

    public void Interact(GameObject camera)
    {
        isOnView = true;
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
            timer += Time.deltaTime;

            if (timer > timeToActivate)
            {
                SetUIState(UIState.Enabled);
            }
        }
        else
        {
            SetUIState(UIState.Disabled);
        }

        isOnView = false;
    }

    void SetUIState(UIState state)
    {
        if (currentState == state)
            return;

        currentState = state;

        stateChanged?.Invoke(state == UIState.Enabled ? true : false);

        print("stateChanged");
    }
}
