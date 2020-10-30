using Newtonsoft.Json;
using System.Linq;

public static class DialogueGraphWalker
{
    public static DialogueAdditionalData GetNextMessage(Dialogue dialogue, ref NodeData currentNode)
    {
        if (currentNode == null)
        {
            currentNode = dialogue.NodeData.FirstOrDefault(x => dialogue.NodeLinks.All(y => y.targetNodeGuid != x.nodeGuid));
            return JsonConvert.DeserializeObject<DialogueAdditionalData>(currentNode.additionalDataJSON);
        }

        var theCurrentNode = currentNode;
        var nextLink = dialogue.NodeLinks.FirstOrDefault(x => x.baseNodeGuid == theCurrentNode.nodeGuid && x.outputPortName == "Next");
        if (nextLink == null)
        {
            currentNode = null;
            return null;
        }

        var nextNodeId = nextLink.targetNodeGuid;
        currentNode = dialogue.NodeData.SingleOrDefault(x => x.nodeGuid == nextNodeId);
        return JsonConvert.DeserializeObject<DialogueAdditionalData>(currentNode.additionalDataJSON);
    }
}