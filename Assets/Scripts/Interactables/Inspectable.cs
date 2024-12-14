using UnityEngine;

public interface Inspectable
{
    InspectableInfo GetInfo();
    void HandlePressedKeycode(KeyCode code);
}

public struct InspectableInfo
{
    public Sprite Image { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Controls Controls { get; private set; }

    public InspectableInfo(Sprite image, string name, string description, Controls controls)
    {
        Image = image;
        Name = name;
        Description = description;
        Controls = controls;
    }
}