using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WristUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numbers;
    [SerializeField] private Image symbol;
    [SerializeField] private Image healthBar;
    
    [SerializeField] private PlayerHealthManager _playerHealthManager;
    [SerializeField] private WeaponMain _weaponMain;

    [SerializeField] private Sprite _bulletSymbol;
    [SerializeField] private Sprite _percentSymbol;

    private void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(_playerHealthManager.CurrentIntegrity / _playerHealthManager.maxIntegrity, 0f, 1f);
        
        if (_weaponMain.isSwap)
        {
            symbol.sprite = _percentSymbol;
            
            var fractionOfSwapTime =
                Mathf.Clamp((Time.time - _weaponMain.swapTimer) / _weaponMain.gunData.swapTime, 0f, 1f);
            var percentage = Mathf.RoundToInt(fractionOfSwapTime * 100f);
            
            numbers.text = percentage.ToString("D3");
        }
        else
        {
            symbol.sprite = _bulletSymbol;
            
            numbers.text = _weaponMain.gunData.currentAmmo.ToString("D3");
        }
    }
}