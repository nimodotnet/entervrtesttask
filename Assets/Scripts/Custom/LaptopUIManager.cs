using UnityEngine;

public class LaptopUIManager : MonoBehaviour
{
    Laptop laptop;

    private void Awake()
    {
        laptop = GetComponent<Laptop>();    
    }

    // Start is called before the first frame update
    void Start()
    {
        laptop.StateChanged.AddListener(OnLaptopStateChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLaptopStateChanged(bool state)
    {

    }
}
