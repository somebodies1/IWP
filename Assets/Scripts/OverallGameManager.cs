using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverallGameManager : MonoBehaviour
{
    public CutsceneManager cutsceneManager;

    public GameObject winloseUI;
    public TextMeshProUGUI winloseText;
    public int roundNum;
    public bool gameWin;

    private void Start()
    {
        roundNum = 1;
        gameWin = false;
    }

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

    public void OnButtonWinScreen()
    {
        if (gameWin && cutsceneManager.CheckIfFinalCutscene())
        {
            ActivateFinishScreen();
            SetWinScreen();
        }
    }

    public void StartIntroCutscene()
    {
        cutsceneManager.IntroTextInit();
        cutsceneManager.StartCutscene();
    }

    public void StartEndCutscene()
    {
        cutsceneManager.EndingTextInit();
        cutsceneManager.StartCutscene();
    }
}
