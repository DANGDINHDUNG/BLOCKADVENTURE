using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public bool isPlayBGM;
    public bool isPlaySFX;

    public static SettingManager settingInstance;

    private void Awake()
    {
        if (settingInstance == null)
        {
            settingInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
