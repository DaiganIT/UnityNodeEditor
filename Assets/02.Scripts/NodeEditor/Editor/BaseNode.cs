using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BaseNode : Node
{
    public string nodeGuid;
    public VisualElement settingsContainer;
    
    public BaseNode()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("Node"));
        AddToClassList("node");

        CreateSettingsContainer();
    }

    protected void AddInputPort(string name)
    {
        var inputPort = GetPortInstance(Direction.Input, Port.Capacity.Multi);
        inputPort.portName = name;
        inputContainer.Add(inputPort);
    }

    protected void AddOutputPort(string name)
    {
        var outputPort = GetPortInstance(Direction.Output, Port.Capacity.Multi);
        outputPort.portName = name;
        outputContainer.Add(outputPort);
    }

    public Port GetInputPort(string name)
    {
        return inputContainer.Query<Port>().ToList().SingleOrDefault(x => x.portName == name);
    }

    public Port GetOutputPort(string name)
    {
        return outputContainer.Query<Port>().ToList().SingleOrDefault(x => x.portName == name);
    }

    void CreateSettingsContainer()
    {
        var contentsContainer = mainContainer.Q<VisualElement>("contents");
        var settingsDivider = new VisualElement();
        settingsDivider.name = "divider";
        settingsDivider.AddToClassList("horizontal");
        contentsContainer.Insert(0, settingsDivider);

        settingsContainer = new VisualElement();
        settingsContainer.name = "settings";
        contentsContainer.Insert(1, settingsContainer);
    }

    public virtual void AddPorts() { }
    #region save data
    public virtual string CreateAdditionalData() { return null; }
    public virtual void PopulateAdditionalData(string JSON) { }
    #endregion
    public virtual void AddSettings() { }

    protected Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }
}