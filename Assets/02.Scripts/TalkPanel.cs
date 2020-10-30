using UnityEngine;

public class TalkPanel : MonoBehaviour
{
    public static TalkPanel Instance;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
