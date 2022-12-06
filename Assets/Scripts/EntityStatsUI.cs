using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityStatsUI : MonoBehaviour
{
    //Name
    public TextMeshProUGUI entityNameTMP;

    //Healthbar
    public GameObject healthBarUI;

    //Full guard
    public TextMeshProUGUI fullGuardTMP;

    public void UpdateChangingValues(float _healthRatio, int _fullGuardAmt)
    {
        healthBarUI.transform.localScale = new Vector3(_healthRatio, 1, 1);
        fullGuardTMP.text = _fullGuardAmt.ToString();
    }

    public void UpdateAllEntityStatsUIValues(string _entityName, float _healthRatio, int _fullGuardAmt)
    {
        entityNameTMP.text = _entityName;
        healthBarUI.transform.localScale = new Vector3(_healthRatio, 1, 1);
        fullGuardTMP.text = _fullGuardAmt.ToString();
    }
}
