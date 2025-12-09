using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialBehaviour : MonoBehaviour
{
    [SerializeField]
    public GameObject mainUI;
    [SerializeField]
    public GameObject pasueUI;
    public void Back()
    {
            SceneManager.LoadScene("MainMenu");
    }
}
