using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> attackUIList;
    public GameObject overallActionUI;

    public void ActivateActionUI()
    {
        overallActionUI.SetActive(true);
    }

    public void DeactivateActionUI()
    {
        overallActionUI.SetActive(false);
    }

    public void SwitchAttackUIByName(string _name)
    {
        for (int i = 0; i < attackUIList.Count; ++i)
        {
            attackUIList[i].SetActive(false);

            if (attackUIList[i].name == _name)
            {
                attackUIList[i].SetActive(true);
            }
        }
    }

    public void SwitchAttackUI(GameObject _uiToSwitch)
    {
        for (int i = 0; i < attackUIList.Count; ++i)
        {
            attackUIList[i].SetActive(false);

            if (attackUIList[i] == _uiToSwitch)
            {
                attackUIList[i].SetActive(true);
            }
        }
    }
}
