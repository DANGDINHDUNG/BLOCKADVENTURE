using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    #region Defines
    public Image squareImage;
    public List<Sprite> squareImages;
    #endregion

    #region Core MonoBehaviours
    // Start is called before the first frame update
    void Start()
    {

    }
    #endregion

    #region Methods
    public void SetImage(bool setFirstImage)
    {
        squareImage.GetComponent<Image>().sprite = setFirstImage ? squareImages[0] : squareImages[1];
    }
    #endregion
}
