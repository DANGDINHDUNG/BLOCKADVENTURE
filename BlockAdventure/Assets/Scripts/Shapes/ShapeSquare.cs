using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hiển thị hình ảnh một ô shape.
/// </summary>
public class ShapeSquare : MonoBehaviour
{
    #region Defines
    public Image occupiedImage;
    #endregion

    #region Core MonoBehaviours
    private void Start()
    {
        occupiedImage.gameObject.SetActive(false);
    }
    #endregion

    #region Methods
    public void ActivateShape()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
    }

    public void SetOccupied()
    {
        occupiedImage.gameObject.SetActive(true);
    }

    public void UnSetOccupied()
    {
        occupiedImage.gameObject.SetActive(false);
    }
    #endregion
}
