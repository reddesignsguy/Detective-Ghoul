using UnityEngine;
using UnityEngine.UI;

public class FOrbit : MonoBehaviour
{
    public RectTransform fText;
    public RectTransform center;
    public float radius = 100f;
    public float speed = 100f;

    private float angle = 0f;
    public float baseSize = 0.0001f;

    void Update()
    {
        angle -= speed * Time.deltaTime;

        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        fText.anchoredPosition = new Vector2(x, y) + center.anchoredPosition;


        // Calculate the distance from the camera
        float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

        // Scale the text to maintain the same size regardless of distance
        float scaleFactor = distance * 0.01f; // Adjust the scale factor as needed
        transform.localScale = new Vector3(baseSize, baseSize, baseSize) * scaleFactor;
    }
}
