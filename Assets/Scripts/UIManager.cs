using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public BattleManager battleManager;

    public List<GameObject> attackUIList;
    public List<GameObject> playerSkillsList;

    //Stats UI
    public GameObject playerStatsUI;
    public List<GameObject> playerStatsUIList;

    public GameObject enemyStatsUI;
    public List<GameObject> enemyStatsUIList;

    //Action UI
    public GameObject overallActionUI;

    public GameObject skillsUI;

    public GameObject targetSelectionUI;
    public List<GameObject> targetButtonsList;

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
        GameObject SkillButton = Instantiate(skillButton, new Vector3(0, 0, 0), Quaternion.identity, _parent);
        SkillButton.transform.localPosition = _buttonPos;

        SkillButton.AddComponent<Skill>();
        SkillButton.GetComponent<Skill>().OverwriteSkill(_skill);

        SkillButton.GetComponent<Button>().onClick.AddListener(delegate { battleManager.OnButtonPlayerSkill(SkillButton.GetComponent<Skill>()); });
        SkillButton.GetComponent<Button>().onClick.AddListener(delegate { SwitchAttackUI(targetSelectionUI); });
    }

    public void SetAllTargetButtons(List<GameObject> _enemiesList)
    {
        for (int i = 0; i < targetButtonsList.Count; ++i)
        {
            ResetTargetButton(targetButtonsList[i]);
            targetButtonsList[i].SetActive(false);
        }

        for (int i = 0; i < _enemiesList.Count; ++i)
        {
            targetButtonsList[i].SetActive(true);
            SetTargetButton(targetButtonsList[i], _enemiesList[i]);
        }
    }

    public void ResetTargetButton(GameObject _buttonGO)
    {
        GameObject TargetButton = _buttonGO;

        TargetButton.GetComponentInChildren<TextMeshProUGUI>().text = " ";
        TargetButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void SetTargetButton(GameObject _buttonGO, GameObject _targetGO)
    {
        GameObject TargetButton = _buttonGO;
        TargetButton.GetComponentInChildren<TextMeshProUGUI>().text = _targetGO.GetComponent<BaseEntity>().entityName;

        //Change targets
        TargetButton.GetComponent<Button>().onClick.AddListener(delegate { battleManager.OnButtonPlayerTarget(_targetGO); });
    }

    public void OnButtonSkillSwitch()
    {
        SwitchCurrentPlayerSkillsUIByName(battleManager.currentCharacterTurn.GetComponent<BaseEntity>().entityName);
    }

    public void SetAllEntityStatsUI(List<GameObject> _charList, bool _isPlayerChar)
    {
        List<GameObject> statsUIList;

        if (_isPlayerChar)
            statsUIList = playerStatsUIList;
        else
            statsUIList = enemyStatsUIList;

        for (int i = 0; i < statsUIList.Count; ++i)
        {
            statsUIList[i].SetActive(false);
        }

        for (int i = 0; i < _charList.Count; ++i)
        {
            statsUIList[i].SetActive(true);
            SetEntityStatsUI(statsUIList[i], _charList[i].GetComponent<BaseEntity>());
        }
    }

    public void SetEntityStatsUI(GameObject _statsGO, BaseEntity _char)
    {
        GameObject EntityStats = _statsGO;

        _char.entityStatsUI = EntityStats;
        EntityStats.GetComponent<EntityStatsUI>().UpdateAllEntityStatsUIValues(_char.entityName, 1, 0, _char.fullGuardAmt);
    }
}
