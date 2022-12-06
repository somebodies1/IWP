using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEntity : MonoBehaviour
{
    public enum ENTITY_TYPE
    {
        PLAYER,
        ENEMY,
        ENTITY_TYPE_NUM
    }

    public enum ANIMATION
    {
        IDLE,
        ATTACK,
        SKILL,
        GUARD,
        END_GUARD,
        ANIMATION_NUM
    }

    public enum ACTION
    {
        ATTACK,
        SKILL,
        GUARD,
        FULL_GUARD,
        ACTION_NUM
    }

    public enum TEMPERAMENT
    {
        NONE,
        FIRE,
        WATER,
        EARTH,
        SKY,
        SUN,
        MOON,
        NUM_TEMPERAMENT
    }

    //Name
    public string entityName;

    //Health
    public int MaxHP = 100;
    public int CurrentHP = 100;

    //Mana
    public int MaxMP = 100;
    public int CurrentMP = 100;

    //Combat Stats
    public float attackStat = 10.0f;
    public float defStat = 5.0f;

    //Action
    public ACTION currentAction;

    //Skills
    public List<Skill> skillList;
    public Skill currentSkill;

    //Guard
    public int fullGuardAmt = 1;

    //Type of entity
    public ENTITY_TYPE entityType;
    public List<int> tpWeaknessList; //-1 Weak  0 Neutral  1 Strong

    public GameObject HealthBar;

    public Animator animator;
    public AnimationClip idleClip;
    public AnimationClip attackClip;
    public AnimationClip skillClip;
    public AnimationClip guardClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        //Debug.Log("rotation: " + this.gameObject.transform.rotation.eulerAngles);
    }

    public bool CheckIfCurrentActionGuard()
    {
        if (currentAction == ACTION.GUARD ||
            currentAction == ACTION.FULL_GUARD)
            return true;
        else
            return false;
    }

    //Full guard amt will only decrease when not guarding or full guarding
    void FullGuardAmtBreak(BaseEntity _targetGO)
    {
        if (!_targetGO.CheckIfCurrentActionGuard())
        {
            _targetGO.fullGuardAmt -= 1;
        }
    }

    //Guard effects
    float CompareGuard(BaseEntity _targetGO)
    {
        switch (_targetGO.currentAction)
        {
            case ACTION.GUARD:
                return 0.5f;
            case ACTION.FULL_GUARD:
                return 0.0f;
            default:
                return 1.0f;
        }
    }

    //Temperament effects
    float CompareTemperament(BaseEntity _targetGO, TEMPERAMENT _tpAttack = TEMPERAMENT.NONE)
    {
        //Get enemy's weakness of specific temperament
        int tpID = _targetGO.tpWeaknessList[(int)_tpAttack];

        //Get multiplier for that temperament
        float tpMultiplier;
        switch (tpID)
        {
            case -1:
                tpMultiplier = 2.0f;
                FullGuardAmtBreak(_targetGO);
                break;
            case 0:
                tpMultiplier = 1.0f;
                break;
            case 1:
                tpMultiplier = 0.5f;
                break;
            default:
                tpMultiplier = 1.0f;
                break;
        }

        return tpMultiplier;
    }

    //Total damage calculation
    public void CalculateDamage(BaseEntity _targetGO, TEMPERAMENT _tpAttack = TEMPERAMENT.NONE)
    {
        if (CheckIfCurrentActionGuard())
            return;

        float totalDmg = 0;

        //Source of attack stat
        switch (currentAction)
        {
            case ACTION.ATTACK:
                totalDmg = attackStat;
                break;
            case ACTION.SKILL:
                totalDmg = currentSkill.skillStrength;
                break;
        }
        Debug.Log("atkStat" + totalDmg);

        //Compare against temperament
        TEMPERAMENT temperament = _tpAttack;

        if (currentSkill)
            temperament = currentSkill.skillType;

        float tpMultiplier = CompareTemperament(_targetGO, temperament);
        totalDmg *= tpMultiplier;
        Debug.Log("afterTPDmg" + totalDmg);

        //Compare against defense
        totalDmg -= _targetGO.defStat;
        Debug.Log("finalDmg: " + totalDmg);

        //Compare against guard
        totalDmg *= CompareGuard(_targetGO);
        Debug.Log("afterGuard" + totalDmg);

        //Finalized damage
        _targetGO.CurrentHP -= (int)totalDmg;

        _targetGO.UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float currentToMaxHealthRatio = (float)CurrentHP / (float)MaxHP;
        HealthBar.transform.localScale = new Vector3(currentToMaxHealthRatio, 1, 1);
    }

    //Triggers animations to start
    public bool BaseEntityAnimation(ANIMATION _anim)
    {
        if (animator != null)
        {
            switch (_anim)
            {
                case ANIMATION.IDLE:
                    break;
                case ANIMATION.ATTACK:
                    animator.SetTrigger("Attack");
                    break;
                case ANIMATION.SKILL:
                    animator.SetTrigger("Skill");
                    break;
                case ANIMATION.GUARD:
                    animator.SetTrigger("Guard");
                    break;
                case ANIMATION.END_GUARD:
                    animator.SetTrigger("EndGuard");
                    break;
            }

            return true;
        }

        return false;
    }

    //Converts action enums to animation enums
    public BaseEntity.ANIMATION CurrentActionToAnimation(ACTION _action)
    {
        switch(_action)
        {
            case ACTION.ATTACK:
                return ANIMATION.ATTACK;
            case ACTION.SKILL:
                return ANIMATION.SKILL;
            case ACTION.GUARD:
                return ANIMATION.GUARD;
            case ACTION.FULL_GUARD:
                return ANIMATION.GUARD;
        }

        return ANIMATION.IDLE;
    }
}
