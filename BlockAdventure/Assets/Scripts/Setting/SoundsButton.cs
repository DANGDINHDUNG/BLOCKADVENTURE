using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsButton : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string soundType;

    private bool _isMuted;

    private void Start()
    {
        if (soundType == "SFX")
        {
            _isMuted = SettingManager.settingInstance.isPlaySFX;
        }
        else if (soundType == "BGM")
        {
            _isMuted = SettingManager.settingInstance.isPlayBGM;
        }
        UpdateSetting(soundType, _isMuted);Debug.Log("SoundType: " + soundType + " isMuted: " + _isMuted);
    }

    public void PlaySound()
    {
        if (!_isMuted)
        {
            UpdateSetting(soundType, true);
        }
        else
        {
            UpdateSetting(soundType, false);
        }
    }

    private void UpdateSetting(string type, bool flag)
    {
        _isMuted = flag;
        if (soundType == "SFX")
        {
            SettingManager.settingInstance.isPlaySFX = flag;
            if (flag)
            {
                _animator.SetTrigger("OpenBGM");
            }
            else
            {
                _animator.SetTrigger("CloseBGM");
            }
        }
        else if (soundType == "BGM")
        {
            SettingManager.settingInstance.isPlayBGM = flag;
            if (flag)
            {
                _animator.SetTrigger("OpenBGM");
                AudioManager.instance.Play("BGM");
                Debug.Log("Play BGM");
            }
            else
            {
                _animator.SetTrigger("CloseBGM");
                AudioManager.instance.Stop("BGM");
                Debug.Log("Stop BGM");
            }
        }
    }
}
