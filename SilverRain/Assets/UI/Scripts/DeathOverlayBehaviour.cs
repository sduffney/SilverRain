using System;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class DeathOverlayBehaviour : MonoBehaviour
{
    private const string MAIN_SCENE = "MainMenu";

    [SerializeField]
    private TMP_Text scoreText;
    private int score = 0;

    public void Retry()
    {
        //TODO
        //Reload level

        gameObject.SetActive(false);
    }

    public void MainMenu() 
    {
        SceneManager.LoadScene(MAIN_SCENE);
    }

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }


}
