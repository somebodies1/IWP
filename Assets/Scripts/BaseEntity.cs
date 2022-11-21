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
        ANIMATION_NUM
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

    //Skills
    public List<Skill> skillList;

    //Type of entity
    public ENTITY_TYPE entityType;

    public GameObject HealthBar;

    public Animator animator;
    public AnimationClip idleClip;
    public AnimationClip attackClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("rotation: " + this.gameObject.transform.rotation.eulerAngles);
    }

    public void CalculateDamage(BaseEntity targetGO)
    {
        int totalDmg = (int)(attackStat - targetGO.defStat);

        targetGO.CurrentHP -= totalDmg;
        targetGO.UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float currentToMaxHealthRatio = (float)CurrentHP / (float)MaxHP;
        HealthBar.transform.localScale = new Vector3(currentToMaxHealthRatio, 1, 1);
    }

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
            }

            return true;
        }

        return false;
    }
}
