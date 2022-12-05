using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill : MonoBehaviour
{
    public string skillName = "Skill";
    public BaseEntity.TEMPERAMENT skillType = BaseEntity.TEMPERAMENT.FIRE;
    public int skillStrength = 10;

    public void OverwriteSkill(Skill _newSkill)
    {
        skillName = _newSkill.skillName;
        skillType = _newSkill.skillType;
        skillStrength = _newSkill.skillStrength;
    }
}
