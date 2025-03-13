using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPopup;
    public GameObject losePopup;
    public GameObject newBestPopup;

    void Start()
    {
        gameOverPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvent.GameOver += ShowGameOverPopup;
    }

    private void OnDisable()
    {
        GameEvent.GameOver -= ShowGameOverPopup;
    }

    private void ShowGameOverPopup(bool newBestScore)
    {
        gameOverPopup.SetActive(true);
        losePopup.SetActive(false);
        newBestPopup.SetActive(true);
    }
}
