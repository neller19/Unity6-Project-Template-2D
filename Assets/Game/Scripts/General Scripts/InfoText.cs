using UnityEngine;

/// <summary>
/// This is just a script so i can write notes on gameObjects to explain what they are supposed to do
/// will be destroyed in builds
/// </summary>
public class InfoText : MonoBehaviour
{
    [TextArea(10, 50)] public string description;

    void Awake()
    {
#if !UNITY_EDITOR
        Destroy(this);
#endif
    }
}