using UnityEngine;

public class GoblinUIFade : MonoBehaviour
{
    private Renderer[] renderers;
    private float visabilityTimer = 0.0f;
    private const float TOGGLE_TIME = 2.0f;
    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        visabilityTimer += Time.deltaTime;

        if(visabilityTimer > TOGGLE_TIME) 
        {
            visabilityTimer = 0.0f;
            ToggleVisable();
        }
    }

    public void ToggleVisable()
    {
        foreach (Renderer renderer in renderers) 
        {
            renderer.enabled = !renderer.enabled;
        }
    }
}
