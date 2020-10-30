using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public interface IAddNode
{
    bool OnSelectEntry(SearchTreeEntry entry, Vector2 mousePosition);
}

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DialogueGraph window;
    private IAddNode graphView;

    public List<SearchTreeEntry> tree;

    public void Configure(DialogueGraph window, IAddNode graphView)
    {
        this.window = window;
        this.graphView = graphView;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
    {
        //Editor window-based mouse position
        var mousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent,
            context.screenMousePosition - window.position.position);

        return graphView.OnSelectEntry(searchTreeEntry, mousePosition);
    }
}