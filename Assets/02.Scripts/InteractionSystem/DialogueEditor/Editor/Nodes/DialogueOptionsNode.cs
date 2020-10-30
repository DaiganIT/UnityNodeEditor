using UnityEngine.UIElements;

public class DialogueOptionsNode : BaseNode
{
    public string npcName;
    public string message;
    public string optionA;
    public string optionB;
    public string optionC;

    private TextField npcNameTextField;
    private TextField messageTextField;
    private TextField optionATextField;
    private TextField optionBTextField;
    private TextField optionCTextField;

    public override void AddSettings()
    {
        npcNameTextField = new TextField("NPC Name");
        npcNameTextField.RegisterValueChangedCallback(evt =>
        {
            npcName = evt.newValue;
        });
        npcNameTextField.SetValueWithoutNotify(npcName);
        var messageLabel = new Label("Message");
        messageTextField = new TextField();
        messageTextField.multiline = true;
        messageTextField.RegisterValueChangedCallback(evt =>
        {
            message = evt.newValue;
        });

        var optionALabel = new Label("Option A");
        optionATextField = new TextField();
        optionATextField.multiline = true;
        optionATextField.RegisterValueChangedCallback(evt =>
        {
            optionA = evt.newValue;
        });

        var optionBLabel = new Label("Option B");
        optionBTextField = new TextField();
        optionBTextField.multiline = true;
        optionBTextField.RegisterValueChangedCallback(evt =>
        {
            optionB = evt.newValue;
        });

        var optionCLabel = new Label("Option C");
        optionCTextField = new TextField();
        optionCTextField.multiline = true;
        optionCTextField.RegisterValueChangedCallback(evt =>
        {
            optionC = evt.newValue;
        });

        settingsContainer.Add(npcNameTextField);
        settingsContainer.Add(messageLabel);
        settingsContainer.Add(messageTextField);
        settingsContainer.Add(optionALabel);
        settingsContainer.Add(optionATextField);
        settingsContainer.Add(optionBLabel);
        settingsContainer.Add(optionBTextField);
        settingsContainer.Add(optionCLabel);
        settingsContainer.Add(optionCTextField);
    }

    public override void AddPorts()
    {
        base.AddPorts();

        //AddOutputEventPort("OnOptionASelected");
        //AddOutputEventPort("OnOptionBSelected");
        //AddOutputEventPort("OnOptionCSelected");

        AddInputPort("Previous");

        AddOutputPort("OptionA");
        AddOutputPort("OptionB");
        AddOutputPort("OptionC");
    }
}