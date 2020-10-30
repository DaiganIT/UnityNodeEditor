using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView, IAddNode
{
    public readonly Vector2 DefaultNodeSize = new Vector2(300, 150);
    private Texture2D indentationIcon;
    NodeSearchWindow searchWindow;

    public DialogueGraphView(DialogueGraph editorWindow)
    {
        styleSheets.Add(Resources.Load<StyleSheet>("NarrativeGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        indentationIcon = new Texture2D(1, 1);
        indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
        indentationIcon.Apply();

        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(DialogueGraph editorWindow)
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Configure(editorWindow, this);
        nodeCreationRequest = context =>
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);

        searchWindow.tree = CreateMenuOptions();
    }

    protected List<SearchTreeEntry> CreateMenuOptions()
    {
        var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
                new SearchTreeGroupEntry(new GUIContent("Dialogue"), 1),
                new SearchTreeEntry(new GUIContent("Dialogue Node", indentationIcon))
                {
                    level = 2, userData = nameof(DialogueNode)
                },
    #region more node types
                //new SearchTreeEntry(new GUIContent("Options Node", indentationIcon))
                //{
                //    level = 2, userData = nameof(DialogueOptionsNode)
                //},
                //new SearchTreeGroupEntry(new GUIContent("Dialogue Start Condition"), 1),
                //new SearchTreeEntry(new GUIContent("If Quest Dialogue Node", indentationIcon))
                //{
                //    level = 2, userData = nameof(IfQuestDialogueNode)
                //},
                //new SearchTreeGroupEntry(new GUIContent("Dialogue Events"), 1),
                //new SearchTreeEntry(new GUIContent("Register Event Node", indentationIcon))
                //{
                //    level = 2, userData = nameof(DialogueRegisterEventNode)
                //},
    #endregion
            };

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, Vector2 mousePosition)
    {
        var graphMousePosition = contentViewContainer.WorldToLocal(mousePosition);

        BaseNode tempNode = null;
        if (searchTreeEntry.userData.ToString() == nameof(DialogueNode))
        {
            tempNode = CreateNode<DialogueNode>("Dialogue", Vector2.zero);
        }
        if (tempNode == null)
            return false;

        tempNode.SetPosition(new Rect(graphMousePosition, DefaultNodeSize));
        AddElement(tempNode);
        return true;
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

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
        {

            if (// link start and end port must be different
            startPort != port
            // link start and end node must be different
            && startPort.node != port.node
            // link start and end port type must match
            && startPort.portType == port.portType)
            // link end must be Input
            //&& port.direction == Direction.Input)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }
}
