using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScore : MonoBehaviour
{
    int combatScore;

    int cooldownTime = 10;
    float cooldownTimer;

    [SerializeField] List<int> scoreGrading;

    // add combat grading thresholds
    UIManager uiManager;

    void Start()
    {
        uiManager = UIManager.Instance;
    }
    void Update()
    {
        if (combatScore <= 0)
            return;

        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
        else
            EndCombatScore();
    }
    public void CalculateCombatScore(AttackSO _combo)
    {
        cooldownTimer = cooldownTime;

        

        AddCombatScore(_combo.ScoreReward);
    }
    public void EndCombatScore()
    {
        SetCombatScore(0);
        cooldownTimer = cooldownTime;
    }

    public int GetCombatScore()
    {
        return combatScore;
    }
    void AddCombatScore(int _add)
    {
        combatScore += _add;
        uiManager.ScoreText.text = combatScore.ToString();
    }
    void SetCombatScore(int _set)
    {
        combatScore = _set;
        uiManager.ScoreText.text = combatScore.ToString();
    }
}
