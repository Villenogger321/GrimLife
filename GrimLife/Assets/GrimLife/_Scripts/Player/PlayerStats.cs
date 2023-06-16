using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Player;
    public Quest quest;
    void Awake()
    {
        Player = this;
    }

    public void GiveQuest(Quest _quest)
    {
        quest = _quest;
        quest.IsActive = true;
    }
}
