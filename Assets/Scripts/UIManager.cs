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
    public List<GameObject> skillButtonList;

    public GameObject limitBreakButton;
    public List<GameObject> limitBreakButtonList;

    //Prefabs
    public GameObject emptyGOUIPrefab;
    public GameObject skillButton;

    public void ActivateActionUI()
    {
        overallActionUI.SetActive(true);
        Debug.Log("ActivateActionUI");
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

    public void OnButtonSkillSwitch()
    {
        SwitchCurrentPlayersSkillUI();
    }

    public void SwitchCurrentPlayersSkillUI()
    {
        List<Skill> currentPlayerSkillList = battleManager.currentCharacterTurn.GetComponent<BaseEntity>().skillList;

        //Disable skill buttons
        for (int i = 0; i < skillButtonList.Count; ++i)
        {
            skillButtonList[i].SetActive(false);
        }

        //Enable limit break buttons that player has
        for (int i = 0; i < currentPlayerSkillList.Count; ++i)
        {
            skillButtonList[i].SetActive(true);
            SetSkillButton(skillButtonList[i], currentPlayerSkillList[i]);
        }
    }

    public void SetSkillButton(GameObject _button, Skill _skill)
    {
        Button skillButton = _button.GetComponent<Button>();

        skillButton.onClick.RemoveAllListeners();
        skillButton.GetComponent<Skill>().OverwriteSkill(_skill);

        skillButton.GetComponent<Button>().onClick.AddListener(delegate { battleManager.OnButtonPlayerSkill(skillButton.GetComponent<Skill>()); });
        skillButton.GetComponent<Button>().onClick.AddListener(delegate { SwitchAttackUI(targetSelectionUI); });
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

    public void DisableAllEnemyStatsUI()
    {
        for (int i = 0; i < enemyStatsUIList.Count; ++i)
        {
            enemyStatsUIList[i].SetActive(false);
        }
    }

    public void SetEntityStatsUI(GameObject _statsGO, BaseEntity _char)
    {
        GameObject EntityStats = _statsGO;

        _char.entityStatsUI = EntityStats;

        float currentToMaxHealthRatio = (float)_char.CurrentHP / (float)_char.MaxHP;
        float currentToMaxLBRatio = (float)_char.CurrentLB / (float)_char.MaxLB;
        EntityStats.GetComponent<EntityStatsUI>().UpdateAllEntityStatsUIValues(_char.entityName, currentToMaxHealthRatio, currentToMaxLBRatio, _char.fullGuardAmt);
    }

    public void OnButtonLimitBreakSwitch()
    {
        SwitchCurrentPlayersLimitBreakUI();
    }

    public void SwitchCurrentPlayersLimitBreakUI()
    {
        List<Skill> currentPlayerLBList = new List<Skill>();
        List<LimitBreak> LBList = battleManager.currentCharacterTurn.GetComponent<BaseEntity>().lbList;

        for (int i = 0; i < LBList.Count; ++i)
        {
            currentPlayerLBList.Add(LBList[i].lbSkill);
        }

        //Disable limit break buttons
        for (int i = 0; i < limitBreakButtonList.Count; ++i)
        {
            limitBreakButtonList[i].SetActive(false);
        }

        //Enable limit break buttons that player has
        for (int i = 0; i < currentPlayerLBList.Count; ++i)
        {
            limitBreakButtonList[i].SetActive(true);
            SetLimitBreakButton(limitBreakButtonList[i], currentPlayerLBList[i], i);
        }
    }

    public void SetLimitBreakButton(GameObject _button, Skill _lb, int _lbNum)
    {
        Button lbButton = _button.GetComponent<Button>();

        lbButton.onClick.RemoveAllListeners();

        lbButton.onClick.AddListener(delegate { battleManager.OnButtonPlayerLimitBreak(_lb, _lbNum); });
        lbButton.onClick.AddListener(delegate { SwitchAttackUI(targetSelectionUI); });
    }
}
