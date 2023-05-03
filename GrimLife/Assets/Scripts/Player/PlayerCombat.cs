using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrimLife
{
    public class PlayerCombat : MonoBehaviour
    {
        public List<AttackSO> Combo;
        float lastClickedTime;
        float lastComboEnd;
        int comboCount;

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

                if (Time.time - lastClickedTime >= 2f)
                {
                    anim.runtimeAnimatorController = Combo[comboCount].animatorOV;
                    anim.Play("Attack", 0, 0);
                    // set weapon damage to combo[comboCount].damage
                    // attack effects and sounds also go here
                    comboCount++;
                    lastClickedTime = Time.time;
                    if(comboCount + 1 > Combo.Count)
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
    }
}
