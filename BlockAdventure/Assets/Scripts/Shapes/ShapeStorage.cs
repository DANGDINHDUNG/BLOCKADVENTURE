using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lưu trữ dữ liệu của các loại Shape và tạo ngẫu nhiên ra màn hình.
/// </summary>
public class ShapeStorage : MonoBehaviour
{
    #region Defines
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;
    #endregion

    #region Core MonoBehaviours
    void Start()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }

    private void OnEnable()
    {
        GameEvent.RequestNewShapes += RegenerateNewShape;
    }

    private void OnDisable()
    {
        GameEvent.RequestNewShapes -= RegenerateNewShape;
    }
    #endregion

    #region Methods
    public Shape GetCurrentSelectedShape()
    {
        foreach (var shape in shapeList)
        {
            // Lấy dữ liệu của shape đang được di chuyển và chưa biến mất
            if (shape.IsOnStartPosition() == false && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }
        Debug.LogError("There is no shape selected");
        return null;
    }

    public void RegenerateNewShape()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }
    #endregion
}
