﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class này quản lý grid system, tạo table grid.
/// </summary>
public class GridSystem : MonoBehaviour
{
    #region Defines
    public ShapeStorage shapeStorage;
    public int columns = 0;
    public int rows = 0;

    public GameObject gridSquarePrefab;

    public float squareScale = 0.5f;
    public SquareTextureData squareTextureData;

    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndicator _lineIndicator;
    [SerializeField] private Config.SquareColor currentActiveSquareColor = Config.SquareColor.NotSet;
    [SerializeField] private List<Config.SquareColor> colorInTheGrid = new List<Config.SquareColor>();
    #endregion

    #region Core MonoBehaviours
    // Start is called before the first frame update
    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
        currentActiveSquareColor = squareTextureData.activeSquareData[0].squareColor;
    }

    private void OnDisable()
    {
        GameEvent.CheckIfShapeCanbePlaced -= CheckIfShapeCanBePlaced;
        GameEvent.UpdateSquareColor -= OnUpdateSquareColor;
        GameEvent.CheckIfPlayerLost -= CheckIfPlayerLost;
    }

    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanbePlaced += CheckIfShapeCanBePlaced;
        GameEvent.UpdateSquareColor += OnUpdateSquareColor;
        GameEvent.CheckIfPlayerLost += CheckIfPlayerLost;
    }
    #endregion

    #region Methods
    private void OnUpdateSquareColor(Config.SquareColor color)
    {
        currentActiveSquareColor = color;
    }

    private List<Config.SquareColor> GetAllSquareColorsInTheGrid()
    {
        var colors = new List<Config.SquareColor>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.SquareOccupied)
            {
                var color = gridSquare.GetCurrentColor();
                
                if (colors.Contains(color) == false)
                {
                    colors.Add(color);
                }
            }
        }

        return colors;
    }

    /// <summary>
    /// Tạo grid đặt các block
    /// </summary>
    private void CreateGrid()
    {
        SpawnGridSquares();
        //SetGridSquaresPosition();
    }

    //private void SetGridSquaresPosition()
    //{
    //    // 0 1 2 3 4
    //    // 5 6 7 8 9

    //    int column_number = 0;
    //    int row_number = 0;
    //    Vector2 square_gap_number = new Vector2(0,0);
    //    bool row_moved = false;

    //    var square_rect = _gridSquares[0].GetComponent<RectTransform>();

    //    _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + squareOffset;
    //    _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + squareOffset;

    //    foreach (GameObject square in _gridSquares)
    //    {
    //        if (column_number + 1 > columns)
    //        {
    //            square_gap_number.x = 0;
    //            //Go to the next column
    //            column_number = 0;
    //            row_number++;
    //            row_moved = true;
    //        }

    //        var pos_x_offset = _offset.x * column_number + (square_gap_number.x * squaresGap);
    //        var pos_y_offset = _offset.y * row_number + (square_gap_number.y * squaresGap);

    //        if (column_number > 0 && column_number %3 == 0 && row_moved == false)
    //        {
    //            row_moved = true;
    //            square_gap_number.y++;
    //            pos_y_offset += squaresGap;
    //        }

    //        if (row_number > 0 && row_number % 3 == 0)
    //        {
    //            square_gap_number.y++;
    //            pos_y_offset += squaresGap;
    //        }

    //        square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + pos_x_offset, startPos.y - pos_y_offset);

    //        square.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x + pos_x_offset, startPos.y - pos_y_offset, 0f);

    //        column_number++;
    //    }
    //}

    private void SpawnGridSquares()
    {
        int square_index = 0;

        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                _gridSquares.Add(Instantiate(gridSquarePrefab) as GameObject);

                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(_lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);

                square_index++;
            }
        }
    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexes = new List<int>();

        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();


            if (gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexes.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActiveSquare();
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();

        if (currentSelectedShape == null) return;  //Không có shape nào được chọn

        if (currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
        {
            foreach (var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(currentActiveSquareColor);
            }

            var shapeLeft = 0;
            foreach (var shape in shapeStorage.shapeList)
            {
                if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }

            if (shapeLeft == 0)
            {
                GameEvent.RequestNewShapes();
            }
            else
            {
                GameEvent.SetShapeInactive();
            }

            CheckIfAnyLineIsCompleted();
        }
        else
        {
            GameEvent.MoveShapeToStartPosition();
        }
    }

    private void CheckIfAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();

        // columns
        foreach (var column in _lineIndicator.columnIndexes)
        {
            lines.Add(_lineIndicator.GetVerticleLine(column));
        }

        // rows
        for (int row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for (var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.line_data[row, index]);
            }

            lines.Add(data.ToArray());
        }

        // squares
        for (var square = 0; square < 9; square++)
        {
            List<int> data = new List<int>(9);

            for (var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.square_data[square, index]);
            }

            lines.Add(data.ToArray());
        }

        colorInTheGrid = GetAllSquareColorsInTheGrid();

        var completedLine = CheckIfSquaresAreCompleted(lines);

        if (completedLine >= 2)
        {
            //TODO: Play bonus animations
            GameEvent.ShowCongratulationWritings();
        }

        var totalScores = 10 * completedLine;
        var bonusScores = ShouldPlayColorBonusAnimation();
        GameEvent.AddScores(totalScores + bonusScores);
        GameEvent.CheckIfPlayerLost();
    }

    private int ShouldPlayColorBonusAnimation()
    {
        var colorInTheGridAfterLineRemove = GetAllSquareColorsInTheGrid();
        Config.SquareColor colorToPlayBonusFor = Config.SquareColor.NotSet;

        foreach (var color in colorInTheGrid)
        {
            if (colorInTheGridAfterLineRemove.Contains(color) == false)
            {
                colorToPlayBonusFor = color;
                Debug.Log(colorToPlayBonusFor);
            }
        }

        if (colorToPlayBonusFor == Config.SquareColor.NotSet)
        {
            Debug.Log("Cannot find color for bonus");
            return 0;
        }

        // Should never play bonus for the current color.
        if (colorToPlayBonusFor == currentActiveSquareColor)
        {
            return 0;
        }

        GameEvent.ShowBonusScreen(colorToPlayBonusFor);

        return 50;
    }

    private int CheckIfSquaresAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();

        var linesCompleted = 0;
        foreach (var line in data)
        {
            var lineCompleted = true;
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if (!comp.SquareOccupied)
                {
                    lineCompleted = false;
                }
            }

            if (lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach (var line in completedLines)
        {
            var completed = false;

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                completed = true;
            }

            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }

            if (completed)
            {
                linesCompleted++;
            }
        }

        return linesCompleted;
    }

    private void CheckIfPlayerLost()
    {
        var validShapes = 0;

        for (var index = 0; index < shapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();

            if (CheckIfShapeCanBePlacedOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                shapeStorage.shapeList[index]?.ActivateShape();
                validShapes++;
            }
        }

        if (validShapes == 0)
        {
            GameEvent.GameOver(false);
        }
    }

    private bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.currentShapeData;
        var shapeColumns = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        //All indexes of fulled up squares.
        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        for (var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < shapeColumns; columnIndex++)
            {
                if (currentShapeData.board[rowIndex].column[columnIndex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }

                squareIndex++;
            }
        }

        if (currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count)
        {
            Debug.LogError("Shape is not placed on grid properly");
        }

        var squareList = GetAllSquaresCombination(shapeColumns, shapeRows);

        bool canPlaceShape = false;

        foreach( var number in squareList )
        {
            bool shapeCanBePlacedOnTheBoard = true;
            foreach (var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOccupied)
                {
                    shapeCanBePlacedOnTheBoard = false;
                }
            }

            if (shapeCanBePlacedOnTheBoard)
            {
                canPlaceShape = true;
            }
        }

        return canPlaceShape;
    }

    private List<int[]> GetAllSquaresCombination(int columns, int rows)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while (lastRowIndex + (rows - 1) < 9)
        {
            var rowData = new List<int>();

            for (var row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for (var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                {
                    rowData.Add(_lineIndicator.line_data[row, column]);
                }
            }

            squareList.Add(rowData.ToArray());

            lastColumnIndex++;

            if (lastColumnIndex + (columns - 1) >= 9)
            {
                lastColumnIndex = 0;
                lastRowIndex++;
            }

            safeIndex++;
            if (safeIndex > 100)
            {
                Debug.LogError("Infinite loop detected");
                break;
            }
        }

        return squareList;
    }
    #endregion
}
