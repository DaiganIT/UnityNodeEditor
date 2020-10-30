using System.Collections.Generic;
using UnityEngine;

public abstract class GraphItem : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<NodeData> NodeData = new List<NodeData>();
}

[System.Serializable]
public class NodeLinkData
{
    public string baseNodeGuid;
    public string outputPortName;
    public string inputPortName;
    public string targetNodeGuid;
    public string linkType;
}

[System.Serializable]
public class NodeData
{
    public string nodeGuid;
    public Vector2 position;
    public string nodeType;
    public string additionalDataJSON;
}
