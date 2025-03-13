using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : MonoBehaviour
{
    public static UnityAction<bool> GameOver;

    public static UnityAction CheckIfShapeCanbePlaced;

    public static UnityAction MoveShapeToStartPosition;

    public static UnityAction RequestNewShapes;

    public static UnityAction SetShapeInactive;

    public static UnityAction<int> AddScores;

    public static UnityAction<int, int> UpdateBestScoreBar;

    public static UnityAction<Config.SquareColor> UpdateSquareColor;

    public static UnityAction ShowCongratulationWritings;

    public static UnityAction<Config.SquareColor> ShowBonusScreen; 
}
