using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "InventoryItem")]
public class InventoryItem : ScriptableObject
{
    public Sprite image;
    public string _name;
    public string description;
    public string code;
    public string hint;
    public bool isClue = false;
}