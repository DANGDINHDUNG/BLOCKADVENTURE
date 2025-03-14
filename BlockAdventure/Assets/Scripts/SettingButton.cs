using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public Button openSettingsButton;
    public Button closeSettingsButton;

    public void SettingsOpened()
    {
        openSettingsButton.gameObject.SetActive(false);
        closeSettingsButton.gameObject.SetActive(true);
        closeSettingsButton.interactable = true;
    }

    public void SettingsClosed()
    {
        openSettingsButton.gameObject.SetActive(true);
        closeSettingsButton.gameObject.SetActive(false);
        openSettingsButton.interactable = true;
    }
}
