using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public UIManager uiManager;
    public CameraManager camManager;
    public OverallGameManager gameManager;

    public EntitySpawner entitySpawner;

    public List<GameObject> playerCharList;
    public List<GameObject> enemiesList;

    public GameObject currentCharacterTurn;

    //Turns always start with player
    private void Start()
    {
        entitySpawner.InitSpawnLocations();
        entitySpawner.SpawnAllPlayerChar();
        entitySpawner.SpawnEnemyRound(1);

        //Add GOs to lists
        playerCharList.AddRange(GameObject.FindGameObjectsWithTag("PlayerChar"));
        enemiesList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        //Generate UI stuff
        //uiManager.SpawnAllSkillButtons(playerCharList);

        uiManager.SetAllEntityStatsUI(playerCharList, true);
        uiManager.SetAllEntityStatsUI(enemiesList, false);

        uiManager.SetAllTargetButtons(enemiesList);

        //Test to kill enemies
        //DeleteEnemy(enemiesList[1]);
        //camManager.CameraMovement(new Vector3(1.5f, 1, -7));

        //Player's turn first
        SwitchToPlayerTurn();

        gameManager.StartIntroCutscene();
    }

    private void WinGame()
    {
        //gameManager.ActivateFinishScreen();
        //gameManager.SetWinScreen();

        gameManager.gameWin = true;
        gameManager.StartEndCutscene();
    }

    IEnumerator WinRound()
    {
        gameManager.roundNum++;
        //Wait for round to end
        yield return new WaitForSeconds(1.0f);

        if (gameManager.roundNum > 5)
        {
            WinGame();
        }
        else
        {
            for (int i = 0; i < playerCharList.Count; ++i)
            {
                BaseEntity p = playerCharList[i].GetComponent<BaseEntity>();
                p.CurrentHP = p.MaxHP;
                p.UpdateStats();
            }

            uiManager.ActivateActionUI();

            DeleteDeadEnemies();
            //entitySpawner.SpawnEnemies();
            entitySpawner.SpawnEnemyRound(gameManager.roundNum);

            enemiesList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            uiManager.SetAllEntityStatsUI(enemiesList, false);
            uiManager.SetAllTargetButtons(enemiesList);
        }
    }

    private void LoseGame()
    {
        gameManager.ActivateFinishScreen();
        gameManager.SetLoseScreen();
    }

    private bool CheckIfAllPlayerCharDead()
    {
        for (int i = 0; i < playerCharList.Count; ++i)
        {
            if (playerCharList[i].GetComponent<BaseEntity>().isDead != true)
                return false;
        }

        return true;
    }

    private bool CheckIfAllEnemiesDead()
    {
        for (int i = 0; i < enemiesList.Count; ++i)
        {
            if (enemiesList[i].GetComponent<BaseEntity>().isDead != true)
                return false;
        }

        return true;
    }

    private void AddPlayer()
    {

    }

    private void AddEnemy()
    {

    }

    private void DeletePlayer(GameObject _playerCharToDelete)
    {
        //Delete GO
        playerCharList.Remove(_playerCharToDelete);
        Destroy(_playerCharToDelete);
    }

    private void DeleteEnemy(GameObject _enemyToDelete)
    {
        //Delete GO
        enemiesList.Remove(_enemyToDelete);
        Destroy(_enemyToDelete);

        uiManager.SetAllTargetButtons(enemiesList);
        uiManager.SetAllEntityStatsUI(enemiesList, false);
    }

    private void DeleteDeadEnemies()
    {
        List<GameObject> enemiesToDelList = new List<GameObject>();

        //Put all dead enemies into temp list
        for (int i = 0; i < enemiesList.Count; ++i)
        {
            if (enemiesList[i].GetComponent<BaseEntity>().isDead == true)
            {
                enemiesToDelList.Add(enemiesList[i]);
            }
        }

        //Delete GO
        for (int i = 0; i < enemiesToDelList.Count; ++i)
        {
            enemiesList.Remove(enemiesToDelList[i]);
            Destroy(enemiesToDelList[i]);
        }

        //if (enemiesToDelList.Count == 0)
        //{
        //    enemiesList = new List<GameObject>();
        //}

        //Update stats UI
        uiManager.SetAllTargetButtons(enemiesList);
        uiManager.SetAllEntityStatsUI(enemiesList, false);
    }

    //Switch to player's turn
    private void SwitchToPlayerTurn()
    {
        if (!CheckIfAllEnemiesDead() && uiManager.cutsceneEnd)
            uiManager.ActivateActionUI();

        for (int i = 0; i < playerCharList.Count; ++i)
        {
            playerCharList[i].GetComponent<PlayerFSM>().SetCurrentState(PlayerFSM.TURN_STATE.SELECTING);
        }
        //playerCharList[0].GetComponent<PlayerFSM>().SetCurrentState(PlayerFSM.TURN_STATE.SELECTING);
        currentCharacterTurn = playerCharList[0];

        BaseEntity currentCharacterBaseEntity = currentCharacterTurn.GetComponent<BaseEntity>();

        //Ends guarding animation
        if (currentCharacterBaseEntity.CheckIfCurrentActionGuard())
        {
            currentCharacterBaseEntity.BaseEntityAnimation(BaseEntity.ANIMATION.END_GUARD);
        }

        //Enables limit break button
        if (currentCharacterBaseEntity.CheckIfLimitBreakFull())
        {
            uiManager.limitBreakButton.SetActive(true);
        }
        else
        {
            uiManager.limitBreakButton.SetActive(false);
        }

        if (CheckIfTeamAttackAvailable())
        {
            uiManager.teamAttackButton.SetActive(true);
        }
        else
        {
            uiManager.teamAttackButton.SetActive(false);
        }

        //Passes turn to next player if dead
        if (currentCharacterBaseEntity.isDead)
        {
            NextPlayerTurn();
        }
    }

    //When switching from player char to player char
    public void NextPlayerTurn()
    {
        if(CheckIfAllEnemiesDead())
        {
            Debug.Log("DeactivateActionUI");
            uiManager.DeactivateActionUI();
            StartCoroutine(WinRound());
        }

        CurrentTurnCheck();

        currentCharacterTurn.GetComponent<PlayerFSM>().SetCurrentState(PlayerFSM.TURN_STATE.TURN_ENDED);

        int currentPlayerCharIndex = playerCharList.IndexOf(currentCharacterTurn);

        Debug.Log("PlayCharIndex: " + currentPlayerCharIndex + " CharListIndex: " + playerCharList.Count);

        if (currentPlayerCharIndex >=  playerCharList.Count - 1)
        {
            //Switch to enemies' turn
            if (CheckIfAllEnemiesDead())
            {
                SwitchToPlayerTurn();
            }
            else
            {
                SwitchToEnemyTurn();
            }
        }
        else
        {
            //Switch to next player's character's turn
            //playerCharList[currentPlayerCharIndex + 1].GetComponent<PlayerFSM>().SetCurrentState(PlayerFSM.TURN_STATE.SELECTING);
            currentCharacterTurn = playerCharList[currentPlayerCharIndex + 1];

            BaseEntity currentCharacterBaseEntity = currentCharacterTurn.GetComponent<BaseEntity>();

            //Ends guarding animation
            if (currentCharacterBaseEntity.CheckIfCurrentActionGuard())
            {
                currentCharacterBaseEntity.BaseEntityAnimation(BaseEntity.ANIMATION.END_GUARD);
            }

            //Enables limit break button
            if (currentCharacterBaseEntity.CheckIfLimitBreakFull())
            {
                uiManager.limitBreakButton.SetActive(true);
            }
            else
            {
                uiManager.limitBreakButton.SetActive(false);
            }

            if (CheckIfTeamAttackAvailable())
            {
                uiManager.teamAttackButton.SetActive(true);
            }
            else
            {
                uiManager.teamAttackButton.SetActive(false);
            }

            //Passes turn to next player if dead or used team attack
            if (currentCharacterBaseEntity.isDead ||
                currentCharacterTurn.GetComponent<PlayerFSM>().currentState == PlayerFSM.TURN_STATE.TURN_ENDED)
            {
                NextPlayerTurn();
            }
            else
            {
                currentCharacterTurn.GetComponent<PlayerFSM>().SetCurrentState(PlayerFSM.TURN_STATE.SELECTING);
            }
        }
    }

    //Switch to enemy's turn
    private void SwitchToEnemyTurn()
    {
        uiManager.DeactivateActionUI();
        enemiesList[0].GetComponent<EnemyFSM>().SetCurrentState(EnemyFSM.TURN_STATE.WAITING);
        currentCharacterTurn = enemiesList[0];

        NextEnemyTurn();
    }

    //When switching from enemy to enemy
    public void NextEnemyTurn()
    {
        CurrentTurnCheck();

        //Choose which player char to target
        EnemyFSM currentCharacterTurnEnemyFSM = currentCharacterTurn.GetComponent<EnemyFSM>();
        currentCharacterTurnEnemyFSM.EnemyAITargeting(playerCharList);

        EnemyAttack(currentCharacterTurnEnemyFSM.targetGO);
    }

    private void NextEnemyTurnConditions()
    {
        if (CheckIfAllPlayerCharDead())
        {
            LoseGame();
            return;
        }
            
        currentCharacterTurn.GetComponent<EnemyFSM>().SetCurrentState(EnemyFSM.TURN_STATE.TURN_ENDED);

        int currentEnemyIndex = enemiesList.IndexOf(currentCharacterTurn);

        if (currentEnemyIndex >= enemiesList.Count - 1)
        {
            //Switch to player's turn
            SwitchToPlayerTurn();
        }
        else
        {
            //Switch to next enemy's turn
            enemiesList[currentEnemyIndex + 1].GetComponent<EnemyFSM>().SetCurrentState(EnemyFSM.TURN_STATE.WAITING);
            currentCharacterTurn = enemiesList[currentEnemyIndex + 1];

            NextEnemyTurn();
        }
    }

    public void OnButtonPlayerAction(int _actionNum)
    {
        currentCharacterTurn.GetComponent<BaseEntity>().currentAction = (BaseEntity.ACTION)_actionNum;
    }

    public void OnButtonPlayerSkill(Skill _skill)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();
        currentCharacterGO.currentSkill = _skill;
    }

    public void OnButtonPlayerLimitBreak(Skill _skill, int _lbNum)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();
        currentCharacterGO.currentSkill = _skill;
        currentCharacterGO.lbSkillNum = _lbNum;
    }

    public void OnButtonPlayerGuard()
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();

        //Check if using full guard
        if (currentCharacterGO.currentAction == BaseEntity.ACTION.FULL_GUARD)
        {
            //Check if conditions are met
            if (!currentCharacterGO.CheckIfEntityCanFullGuard())
            {
                //Disallows full guard when conditions are not met
                return;
            }
        }
        
        BaseEntity.ANIMATION currentCharacterAnimation = currentCharacterGO.CurrentActionToAnimation(currentCharacterGO.currentAction);

        PlayerGuard(currentCharacterAnimation);
    }

    void PlayerGuard(BaseEntity.ANIMATION _animation)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();

        if (currentCharacterGO.BaseEntityAnimation(_animation))
        {
            uiManager.DeactivateActionUI();
            StartCoroutine(WaitForPlayerAnimation(currentCharacterGO.gameObject, _animation));
        }
        else
        {
            NextPlayerTurn();
        }

        uiManager.SwitchAttackUIByName("ActionUI");
    }

    public void OnButtonPlayerTarget(GameObject _targetGO)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();

        //Check if using limit break
        if (currentCharacterGO.currentAction == BaseEntity.ACTION.LIMIT_BREAK)
        {
            //Use character's limit break meter
            currentCharacterGO.UseLimitBreak();
        }
            
        BaseEntity.ANIMATION currentCharacterAnimation = currentCharacterGO.CurrentActionToAnimation(currentCharacterGO.currentAction);

        PlayerTarget(_targetGO.GetComponent<BaseEntity>(), currentCharacterAnimation);
    }

    void PlayerTarget(BaseEntity _targetGO, BaseEntity.ANIMATION _animation)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();
        
        //Ends next player char's turn if using team attack
        if (currentCharacterGO.currentAction == BaseEntity.ACTION.TEAM_ATTACK)
        {
            int currentPlayerCharIndex = playerCharList.IndexOf(currentCharacterTurn);
            playerCharList[currentPlayerCharIndex + 1].GetComponent<PlayerFSM>().currentState = PlayerFSM.TURN_STATE.TURN_ENDED;
        }

        if (currentCharacterGO.BaseEntityAnimation(_animation))
        {
            uiManager.DeactivateActionUI();
            //if (_animation == BaseEntity.ANIMATION.LIMIT_BREAK)
            //    StartCoroutine(currentCharacterGO.LimitBreakAnimation(camManager.mainCamera));
            
            StartCoroutine(WaitForPlayerAnimation(_targetGO.gameObject, _animation));
        }
        else
        {
            BaseEntity targetGO = _targetGO.GetComponent<BaseEntity>();
            currentCharacterGO.CalculateDamage(targetGO);

            NextPlayerTurn();
        }

        uiManager.SwitchAttackUIByName("ActionUI");
    }

    public void EnemyAttack(GameObject _targetGO)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();

        if (currentCharacterGO.BaseEntityAnimation(BaseEntity.ANIMATION.ATTACK))
        {
            StartCoroutine(WaitForEnemyAnimation(_targetGO));
        }
        else
        {
            BaseEntity targetGO = _targetGO.GetComponent<BaseEntity>();
            currentCharacterGO.CalculateDamage(targetGO);
            NextEnemyTurnConditions();
        }
    }

    private void CurrentTurnCheck()
    {
        Debug.Log("Current Turn: " + currentCharacterTurn);
    }

    IEnumerator WaitForPlayerAnimation(GameObject _targetGO, BaseEntity.ANIMATION _animation)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();
        BaseEntity targetGO = _targetGO.GetComponent<BaseEntity>();

        Vector3 oldCCPos = currentCharacterTurn.transform.position;
        
        //Wait for animation to end
        switch (_animation)
        {
            case BaseEntity.ANIMATION.ATTACK:
                if (currentCharacterGO.currentAction == BaseEntity.ACTION.TEAM_ATTACK)
                {
                    List<Vector3> oldPosList = new List<Vector3>();
                    for (int i = 0; i < playerCharList.Count; ++i)
                    {
                        oldPosList.Add(playerCharList[i].transform.position);
                        playerCharList[i].transform.position = new Vector3(_targetGO.transform.position.x - 1, oldCCPos.y, _targetGO.transform.position.z - 0.5f + (i * 1));

                        if (playerCharList[i] != currentCharacterTurn)
                        {
                            playerCharList[i].GetComponent<BaseEntity>().BaseEntityAnimation(_animation);
                        }
                    }
                    camManager.CameraCloseUp(new Vector3(_targetGO.transform.position.x - 1.5f, 1, _targetGO.transform.position.z - 2));

                    yield return new WaitForSeconds(currentCharacterGO.attackClip.length);

                    for (int i = 0; i < playerCharList.Count; ++i)
                    {
                        playerCharList[i].transform.position = oldPosList[i];
                    }
                    oldPosList.Clear();
                }
                else
                {
                    currentCharacterTurn.transform.position = new Vector3(_targetGO.transform.position.x - 1, oldCCPos.y, _targetGO.transform.position.z);
                    camManager.CameraCloseUp(new Vector3(_targetGO.transform.position.x - 1.5f, 1, _targetGO.transform.position.z - 2));

                    yield return new WaitForSeconds(currentCharacterGO.attackClip.length);

                    currentCharacterTurn.transform.position = oldCCPos;
                }
                break;
            case BaseEntity.ANIMATION.SKILL:
                yield return new WaitForSeconds(currentCharacterGO.skillClip.length);
                break;
            case BaseEntity.ANIMATION.GUARD:
                yield return new WaitForSeconds(0.5f);
                break;
            case BaseEntity.ANIMATION.LIMIT_BREAK:
                if (currentCharacterGO.lbList[currentCharacterGO.lbSkillNum])
                    StartCoroutine(currentCharacterGO.LimitBreakAnimation(camManager.mainCamera, currentCharacterGO.lbSkillNum));
                yield return new WaitForSeconds(currentCharacterGO.GetLBAnimationTime() - 0.5f);
                break;
        }
        camManager.SetMainCameraToOriginalState();

        if (currentCharacterGO.currentAction == BaseEntity.ACTION.TEAM_ATTACK)
        {
            float bonusDmg = 0;
            for (int i = 0; i < playerCharList.Count; ++i)
            {
                bonusDmg += playerCharList[i].GetComponent<BaseEntity>().attackStat * 1.5f;
            }
            bonusDmg -= currentCharacterTurn.GetComponent<BaseEntity>().attackStat;
            StartCoroutine(uiManager.ActivatePlayerDamageUI(currentCharacterGO.CalculateDamage(targetGO, bonusDmg)));
        }
        else
        {
            StartCoroutine(uiManager.ActivatePlayerDamageUI(currentCharacterGO.CalculateDamage(targetGO)));
        }
            

        //Delete dead enemy
        DeleteDeadEnemies();

        uiManager.ActivateActionUI();

        currentCharacterGO.currentSkill = null;
        NextPlayerTurn();
    }

    IEnumerator WaitForEnemyAnimation(GameObject _targetGO)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();
        BaseEntity targetGO = _targetGO.GetComponent<BaseEntity>();

        Vector3 oldCCPos = currentCharacterTurn.transform.position;
        currentCharacterTurn.transform.position = new Vector3(_targetGO.transform.position.x + 1, oldCCPos.y, _targetGO.transform.position.z);
        camManager.CameraCloseUp(new Vector3(_targetGO.transform.position.x + 1.5f, 1, _targetGO.transform.position.z - 2));

        //Wait for animation to end
        yield return new WaitForSeconds(currentCharacterGO.attackClip.length);

        currentCharacterTurn.transform.position = oldCCPos;
        camManager.SetMainCameraToOriginalState();

        StartCoroutine(uiManager.ActivateEnemyDamageUI(currentCharacterGO.CalculateDamage(targetGO)));

        NextEnemyTurnConditions();
    }

    public void SwitchToCurrentPlayerCharSkills()
    {
        BaseEntity currentEntity = currentCharacterTurn.GetComponent<BaseEntity>();

        if (currentEntity.entityType != BaseEntity.ENTITY_TYPE.PLAYER)
            return;

        uiManager.SwitchCurrentPlayerSkillsUIByName(currentEntity.entityName);
    }

    public bool CheckIfTeamAttackAvailable()
    {
        int currentPlayerCharIndex = playerCharList.IndexOf(currentCharacterTurn);

        if (currentPlayerCharIndex == 0)
            return true;

        return false;
    }
}
