using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WristUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numbers;
    [SerializeField] private GameObject healthBar;
    
    [SerializeField] private PlayerHealthManager _playerHealthManager;
    [SerializeField] private WeaponMain _weaponMain;

    [SerializeField] private GameObject _bulletSymbol;
    [SerializeField] private GameObject _percentSymbol;
    
    private void Update()
    {
        var fillAmount = 1f;
        if (SceneManager.GetActiveScene().name != "IntroScene" && SceneManager.GetActiveScene().name != "Credits")
        {
            fillAmount = Mathf.Clamp(_playerHealthManager.CurrentIntegrity / _playerHealthManager.maxIntegrity, 0f, 1f);
        }
        
        healthBar.transform.localScale = new Vector3(1f, fillAmount, 1f);
        healthBar.transform.localPosition = new Vector3(0f, -0.00212f * (1f - fillAmount), 0f);
        
        if (_weaponMain.isSwap)
        {
            _percentSymbol.SetActive(true);
            _bulletSymbol.SetActive(false);
            
            var fractionOfSwapTime =
                Mathf.Clamp((Time.time - _weaponMain.swapTimer) / _weaponMain.gunData.swapTime, 0f, 1f);
            var percentage = Mathf.RoundToInt(fractionOfSwapTime * 100f);
            
            numbers.text = percentage.ToString("D3");
        }
        else
        {
            _bulletSymbol.SetActive(true);
            _percentSymbol.SetActive(false);
            
            numbers.text = _weaponMain.gunData.currentAmmo.ToString("D3");
        }
    }
}