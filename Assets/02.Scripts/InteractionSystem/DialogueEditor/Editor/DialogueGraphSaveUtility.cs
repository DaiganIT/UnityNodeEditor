using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class DialogueGraphSaveUtility
{
    public static void Save(DialogueGraphView graphView, GraphItem graphItem)
    {
        SaveNodes(graphView, graphItem);
        EditorUtility.SetDirty(graphItem);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void SaveNodes(DialogueGraphView graphView, GraphItem graphItem)
    {
        graphItem.NodeLinks.Clear();
        var connectedSockets = graphView.edges.ToList().Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedSockets.Count(); i++)
        {
            var outputNode = (connectedSockets[i].output.node as BaseNode);
            var inputNode = (connectedSockets[i].input.node as BaseNode);
            graphItem.NodeLinks.Add(new NodeLinkData
            {
                baseNodeGuid = outputNode.nodeGuid,
                outputPortName = connectedSockets[i].output.portName,
                inputPortName = connectedSockets[i].input.portName,
                targetNodeGuid = inputNode.nodeGuid,
            });
        }

        graphItem.NodeData.Clear();
        foreach (var node in graphView.nodes.ToList().Cast<BaseNode>())
        {
            graphItem.NodeData.Add(new NodeData
            {
                nodeGuid = node.nodeGuid,
                position = node.GetPosition().position,
                nodeType = node.GetType().FullName,
                additionalDataJSON = node.CreateAdditionalData()
            });
        }
    }

    public static void Load(DialogueGraphView graphView, GraphItem graphItem)
    {
        ClearGraph(graphView);
        GenerateDialogueNodes(graphView, graphItem);
        ConnectDialogueNodes(graphView, graphItem);
    }

    private static void ClearGraph(DialogueGraphView graphView)
    {
        foreach (var perNode in graphView.nodes.ToList())
        {
            graphView.edges.ToList().Where(x => x.input.node == perNode).ToList().ForEach(edge => graphView.RemoveElement(edge));
            graphView.RemoveElement(perNode);
        }
    }

    private static void GenerateDialogueNodes(DialogueGraphView graphView, GraphItem graphItem)
    {
        foreach (var node in graphItem.NodeData)
        {
            BaseNode tempNode = null;
            if (node.nodeType == nameof(DialogueNode))
            {
                tempNode = graphView.CreateNode<DialogueNode>("Dialogue", Vector2.zero);
            }
            if (tempNode == null) continue;

            tempNode.nodeGuid = node.nodeGuid;
            tempNode.SetPosition(new Rect(node.position, graphView.DefaultNodeSize));
            graphView.AddElement(tempNode);
            tempNode.PopulateAdditionalData(node.additionalDataJSON);
        }
    }

    private static void ConnectDialogueNodes(DialogueGraphView graphView, GraphItem graphItem)
    {
        var nodes = graphView.nodes.ToList().Cast<BaseNode>();
        foreach (var edge in graphItem.NodeLinks)
        {
            var baseNode = nodes.SingleOrDefault(x => x.nodeGuid == edge.baseNodeGuid);
            var targetNode = nodes.SingleOrDefault(x => x.nodeGuid == edge.targetNodeGuid);

            var baseNodePort = baseNode.GetOutputPort(edge.outputPortName);
            var targetNodePort = targetNode.GetInputPort(edge.inputPortName);

            var createdEdge = LinkNodesTogether(baseNodePort, targetNodePort);
            graphView.Add(createdEdge);
        }
    }

    private static Edge LinkNodesTogether(Port outputSocket, Port inputSocket)
    {
        var tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);

        return tempEdge;
    }
}
