using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBreak : MonoBehaviour
{
    public Skill lbSkill;

    public List<Vector3> v3CameraPosition = new List<Vector3>();
    public List<Vector3> v3CameraRotation = new List<Vector3>(); //In euler angles

    public List<AnimationClip> lbClipList;

    public float GetLimitBreakAnimationTime()
    {
        float animTime = 0.0f;

        for (int i = 0; i < lbClipList.Count; ++i)
        {
            animTime += lbClipList[i].length;
        }

        return animTime;
    }
}
