using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SquareTextureData")]
[System.Serializable]
public class SquareTextureData : ScriptableObject
{
    [System.Serializable]
    public class TextureData
    {
        public Sprite texture;
        public Config.SquareColor squareColor;
    }

    public int threshHoldVal = 10;
    private const int startThreshHoldVal = 10;
    public List<TextureData> activeSquareData = new List<TextureData>();

    public Config.SquareColor currentColor;
    private Config.SquareColor _nextColor;

    public int GetCurrentColorIndex()
    {
        var currentIndex = 0;
        for (int i = 0; i < activeSquareData.Count; i++)
        {
            if (activeSquareData[i].squareColor == currentColor)
            {
                currentIndex = i;
            }
        }

        return currentIndex;
    }

    public void UpdateColor(int current_score)
    {
        currentColor = _nextColor;
        var currentColorIndex = GetCurrentColorIndex();

        if (currentColorIndex == activeSquareData.Count - 1)
        {
            _nextColor = activeSquareData[0].squareColor;
        }
        else
        {
            _nextColor = activeSquareData[currentColorIndex + 1].squareColor;
        }

        threshHoldVal = startThreshHoldVal + current_score;
    }

    public void SetStartColor()
    {
        threshHoldVal = startThreshHoldVal;
        currentColor = activeSquareData[0].squareColor;
        _nextColor = activeSquareData[1].squareColor;
    }

    private void Awake()
    {
        SetStartColor();
    }

    private void OnEnable()
    {
        SetStartColor();
    }
}
