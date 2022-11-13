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

    //Type of entity
    public ENTITY_TYPE entityType;

    public GameObject HealthBar;

    public void UpdateHealthBar()
    {
        float currentToMaxHealthRatio = (float)CurrentHP / (float)MaxHP;
        HealthBar.transform.localScale = new Vector3(currentToMaxHealthRatio, 1, 1);
    }
}
