using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI CombatGradeText;


    public static UIManager Instance;

    void Awake()
    {
        if (UIManager.Instance != null)
            Destroy(this);
        else
            UIManager.Instance = this;
    }
}
