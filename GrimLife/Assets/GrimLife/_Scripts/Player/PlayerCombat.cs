using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    public List<AttackSO> Combo;
    [SerializeField] LayerMask hitMask;
    float lastClickedTime;
    float lastComboEnd;
    int comboCount;
    int damage;
    bool readyToHit = true;

    Animator anim;
    PlayerCombatScore playerCombatScore;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerCombatScore = GetComponent<PlayerCombatScore>();
    }

    void Update()
    {
        ExitAttack();
    }
    void Attack()
    {
        if (Time.time - lastComboEnd > .75f && comboCount <= Combo.Count)
        {
            CancelInvoke("EndCombo");
            AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);

            if (readyToHit)
            {
                readyToHit = false;
                anim.runtimeAnimatorController = Combo[comboCount].AnimatorOV;
                anim.Play("Attack", 0, 0);

                damage = Combo[comboCount].Damage;
                // attack effects and sounds also go here

                comboCount++;
                lastClickedTime = Time.time;
                if (comboCount + 1 > Combo.Count)
                    comboCount = 0;
            }
        }
    }
    void ExitAttack()
    {
        AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.normalizedTime > 0.9 && animStateInfo.IsTag("Attack"))
            Invoke("EndCombo", 1);  // 1 == cooldown until combo ended
    }
    void EndCombo()
    {
        comboCount = 0;
        lastComboEnd = Time.time;
        playerCombatScore.EndCombatCombo();
        readyToHit = true;
    }
    void OnFire()
    {
        Attack();
    }
    void CalculateHit()
    {
        Collider[] damageAble = Physics.OverlapBox(transform.position + transform.forward * 1.5f, new Vector3(1f, 1.5f, 1f),
            transform.rotation, hitMask);

        for (int i = 0; i < damageAble.Length; i++)
        {
            if (damageAble[i].GetComponent<Health>() is Health damageAbleHealth)
            {
                damageAbleHealth.TakeDamage(damage);

                if (damageAble[i].CompareTag("Enemy"))
                    playerCombatScore.CalculateCombatScore(Combo[comboCount]);
            }
        }
    }
    void ResetHitCooldown()
    {
        readyToHit = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + transform.forward * 1.5f, new Vector3(2f, 3f, 2f));
    }
}
