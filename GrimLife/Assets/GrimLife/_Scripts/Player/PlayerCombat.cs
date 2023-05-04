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
        
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ExitAttack();
    }
    void Attack()
    {
        if (Time.time - lastComboEnd > 0.5f && comboCount <= Combo.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= .2f)
            {
                anim.runtimeAnimatorController = Combo[comboCount].animatorOV;
                anim.Play("Attack", 0, 0);

                damage = Combo[comboCount].damage;
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
    }
    void OnFire()
    {
        Attack();
    }
    public void CalculateHit()
    {
        Collider[] damageAble = Physics.OverlapBox(transform.position + transform.forward * 1.5f, new Vector3(1f, 1.5f, 1f),
            transform.rotation, hitMask);

        for (int i = 0; i < damageAble.Length; i++)
        {
            if (damageAble[i].GetComponent<Health>() is Health damageAbleHealth)
            {
                damageAbleHealth.TakeDamage(damage);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + transform.forward * 1.5f, new Vector3(2f, 3f, 2f));
    }
}
