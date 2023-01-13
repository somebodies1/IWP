using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverallGameManager : MonoBehaviour
{
    public GameObject winloseUI;
    public TextMeshProUGUI winloseText;

    public void ActivateFinishScreen()
    {
        winloseUI.SetActive(true);
    }

    public void DisableFinishScreen()
    {
        winloseUI.SetActive(false);
    }

    public void SetWinScreen()
    {
        winloseText.text = "You Win";
    }

    public void SetLoseScreen()
    {
        winloseText.text = "You Lose";
    }
}
