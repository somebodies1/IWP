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

    public bool isDead = false;

    public BaseEntity(string _name, int _hp, int _mp, ENTITY_TYPE _type)
    {
        entityName = _name;
        MaxHP = CurrentHP = _hp;
        MaxMP = CurrentMP = _mp;
        entityType = _type;
    }
}
