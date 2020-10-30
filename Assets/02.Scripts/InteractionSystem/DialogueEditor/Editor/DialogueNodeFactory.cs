using UnityEngine;

public class DialogueNodeFactory : INodeFactory
{
    public BaseNode CreateNode(string nodeType, BaseGraphView graphView)
    {
        BaseNode tempNode = null;
        if (nodeType == nameof(DialogueNode))
        {
            tempNode = graphView.CreateNode<DialogueNode>("Dialogue", Vector2.zero);
        }
        else if (nodeType == nameof(IfEventDialogueNode))
        {
            tempNode = graphView.CreateNode<IfEventDialogueNode>("If Event", Vector2.zero);
        }
        //else if (nodeType == nameof(IfQuestDialogueNode))
        //{
        //    tempNode = graphView.CreateNode<IfQuestDialogueNode>("If Quest", Vector2.zero);
        //}
        else if (nodeType == nameof(DialogueOptionsNode))
        {
            tempNode = graphView.CreateNode<DialogueOptionsNode>("Dialogue Option", Vector2.zero);
        }
        else if (nodeType == nameof(DialogueRegisterEventNode))
        {
            tempNode = graphView.CreateNode<DialogueRegisterEventNode>("Register Event", Vector2.zero);
        }
        //else if (nodeType == nameof(DialogueQuestEventNode))
        //{
        //    tempNode = graphView.CreateNode<DialogueQuestEventNode>("Quest Event", Vector2.zero);
        //}
        else
        {
            Debug.LogError("Invalid type " + nodeType);
        }

        return tempNode;
    }
}