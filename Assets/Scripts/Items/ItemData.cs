using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public string _name;
    public string description;
}
