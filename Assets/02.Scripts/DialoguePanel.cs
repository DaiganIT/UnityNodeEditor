using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public static DialoguePanel Instance;

    [SerializeField]
    TextMeshProUGUI npcNameText;
    [SerializeField]
    TextMeshProUGUI messageText;

    Dialogue currentDialogue;
    NodeData currentNode;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            ShowNextMessage();
        }
    }

    public void Show(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        gameObject.SetActive(true);
        ShowNextMessage();
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        if (currentDialogue != null)
        {
            currentNode = null;
            currentDialogue = null;
            Hero.Instance.InteractFinished();
        }
    }

    void ShowNextMessage()
    {
        var dialogueData = DialogueGraphWalker.GetNextMessage(currentDialogue, ref currentNode);

        if (dialogueData == null)
        {
            Hide();
            return;
        }

        npcNameText.text = dialogueData.npcName;
        messageText.text = dialogueData.message;
    }
}
