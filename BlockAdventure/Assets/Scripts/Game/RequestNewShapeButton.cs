using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestNewShapeButton : MonoBehaviour
{
    public int numberOfRequests = 3;
    public TextMeshProUGUI numberText;

    private int _currentNumberOfRequests;
    [SerializeField] private Button _requestNewShapeButton;
    [SerializeField] private Button _adsButton;
    private bool _isLocked;

    private void Start()
    {
        _currentNumberOfRequests = numberOfRequests;
        _requestNewShapeButton.onClick.AddListener(RequestNewShapes);
        Unlock();
    }

    private void OnEnable()
    {
        GameEvent.GrantAdsRewards += GrantAdsRewards;
    }

    private void OnDisable()
    {
        GameEvent.GrantAdsRewards -= GrantAdsRewards;
    }

    private void RequestNewShapes()
    {
        if (!_isLocked)
        {
            _currentNumberOfRequests--;
            GameEvent.RequestNewShapes();
            GameEvent.CheckIfPlayerLost();

            if (_currentNumberOfRequests <= 0)
            {
                Lock();
            }

            numberText.text = _currentNumberOfRequests.ToString();
        }
    }

    private void Lock()
    {
        _isLocked = true;
        _requestNewShapeButton.interactable = false;
        _adsButton.gameObject.SetActive(true);
        numberText.text = _currentNumberOfRequests.ToString();
    }

    private void Unlock()
    {
        _isLocked = false;
        _requestNewShapeButton.interactable = true;
        _adsButton.gameObject.SetActive(false);
        numberText.text = _currentNumberOfRequests.ToString();
    }

    private void GrantAdsRewards()
    {
        _currentNumberOfRequests++;
        Unlock();
    }
}
