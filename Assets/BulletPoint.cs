using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletPoint : MonoBehaviour
{
    public Sprite on;
    public Sprite off;
    private Image image;

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();   
    }

    public void SetFill(bool fill)
    {
        if (fill)
        {
            image.sprite = on;
        }
        else
        {
            image.sprite = off;
        }
    }
}
