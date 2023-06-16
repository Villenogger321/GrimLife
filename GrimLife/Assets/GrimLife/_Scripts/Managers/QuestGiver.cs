using UnityEngine;
using System.Collections;

public class QuestGiver : MonoBehaviour
{
	public Quest quest = new Quest();
    string[] textPrompt;
    bool interactedWith;
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (!interactedWith)
            {
                DialogueManager.Instance.StartDialogue(textPrompt);
                interactedWith = true;
                //PlayerStats.Player.GiveQuest(Quest);
            }
        }
    }
}

[System.Serializable]
public class Quest
{
	public bool IsActive;

    public string Title;
    public string Description;

    public QuestGoal Goal;
    // include rewards here
}
[System.Serializable]
public class QuestGoal
{
    public GoalType GoalType;

    public int RequiredAmount;
    public int CurrentAmount;

    public bool IsReached()
    {
        return (CurrentAmount >= RequiredAmount);
    }
    public void EnemyKilled()
    {
        if (GoalType == GoalType.Kill)
            CurrentAmount++;
    }
    public void ItemGathered()
    {
        if (GoalType == GoalType.Gather)
            CurrentAmount++;
    }
}

public enum GoalType
{
    Kill,
    Gather
}
