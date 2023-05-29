using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] List<EnemyMovement> aggroedEnemies;

    static AIManager instance;

    [SerializeField] float attackTimer;
    [SerializeField] int chanceToAttack;
    [SerializeField] int currentChanceToAttack;
    [SerializeField] int chanceIncrease;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Multiple instances of AIManager", gameObject);
    }
    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            attackTimer += 1;
            CalculateAttackChance();
        }
    }
    void CalculateAttackChance()
    {
        if (Random.Range(0, 100) <= currentChanceToAttack)
        {
            currentChanceToAttack = chanceToAttack; // reset chance

            SetupBias();
        }
        else
        {
            currentChanceToAttack += chanceIncrease;
        }
    }

    void SetupBias()
    {
        int attackingEnemyAmount = 1 + Mathf.FloorToInt(aggroedEnemies.Count / 5f);
        attackingEnemyAmount = Mathf.Clamp(attackingEnemyAmount, 1, 3);

        List<Bias> biases = new List<Bias>();

        foreach (EnemyMovement enemy in aggroedEnemies)
        {
            biases.Add(new Bias(enemy.GetBiasValue(), enemy));
        }

        for (int i = 0; i < attackingEnemyAmount; i++)
        {
            CalculateBias(ref biases);
        }
    }
    void CalculateBias(ref List<Bias> biases)
    {
        float totalBias = 0;

        foreach (Bias bias in biases)
        {
            totalBias += bias.biasValue;
        }
        float chosenValue = Random.Range(0, totalBias);

        for (int i = 0; i < biases.Count; i++)
        {
            chosenValue -= biases[i].biasValue;
            if (chosenValue < 0)
            {
                biases[i].enemy.SignalPrepareAttack();
                biases.RemoveAt(i);
                return;
            }
        }
    }
    struct Bias // find better name!
    {
        public float biasValue;
        public EnemyMovement enemy;

        public Bias(float biasValue, EnemyMovement enemy)
        {
            this.biasValue = biasValue;
            this.enemy = enemy;
        }
    }

    public static void AddAggroedEnemy(EnemyMovement _enemy)
    {
        if (instance.aggroedEnemies.Contains(_enemy))
        {
            Debug.LogWarning($"{_enemy.name} already exists in list", _enemy.gameObject);
            return;
        }

        instance.aggroedEnemies.Add(_enemy);
    }
    public static void RemoveAggroedEnemy(EnemyMovement _enemy)
    {
        if (instance.aggroedEnemies.Contains(_enemy))
        {
            instance.aggroedEnemies.Remove(_enemy);
            return;
        }

        Debug.LogWarning($"{_enemy.name} does not exists in list", _enemy.gameObject);
    }
}
