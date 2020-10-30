using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseNodeGraph : EditorWindow
{
    public GraphItem currentGraphItem { get; set; }

    protected BaseGraphView graphView { get; private set; }
    GraphSaveUtility saveUtility;

    protected static string windowTitle;

    public void Initialise()
    {
        //saveUtility = CreateSaveUtility(graphView);
        //saveUtility.Load(currentGraphItem);
        //OnLoaded();
    }

    private void OnEnable()
    {
        ConstructGraphView();
        //GenerateToolbar();
        GenerateMiniMap();
    }

    //private void Save()
    //{
    //    saveUtility.Save(currentGraphItem);
    //    OnSaved();
    //}

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void ConstructGraphView()
    {
        graphView = CreateGraphView();
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    //private void GenerateToolbar()
    //{
    //    var toolbar = new Toolbar();
    //    toolbar.Add(new ToolbarButton(() => Save()) { text = "Save" });

    //    rootVisualElement.Add(toolbar);
    //}

    private void GenerateMiniMap()
    {
        var miniMap = new MiniMap { anchored = true };
        var cords = graphView.contentViewContainer.WorldToLocal(new Vector2(this.position.width - 210, this.position.height - 150));
        miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
        graphView.Add(miniMap);
    }

    protected abstract BaseGraphView CreateGraphView();
    //protected abstract GraphSaveUtility CreateSaveUtility(BaseGraphView graphView);
    //protected virtual void OnLoaded() { }
    //protected virtual void OnSaved() { }
}
