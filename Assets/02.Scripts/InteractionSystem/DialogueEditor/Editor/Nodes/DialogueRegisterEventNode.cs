using Newtonsoft.Json;
using UnityEngine.UIElements;

public class DialogueRegisterEventNode : BaseNode
{
    class AdditionalData
    {
        public string eventName;
    }

    public string eventName;

    private TextField eventTextField;

    public override void AddSettings()
    {
        eventTextField = new TextField("Event Name");
        eventTextField.RegisterValueChangedCallback(evt =>
        {
            eventName = evt.newValue;
        });

        settingsContainer.Add(eventTextField);
    }

    //public override string CreateAdditionalData()
    //{
    //    return JsonConvert.SerializeObject(new AdditionalData
    //    {
    //        eventName = eventName,
    //    });
    //}
    //public override void PopulateAdditionalData(string JSON)
    //{
    //    var data = JsonConvert.DeserializeObject<AdditionalData>(JSON);

    //    eventTextField.value = data.eventName;
    //}

    public override void AddPorts()
    {
        base.AddPorts();

        // add ports
        //AddInputEventPort("On Event");
    }
}
