using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> attackUIList;

    public void SwitchAttackUI(GameObject _uiToSwitch)
    {
        for (int i = 0; i < attackUIList.Count; ++i)
        {
            attackUIList[i].SetActive(false);

            if (attackUIList[i] == _uiToSwitch)
            {
                _uiToSwitch.SetActive(true);
            }
        }
    }
}
