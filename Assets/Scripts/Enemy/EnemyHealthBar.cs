using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private Image _shieldImage;
    [SerializeField] private Image _armourImage;
    [SerializeField] private Image _integrityImage;
    
    private float _maxShield;
    private float _maxArmour;
    private float _maxIntegrity;
    
    public void SetMaxValues(float maxShield, float maxArmour, float maxIntegrity)
    {
        _maxShield = maxShield;
        _maxArmour = maxArmour;
        _maxIntegrity = maxIntegrity;
    }

    private void Update()
    {
        transform.LookAt(_playerData.position);
    }

    public void UpdateHealthBar(float currentShield, float currentArmour, float currentIntegrity)
    {
        var maxTotal = _maxShield + _maxArmour + _maxIntegrity;
        var integrityFractionOfHealthBar = currentIntegrity / maxTotal;
        var armourFractionOfHealthBar = currentArmour / maxTotal;
        var shieldFractionOfHealthBar = currentShield / maxTotal;

        _integrityImage.fillAmount = integrityFractionOfHealthBar;

        _armourImage.transform.localPosition =
            new Vector3(integrityFractionOfHealthBar, 0f, 0f);
        _armourImage.fillAmount = armourFractionOfHealthBar;

        _shieldImage.transform.localPosition =
            new Vector3(integrityFractionOfHealthBar + armourFractionOfHealthBar, 0f, 0f);
        _shieldImage.fillAmount = shieldFractionOfHealthBar;
    }
}
