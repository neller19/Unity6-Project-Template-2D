using UnityEngine;
using TMPro;

public class DebugMessageLine : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI titleLabel;
    [SerializeField]
    TextMeshProUGUI infoLabel;

    public void SetTitle(string title)
    {
        titleLabel.text = title;
    }
    public void SetMessage(string message)
    {
        infoLabel.text = message;
    }

    public void Return()
    {
        var db = DebugMessagePanel.Instance;
        if (db) db.ReturnDebugWriter(this);
    }
}
