using Newtonsoft.Json;
using UnityEngine.UIElements;

public class IfEventDialogueNode : BaseNode
{
    class AdditionalData
    {
        public string eventField;
    }

    public string eventField;

    private TextField eventTextField;

    public override void AddSettings()
    {
        eventTextField = new TextField("Event Name");
        eventTextField.RegisterValueChangedCallback(evt =>
        {
            eventField = evt.newValue;
        });

        settingsContainer.Add(eventTextField);
    }

    //public override string CreateAdditionalData() 
    //{
    //    return JsonConvert.SerializeObject(new AdditionalData
    //    {
    //        eventField = eventField,
    //    });
    //}
    //public override void PopulateAdditionalData(string JSON) 
    //{
    //    var data = JsonConvert.DeserializeObject<AdditionalData>(JSON);

    //    eventTextField.value = data.eventField;
    //}

    public override void AddPorts()
    {
        base.AddPorts();

        // add ports
        AddInputPort("Previous");
        AddOutputPort("True");
        AddOutputPort("False");
    }
}
