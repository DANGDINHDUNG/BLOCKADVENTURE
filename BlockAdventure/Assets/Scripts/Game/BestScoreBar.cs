using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BestScoreBar : MonoBehaviour
{
    public Image fillInImage;
    public TextMeshProUGUI bestScoreText;

    private void OnEnable()
    {
        GameEvent.UpdateBestScoreBar += UpdateBestScoreBar;
    }

    private void OnDisable()
    {
        GameEvent.UpdateBestScoreBar -= UpdateBestScoreBar;
    }

    private void UpdateBestScoreBar(int currentScores, int bestScore)
    {
        bestScoreText.text = bestScore.ToString();
        fillInImage.fillAmount = (float)currentScores / bestScore;
    }
}
