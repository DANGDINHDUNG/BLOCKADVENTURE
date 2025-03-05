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
    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;

    public GameObject gridSquarePrefab;
    public Vector2 startPos = new Vector2(0, 0);

    public float squareScale = 0.5f;
    public float squareOffset = 0f;

    private Vector2 _offset = new Vector2(0, 0);
    private List<GameObject> _gridSquares = new List<GameObject>();
    #endregion

    #region Core MonoBehaviours
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void OnDisable()
    {
        GameEvent.CheckIfShapeCanbePlaced -= CheckIfShapeCanBePlaced;
    }

    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanbePlaced += CheckIfShapeCanBePlaced;
    }
    #endregion

    #region Methods
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
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(square_index % 2 == 0);

                square_index++;
            }
        }
    }


    private void CheckIfShapeCanBePlaced()
    {
        foreach (var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();


            if (gridSquare.CanUseThisSquare())
            {
                gridSquare.ActiveSquare();
            }
        }
    }
    #endregion
}
