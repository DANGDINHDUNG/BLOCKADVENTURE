using System;
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
        GameEvent.CheckIfPlayerDefeated -= CheckIfPlayerDefeated;
    }

    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanbePlaced += CheckIfShapeCanBePlaced;
        GameEvent.UpdateSquareColor += OnUpdateSquareColor;
        GameEvent.CheckIfPlayerLost += CheckIfPlayerLost;
        GameEvent.CheckIfPlayerDefeated += CheckIfPlayerDefeated;
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
    }

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

            AudioManager.instance.Play("Put_Block");

            if (shapeLeft == 0)
            {
                GameEvent.RequestNewShapes();

                // Khi người chơi đã đặt hết shape hiện có, sẽ tới lượt hành động của quái.
                GameEvent.OnEnemyTurn();
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

        var totalDamages = 10 * completedLine;
        var bonusDamages = ShouldPlayColorBonusAnimation();
        //GameEvent.AddScores(totalDamages + bonusDamages);

        // Tính lượng damage vào máu quái
        GameEvent.EnemyTakeDamage(totalDamages + bonusDamages);
        GameEvent.CheckIfPlayerLost();
        CheckIfEnemyDefeated();
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

            AudioManager.instance.Play("Break_Block");
        }

        return linesCompleted;
    }

    // Kiểm tra nếu quái đã mất hết máu, tạo quái mới và tăng stage.
    private void CheckIfEnemyDefeated()
    {
        if (BattleManager.battleInstance.enemy.currentHealthPoint <= 0)
        {
            BattleManager.battleInstance.RequestNewEnemy();
            BattleManager.battleInstance.stage++;
            BattleManager.battleInstance.SetStage();

            foreach (var square in _gridSquares)
            {
                square.GetComponent<GridSquare>().ClearOccupied();
                square.GetComponent<GridSquare>().Deactivate();
            }
        }
    }

    // Kiểm tra xem người chơi đã mất hết không gian trống để đặt shape mới.
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
            //GameEvent.GameOver(false);
            // Khi người chơi không thể đặt thêm mảnh, sẽ reset lại grid và mất một lượng lớn máu.
            foreach (var square in _gridSquares)
            {
                square.GetComponent<GridSquare>().ClearOccupied();
                square.GetComponent<GridSquare>().Deactivate();
            }

            GameEvent.PlayerTakeDamage(100);
            GameEvent.CheckIfPlayerDefeated();

            Debug.Log("Out of empty space");
        }
    }

    // Khi người chơi mất hết máu, kết thúc game.
    private void CheckIfPlayerDefeated()
    {
        if (BattleManager.battleInstance.player.currentHealthPoint <= 0)
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
