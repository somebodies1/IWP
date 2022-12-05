using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public BattleManager battleManager;

    public List<GameObject> attackUIList;
    public List<GameObject> playerSkillsList;

    public GameObject overallActionUI;
    public GameObject skillsUI;
    public GameObject targetSelectionUI;

    public GameObject switchSkillsButton;

    //Prefabs
    public GameObject emptyGOUIPrefab;
    public GameObject skillButton;

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

    public void SwitchCurrentPlayerSkillsUIByName(string _name)
    {
        for (int i = 0; i < playerSkillsList.Count; ++i)
        {
            playerSkillsList[i].SetActive(false);

            if (playerSkillsList[i].name == _name)
            {
                playerSkillsList[i].SetActive(true);
            }
        }
    }

    public void SpawnAllSkillButtons(List<GameObject> _playerCharList)
    {
        for (int i = 0; i < _playerCharList.Count; ++i)
        {
            List<Skill> SkillList = _playerCharList[i].GetComponent<BaseEntity>().skillList;
            string playerName = _playerCharList[i].GetComponent<BaseEntity>().entityName;

            var emptyGOPrefab = Instantiate(emptyGOUIPrefab, skillsUI.transform);
            emptyGOPrefab.name = (playerName);

            playerSkillsList.Add(emptyGOPrefab);

            float buttonXPos = -450f;

            for (int j = 0; j < SkillList.Count; ++j)
            {
                SpawnSkillButton(emptyGOPrefab.transform, new Vector3(buttonXPos + (j * 150), 0, 0), SkillList[j]);
            }
        }
    }

    public void SpawnSkillButton(Transform _parent, Vector3 _buttonPos, Skill _skill)
    {
        //var skillButton = Instantiate(_skill, new Vector3(0, 0, 0), Quaternion.identity, _parent);
        GameObject SkillButton = Instantiate(skillButton, new Vector3(0, 0, 0), Quaternion.identity, _parent);
        SkillButton.transform.localPosition = _buttonPos;

        SkillButton.AddComponent<Skill>();
        SkillButton.GetComponent<Skill>().OverwriteSkill(_skill);

        SkillButton.GetComponent<Button>().onClick.AddListener(delegate { battleManager.OnButtonPlayerSkill(SkillButton.GetComponent<Skill>()); });
        SkillButton.GetComponent<Button>().onClick.AddListener(delegate { SwitchAttackUI(targetSelectionUI); });
    }

    public void OnButtonSkillSwitch()
    {
        SwitchCurrentPlayerSkillsUIByName(battleManager.currentCharacterTurn.GetComponent<BaseEntity>().entityName);
    }
}
