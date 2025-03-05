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
    // Start is called before the first frame update
    void Start()
    {
        foreach (var shape in shapeList)
        {
            var shapeIndex = Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }
    #endregion
}
