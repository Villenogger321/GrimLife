using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScore : MonoBehaviour
{
    [SerializeField] int combatScore;
    [SerializeField] string combatGrade = "D";
    [SerializeField] float scoreMultiplier = 1;

    int scoreCooldownTime = 10;
    float scoreCooldownTimer;

    [SerializeField] float gradeScore;
    [SerializeField] int gradeIndex;
    [SerializeField] int[] gradeThreshold;
    [SerializeField] string[] scoreGrade;
    [SerializeField] float[] gradeScoreMultiplier;

    UIManager uiManager;

    void Start()
    {
        uiManager = UIManager.Instance;
    }
    void Update()
    {
        if (combatScore <= 0)
            return;

        if (scoreCooldownTimer > 0)
            scoreCooldownTimer -= Time.deltaTime;
        else
            EndCombatScore();

        if (gradeScore > 0)
        {
            gradeScore -= Time.deltaTime;
            CalculateGrade();
        }
        else
            gradeScore = 0;
    }
    public void CalculateCombatScore(AttackSO _combo)
    {
        scoreCooldownTimer = scoreCooldownTime;
        AddCombatScore(_combo.ScoreReward);
        scoreMultiplier += 0.1f;
        gradeScore += 5;

        CalculateGrade();
    }
    void CalculateGrade()
    {
        for (int i = 0; i < scoreGrade.Length; i++)
        {
            if (gradeScore >= gradeThreshold[i])
            {
                gradeIndex = i;
                combatGrade = scoreGrade[gradeIndex];
                uiManager.CombatGradeText.text = combatGrade.ToString();
                break;
            }
        }
    }
    public void EndCombatScore()
    {
        FinishedCombatCombo();

        SetCombatScore(0);
        scoreMultiplier = 1;
        gradeScore = 0;
        scoreCooldownTimer = scoreCooldownTime;
        combatGrade = "D";
        uiManager.CombatGradeText.text = combatGrade;
    }

    void FinishedCombatCombo()
    {
        combatScore = (int)(combatScore * gradeScoreMultiplier[gradeIndex]);

        print("grade: " + combatGrade + " score: " + combatScore + 
            " grade multiplier: " + gradeScoreMultiplier[gradeIndex]);
    }
    void AddCombatScore(int _add)
    {
        combatScore += (int)(_add * scoreMultiplier);
        uiManager.ScoreText.text = combatScore.ToString();
    }
    void SetCombatScore(int _set)
    {
        combatScore = _set;
        uiManager.ScoreText.text = combatScore.ToString();
    }
    public void EndCombatCombo()
    {
        scoreMultiplier = 1;
    }
}
/* *Multiplier based on combat combo hits, reset when combat combo ends
 * Grading decays 1/sec, every hit gives 5. Grades are 0 10 20 30 40 50
 * When score combo ends = combatscore * grademultiplier
 */