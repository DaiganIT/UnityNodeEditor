using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseGraphView : GraphView
{
    public readonly Vector2 DefaultNodeSize = new Vector2(300, 150);
    NodeSearchWindow searchWindow;
    protected INodeFactory nodeFactory;

    public BaseGraphView(BaseNodeGraph editorWindow)
    {
        styleSheets.Add(Resources.Load<StyleSheet>("NarrativeGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        //nodeFactory = CreateNodeFactory();

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(BaseNodeGraph editorWindow)
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        //searchWindow.Configure(editorWindow);
        nodeCreationRequest = context =>
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);

        //searchWindow.tree = CreateMenuOptions();
    }

    public void AddNode<T>(string nodeName, Vector2 position) where T : BaseNode, new()
    {
        AddElement(CreateNode<T>(nodeName, position));
    }

    public void AddNode(BaseNode node)
    {
        AddElement(node);
    }

    public BaseNode CreateNode<T>(string nodeName, Vector2 position) where T : BaseNode, new()
    {
        var tempNode = new T { nodeGuid = Guid.NewGuid().ToString(), title = nodeName };

        tempNode.AddSettings();
        tempNode.AddPorts();

        tempNode.RefreshExpandedState();
        tempNode.RefreshPorts();
        tempNode.SetPosition(new Rect(position, DefaultNodeSize));

        return tempNode;
    }

    //protected abstract List<SearchTreeEntry> CreateMenuOptions();
    //protected abstract INodeFactory CreateNodeFactory();
    //public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, Vector2 graphMousePosition)
    //{
    //    var node = nodeFactory.CreateNode(searchTreeEntry.userData.ToString(), this);
    //    if (node == null)
    //        return false;

    //    node.SetPosition(new Rect(graphMousePosition, DefaultNodeSize));
    //    AddNode(node);
    //    return true;
    //}
}
