using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] private Light lightObject;
    private bool on = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightObject.enabled = on;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lightObject.enabled = !lightObject.enabled;
        }
    }
}
