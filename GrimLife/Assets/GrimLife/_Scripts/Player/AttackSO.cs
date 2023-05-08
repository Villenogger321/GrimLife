using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/New Attack")]
public class AttackSO : ScriptableObject
{
    public AnimatorOverrideController AnimatorOV;
    public int Damage;
    public int ScoreReward;
}

