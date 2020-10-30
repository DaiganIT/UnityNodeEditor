using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class GraphSaveUtility
{
    private BaseGraphView graphView;
    private List<Edge> Edges => graphView.edges.ToList();
    private List<BaseNode> Nodes => graphView.nodes.ToList().Cast<BaseNode>().ToList();

    private INodeFactory nodeFactory;

    public GraphSaveUtility(BaseGraphView graphView)
    {
        this.graphView = graphView;
        nodeFactory = CreateNodeFactory();
    }

    public void Save(GraphItem graphItem)
    {
        SaveNodes(graphItem);
        EditorUtility.SetDirty(graphItem);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void SaveNodes(GraphItem graphItem)
    {
        graphItem.NodeLinks.Clear();
        var connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
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
        foreach (var node in Nodes)
        {
            graphItem.NodeData.Add(new NodeData
            {
                nodeGuid = node.nodeGuid,
                position = node.GetPosition().position,
                nodeType = node.GetType().FullName,
                //additionalDataJSON = node.CreateAdditionalData()
            });
        }
    }

    public void Load(GraphItem graphItem)
    {
        ClearGraph();
        GenerateDialogueNodes(graphItem);
        ConnectDialogueNodes(graphItem);
    }

    private void ClearGraph()
    {
        foreach (var perNode in Nodes)
        {
            Edges.Where(x => x.input.node == perNode).ToList().ForEach(edge => graphView.RemoveElement(edge));
            graphView.RemoveElement(perNode);
        }
    }

    private void GenerateDialogueNodes(GraphItem graphItem)
    {
        foreach (var node in graphItem.NodeData)
        {
            var tempNode = nodeFactory.CreateNode(node.nodeType, graphView);
            if (tempNode == null) continue;

            tempNode.nodeGuid = node.nodeGuid;
            tempNode.SetPosition(new Rect(node.position, graphView.DefaultNodeSize));
            graphView.AddElement(tempNode);
            //tempNode.PopulateAdditionalData(node.additionalDataJSON);
        }
    }

    private void ConnectDialogueNodes(GraphItem graphItem)
    {
        foreach (var edge in graphItem.NodeLinks)
        {
            var baseNode = Nodes.SingleOrDefault(x => x.nodeGuid == edge.baseNodeGuid);
            var targetNode = Nodes.SingleOrDefault(x => x.nodeGuid == edge.targetNodeGuid);

            var baseNodePort = baseNode.GetOutputPort(edge.outputPortName);
            var targetNodePort = targetNode.GetInputPort(edge.inputPortName);

            LinkNodesTogether(baseNodePort, targetNodePort);
        }
    }

    private void LinkNodesTogether(Port outputSocket, Port inputSocket)
    {
        var tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        graphView.Add(tempEdge);
    }

    protected abstract INodeFactory CreateNodeFactory();
}
