using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverallGameManager : MonoBehaviour
{
    public CutsceneManager cutsceneManager;

    public GameObject winloseUI;
    public TextMeshProUGUI winloseText;
    public int roundNum = 1;
    public bool gameWin = false;
    public bool roundWin = false;

    private void Start()
    {
        roundNum = 1;
        gameWin = false;
        roundWin = false;
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
