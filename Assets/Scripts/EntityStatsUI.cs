using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityStatsUI : MonoBehaviour
{
    //Name
    public TextMeshProUGUI entityNameTMP;

    //Bars
    public GameObject healthBarUI;
    public GameObject limitBreakBarUI;

    //Full guard
    public TextMeshProUGUI fullGuardTMP;

    public void UpdateChangingValues(float _healthRatio, float _lbRatio, int _fullGuardAmt)
    {
        healthBarUI.transform.localScale = new Vector3(_healthRatio, 1, 1);
        limitBreakBarUI.transform.localScale = new Vector3(_lbRatio, 1, 1);
        fullGuardTMP.text = _fullGuardAmt.ToString();
    }

    public void UpdateAllEntityStatsUIValues(string _entityName, float _healthRatio, float _lbRatio, int _fullGuardAmt)
    {
        entityNameTMP.text = _entityName;
        healthBarUI.transform.localScale = new Vector3(_healthRatio, 1, 1);
        limitBreakBarUI.transform.localScale = new Vector3(_lbRatio, 1, 1);
        fullGuardTMP.text = _fullGuardAmt.ToString();
    }
}
