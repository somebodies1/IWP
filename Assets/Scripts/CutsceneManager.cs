using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CutsceneManager : MonoBehaviour
{
    public UIManager uiManager;

    public GameObject mainTextBox, nameTextBox;
    public TextMeshProUGUI mainTextTMP, nameTextTMP;

    private List<string> mainTextList;
    private List<string> nameTextList;
    private int textNum, maxTextNum;

    public bool CheckIfFinalCutscene()
    {
        if (textNum >= maxTextNum)
        {
            return true;
        }

        return false;
    }

    public void IntroTextInit()
    {
        mainTextList = new List<string>();
        nameTextList = new List<string>();

        CreateText("It’s gonna be a long time until we reach the OverWorld.", "Arisa");
        CreateText("(I can’t believe that there’s someone else besides me who wants to reach there too!)", "Arisa");
        CreateText("(His name is Souta and he’s also interested in reaching the OverWorld.)", "Arisa");
        CreateText("(It’s a good thing that he’s also pretty strong or else it would be tough if it was just me since there’s said to be creatures around here guarding the entrance.)", "Arisa");
        CreateText("Are you daydreaming?", "Souta");
        CreateText("Maybe…", "Arisa");
        CreateText("Hey, there’s creatures in front of us!", "Arisa");
        CreateText("That means we’re close to the OverWorld.", "Souta");
        CreateText("We should probably get rid of them then.", "Souta");

        maxTextNum = mainTextList.Count;
    }

    public void EndingTextInit()
    {
        mainTextList = new List<string>();
        nameTextList = new List<string>();

        CreateText("That should be the last of them.", "Souta");
        CreateText("Look! The entrance of the OverWorld is over there!", "Arisa");
        CreateText("I guess all that adventuring paid off.", "Souta");

        maxTextNum = mainTextList.Count;
    }

    private void CreateText(string _mainText, string _name)
    {
        mainTextList.Add(_mainText);
        nameTextList.Add(_name);
    }

    public void OnButtonNextText()
    {
        textNum++;

        if (textNum >= maxTextNum)
        {
            EndCutscene();
        }
        else
        {
            ChangeBothText(textNum);
        }
    }

    private void ChangeBothText(int _num)
    {
        mainTextTMP.text = mainTextList[_num];
        nameTextTMP.text = nameTextList[_num];
    }

    public void StartCutscene()
    {
        textNum = 0;
        ChangeBothText(textNum);
        uiManager.cutsceneEnd = false;
        uiManager.ActivateCutsceneUI();
        uiManager.DeactivateMainUI();
    }

    public void EndCutscene()
    {
        uiManager.cutsceneEnd = true;
        uiManager.DeactivateCutsceneUI();
        uiManager.ActivateMainUI();
    }
}
