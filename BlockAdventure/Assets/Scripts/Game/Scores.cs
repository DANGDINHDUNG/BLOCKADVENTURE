using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class BestScoreData
{
    public int score = 0;
}

public class Scores : MonoBehaviour
{
    #region Defines
    public SquareTextureData squareTextureData;
    public TextMeshProUGUI scoreText;

    private bool newBestScore = false;
    private BestScoreData bestScore_ = new BestScoreData();
    private int currentScores = 0;

    private string bestScoreKey = "bsdat";
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        if (BinaryDataStream.Exist(bestScoreKey))
        {
            StartCoroutine(ReadDataFile());
        }
    }

    private void Start()
    {
        newBestScore = false;
        currentScores = 0;
        squareTextureData.SetStartColor();
        UpdateScoreText();
    }

    private void OnEnable()
    {
        GameEvent.AddScores += AddScores;
        GameEvent.GameOver += SaveBestScore;
    }

    private void OnDisable()
    {
        GameEvent.AddScores -= AddScores;
        GameEvent.GameOver -= SaveBestScore;
    }
    #endregion

    #region Methods
    private IEnumerator ReadDataFile()
    {
        bestScore_ = BinaryDataStream.Read<BestScoreData>(bestScoreKey);
        yield return new WaitForEndOfFrame();
        GameEvent.UpdateBestScoreBar(currentScores, bestScore_.score);
    }

    private void AddScores(int scores)
    {
        currentScores += scores;
        if (currentScores > bestScore_.score)
        {
            newBestScore = true;
            bestScore_.score = currentScores;
            SaveBestScore(true);
        }

        UpdateSquareColor();
        GameEvent.UpdateBestScoreBar(currentScores, bestScore_.score);
        UpdateScoreText();
    }

    public void SaveBestScore(bool newBestScore)
    {
        BinaryDataStream.Save<BestScoreData>(bestScore_, bestScoreKey);
    }

    private void UpdateSquareColor()
    {
        if (GameEvent.UpdateSquareColor != null && currentScores >= squareTextureData.threshHoldVal)
        {
            squareTextureData.UpdateColor(currentScores);
            GameEvent.UpdateSquareColor(squareTextureData.currentColor);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = currentScores.ToString();
    }
    #endregion
}
