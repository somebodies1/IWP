using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public enum TURN_STATE
    {
        WAITING, //When character's action has not been done
        SELECTING, //When controlling that character
        TURN_ENDED, //When character finished their action
        TURN_STATE_NUM
    }

    public BattleManager battleManager;
    public BaseEntity playerChar;

    public TURN_STATE currentState;

    private void Start()
    {
        battleManager = GameObject.FindObjectOfType<BattleManager>();
        playerChar = this.GetComponent<BaseEntity>();
    }

    public void SetCurrentState(TURN_STATE newState)
    {
        currentState = newState;
    }

}
