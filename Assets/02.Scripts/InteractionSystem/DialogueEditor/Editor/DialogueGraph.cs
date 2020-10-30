using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    public Dialogue currentDialogue { get; set; }
    DialogueGraphView graphView;

    [OnOpenAsset(1)]
    public static bool ShowWindow(int instanceId, int line)
    {
        var item = EditorUtility.InstanceIDToObject(instanceId);
        if (item is Dialogue)
        {
            var window = (DialogueGraph)GetWindow(typeof(DialogueGraph));
            window.titleContent = new GUIContent("Dialogue Graph");
            window.currentDialogue = item as Dialogue;
            window.minSize = new Vector2(500, 250);

            window.Load();

            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    #region Save and Load
    public void Load()
    {
        DialogueGraphSaveUtility.Load(graphView, currentDialogue);
    }

    private void Save()
    {
        DialogueGraphSaveUtility.Save(graphView, currentDialogue);
    }
    #endregion

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView(this)
        {
            name = "Diualogue Graph",
        };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();
        toolbar.Add(new ToolbarButton(Save) { text = "Save" });

        rootVisualElement.Add(toolbar);
    }
}
