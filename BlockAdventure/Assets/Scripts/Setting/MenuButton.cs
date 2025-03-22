using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    #region Defines
    #endregion

    #region Core MonoBehaviours
    private void Awake()
    {
        if (Application.isEditor == false)
            Debug.unityLogger.logEnabled = false;
    }
    #endregion

    #region Methods
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    #endregion
}
