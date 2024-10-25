using UnityEngine;
using UnityEngine.UI;

public class FOrbit : MonoBehaviour
{
    public RectTransform fText;
    public RectTransform center;
    public float radius = 100f;
    public float speed = 100f;

    private float angle = 0f;

    void Update()
    {
        angle -= speed * Time.deltaTime;

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        fText.anchoredPosition = new Vector2(x, y) + center.anchoredPosition;
    }
}
