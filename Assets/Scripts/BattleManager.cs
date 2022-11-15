using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public UIManager uiManager;

    public List<GameObject> playerCharList;
    public List<GameObject> enemiesList;

    public GameObject currentCharacterTurn;

    //Turns always start with player
    private void Start()
    {
        playerCharList.AddRange(GameObject.FindGameObjectsWithTag("PlayerChar"));
        enemiesList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        SwitchToPlayerTurn();
    }

    //Switch to player's turn
    private void SwitchToPlayerTurn()
    {
        playerCharList[0].GetComponent<PlayerFSM>().SetCurrentState(PlayerFSM.TURN_STATE.SELECTING);
        currentCharacterTurn = playerCharList[0];
    }

    //When switching from player char to player char
    public void NextPlayerTurn()
    {
        CurrentTurnCheck();

        currentCharacterTurn.GetComponent<PlayerFSM>().SetCurrentState(PlayerFSM.TURN_STATE.TURN_ENDED);

        int currentPlayerCharIndex = playerCharList.IndexOf(currentCharacterTurn);

        Debug.Log("PlayCharIndex: " + currentPlayerCharIndex + " CharListIndex: " + playerCharList.Count);

        //Switch to next player's character's turn
        if (currentPlayerCharIndex >=  playerCharList.Count - 1)
        {
            SwitchToEnemyTurn();
        }
        else
        {
            playerCharList[currentPlayerCharIndex + 1].GetComponent<PlayerFSM>().SetCurrentState(PlayerFSM.TURN_STATE.SELECTING);
            currentCharacterTurn = playerCharList[currentPlayerCharIndex + 1];
        }
    }

    //Switch to enemy's turn
    private void SwitchToEnemyTurn()
    {
        enemiesList[0].GetComponent<EnemyFSM>().SetCurrentState(EnemyFSM.TURN_STATE.WAITING);
        currentCharacterTurn = enemiesList[0];

        NextEnemyTurn();
    }

    //When switching from enemy to enemy
    public void NextEnemyTurn()
    {
        CurrentTurnCheck();

        EnemyAttack(currentCharacterTurn.GetComponent<EnemyFSM>().targetGO);
        currentCharacterTurn.GetComponent<EnemyFSM>().SetCurrentState(EnemyFSM.TURN_STATE.TURN_ENDED);

        int currentEnemyIndex = enemiesList.IndexOf(currentCharacterTurn);

        Debug.Log("CurrentEnemyIndex: " + currentEnemyIndex + " EnemyListIndex: " + enemiesList.Count);
        
        //Switch to next enemy's turn
        if (currentEnemyIndex >= enemiesList.Count - 1)
        {
            SwitchToPlayerTurn();
        }
        else
        {
            enemiesList[currentEnemyIndex + 1].GetComponent<EnemyFSM>().SetCurrentState(EnemyFSM.TURN_STATE.WAITING);
            currentCharacterTurn = enemiesList[currentEnemyIndex + 1];
            
            NextEnemyTurn();
        }
    }

    public void PlayerAttack(GameObject _targetGO)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();

        if (currentCharacterGO.AttackAnimation())
        {
            uiManager.DeactivateActionUI();
            StartCoroutine(WaitForAnimation(_targetGO));
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
        BaseEntity targetGO = _targetGO.GetComponent<BaseEntity>();

        currentCharacterGO.CalculateDamage(targetGO);
    }

    private void CurrentTurnCheck()
    {
        Debug.Log("Current Turn: " + currentCharacterTurn);
    }

    IEnumerator WaitForAnimation(GameObject _targetGO)
    {
        BaseEntity currentCharacterGO = currentCharacterTurn.GetComponent<BaseEntity>();
        BaseEntity targetGO = _targetGO.GetComponent<BaseEntity>();

        //Wait for animation to end
        yield return new WaitForSeconds(currentCharacterGO.attackClip.length);

        currentCharacterGO.CalculateDamage(targetGO);

        uiManager.ActivateActionUI();

        NextPlayerTurn();
    }
}
