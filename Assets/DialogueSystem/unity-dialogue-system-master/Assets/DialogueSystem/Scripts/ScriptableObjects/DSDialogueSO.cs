using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Data;
    using Enumerations;
    using Unity.Burst.CompilerServices;

    public class DSDialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<DSDialogueChoiceData> Choices { get; set; }
        [field: SerializeField] public DSDialogueType DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }
        [field: SerializeField] public bool IsExitable { get; set; }
        [field: SerializeField] public Sprite Sprite { get; set; }
        [field: SerializeField] public bool SpriteLeftSide { get; set; }


        public void Initialize(string dialogueName, string text, List<DSDialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue, bool isExitable = false, Sprite sprite = null, bool spriteLeftSide = true)
        {
            DialogueName = dialogueName;
            Text = text;
            Choices = choices;
            DialogueType = dialogueType;
            IsStartingDialogue = isStartingDialogue;
            IsExitable = isExitable;
            Sprite = sprite;
            SpriteLeftSide = spriteLeftSide;
        }

        public void InitialiazeDisconnectedDialogue(string dialogueName, string text,  Sprite sprite = null, bool spriteLeftSide = true)
        {
            DSDialogueChoiceData nextChoice = new DSDialogueChoiceData();
            nextChoice.NextDialogue = null;

            List<DSDialogueChoiceData> choices = new List<DSDialogueChoiceData>
            {
                nextChoice
            };

            DialogueName = dialogueName;
            Text = text;
            Choices = choices;
            DialogueType = DSDialogueType.SingleChoice;
            IsStartingDialogue = true;
            IsExitable = false;
            Sprite = sprite;
            SpriteLeftSide = spriteLeftSide;
        }
    }
}