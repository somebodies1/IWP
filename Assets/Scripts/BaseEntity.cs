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

    //Name
    public string entityName;

    //Health
    public int MaxHP;
    public int CurrentHP;

    //Mana
    public int MaxMP;
    public int CurrentMP;

    //Combat Stats
    public float attackStat = 10.0f;
    public float defStat = 5.0f;


    //Type of entity
    public ENTITY_TYPE entityType;

    public GameObject HealthBar;

    public Animator animator;
    public AnimationClip idleClip;
    public AnimationClip attackClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
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

    public bool AttackAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            Debug.Log("AttackAnimTime: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Debug.Log("attackClip: " + attackClip.length);
            return true;
        }

        return false;
    }
}
