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

    // Start is called before the first frame update
    void Start()
    {
        uiManager.DeactivateMainUI();
        textNum = 0;

        TextInit();
        ChangeBothText(textNum);
        
    }

    private void TextInit()
    {
        mainTextList = new List<string>();
        nameTextList = new List<string>();

        CreateText("Hey", "Arisa");
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

    private void DeactivateAllCutsceneGOs()
    {
        mainTextBox.SetActive(false);
        nameTextBox.SetActive(false);
    }

    public void EndCutscene()
    {
        uiManager.cutsceneEnd = true;
        DeactivateAllCutsceneGOs();
        uiManager.ActivateMainUI();
    }
}
