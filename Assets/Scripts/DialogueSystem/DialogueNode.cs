using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueNode : MonoBehaviour
{
    int nextNodeIndex;
    public abstract void InjectDialogueNodeUI();
}
