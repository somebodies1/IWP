using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum TURN_STATE
    {
        WAITING, //When character's action has not been done
        TURN_ENDED, //When character finished their action
        TURN_STATE_NUM
    }

    public BaseEntity enemy;

    public TURN_STATE currentState;

    public GameObject targetGO;

    private void Start()
    {
        enemy = this.GetComponent<BaseEntity>();
    }

    public void SetCurrentState(TURN_STATE newState)
    {
        currentState = newState;
    }

    //Decide which player char to target
    public void EnemyAITargeting(List<GameObject> _playerCharList)
    {
        //Random targeting
        int playerListID = Random.Range(0, _playerCharList.Count);
        targetGO = _playerCharList[playerListID];
    }
}
