using System.Collections;
using System.Collections.Generic;
using DS.Data;
using DS.Enumerations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PicturesPageUI : UIManager
{
    public VerticalLayoutGroup layoutGroup;

    public TextMeshProUGUI numPlaceholder;

    public void setUp(List<Sprite> images, int pageNum)
    {
        // Get page of  book
        numPlaceholder.text = "Page " + (pageNum + 1).ToString();

        // Insert images
        List<Image> imagePlaceholders = new List<Image>(layoutGroup.gameObject.GetComponentsInChildren<Image>());

        // Disable placeholders
        foreach (Image placeholder in imagePlaceholders)
        {
            placeholder.enabled = false;
        }

        // Enable placeholders as needed
        IEnumerator<Image> it = imagePlaceholders.GetEnumerator();
        foreach (Sprite sprite in images)
        {
            if (it.MoveNext())
            {
                it.Current.sprite = sprite;
                it.Current.enabled = true;
            }
        }
    }
}
