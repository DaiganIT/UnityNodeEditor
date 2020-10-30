using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Dialogue")]
public class Dialogue : GraphItem { }

public class DialogueAdditionalData
{
    public string npcName;
    public string message;
}

public enum DialogueQuestEnum
{
    StartQuest,
    ProgressQuest,
    CompleteQuest
}