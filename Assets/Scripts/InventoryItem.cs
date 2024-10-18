using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public Texture2D image;
    public string _name;
    public string description;
}

