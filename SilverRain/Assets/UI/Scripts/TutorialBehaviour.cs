using UnityEngine;

public class TutorialBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject mainUI;
    [SerializeField]
    public GameObject pasueUI;
    public void Back()
    {
        if (mainUI != null)
        {
            mainUI.SetActive(true);
        }
        else if (pasueUI != null)
        {
            pasueUI.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
