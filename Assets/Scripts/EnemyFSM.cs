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

    public BattleManager battleManager;
    public BaseEntity enemy;

    public TURN_STATE currentState;

    public GameObject targetGO;

    private void Start()
    {
        battleManager = GameObject.FindObjectOfType<BattleManager>();
        enemy = this.GetComponent<BaseEntity>();
    }

    public void SetCurrentState(TURN_STATE newState)
    {
        currentState = newState;
    }
}
