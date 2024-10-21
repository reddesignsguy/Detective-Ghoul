
using System.Collections.Generic;


[System.Serializable]
public class Dialogue 
{
    public int id ;
    public string DialogueText ;
    public List<Option> options ;
}

[System.Serializable]
public class Option{
    public int id ;
    public string OptionText;
    public int NextDialogueIndex;
}
