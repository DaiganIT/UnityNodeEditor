using Newtonsoft.Json;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    private TextField npcNameTextField;
    private TextField messageTextField;

    public override void AddSettings()
    {
        npcNameTextField = new TextField("NPC Name");
        npcNameTextField.name = "npcName";
        npcNameTextField.SetValueWithoutNotify("");
        var messageLabel = new Label("Message");
        messageTextField = new TextField();
        messageTextField.name = "message";
        messageTextField.multiline = true;
        messageTextField.SetValueWithoutNotify("");

        settingsContainer.Add(npcNameTextField);
        settingsContainer.Add(messageLabel);
        settingsContainer.Add(messageTextField);
    }

    #region Additional Data
    class AdditionalData
    {
        public string npcName;
        public string message;
    }

    public override string CreateAdditionalData()
    {
        return JsonConvert.SerializeObject(new AdditionalData
        {
            npcName = npcNameTextField.value,
            message = messageTextField.value
        });
    }
    public override void PopulateAdditionalData(string JSON)
    {
        var data = JsonConvert.DeserializeObject<AdditionalData>(JSON);

        if (data == null) return;

        npcNameTextField.value = data.npcName;
        messageTextField.value = data.message;
    }
    #endregion

    public override void AddPorts()
    {
        base.AddPorts();

        AddInputPort("Previous");
        AddOutputPort("Next");
    }
}
