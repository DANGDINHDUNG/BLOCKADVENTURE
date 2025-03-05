using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class này quản lý từng ô grid, hiển thị hình ảnh tương ứng với trạng thái grid (trống, đã chọn, đã đặt block).
/// </summary>
public class GridSquare : MonoBehaviour
{
    #region Defines
    public Image hoverImage;
    public Image activeImage;
    public Image squareImage;
    public List<Sprite> squareImages;
    #endregion

    #region Properties
    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }    
    #endregion

    #region Core MonoBehaviours
    // Start is called before the first frame update
    void Start()
    {
        Selected = false;
        SquareOccupied = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hoverImage.gameObject.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hoverImage.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hoverImage.gameObject.SetActive(false);
    }
    #endregion

    #region Methods
    public void SetImage(bool setFirstImage)
    {
        squareImage.GetComponent<Image>().sprite = setFirstImage ? squareImages[0] : squareImages[1];
    }
    #endregion

    #region Methods
    public bool CanUseThisSquare()
    {
        return hoverImage.gameObject.activeSelf;
    }

    public void ActiveSquare()
    {
        hoverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = true;
        SquareOccupied = true;
    }
    #endregion
}
