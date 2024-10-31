using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleInteractable : MonoBehaviour
{
    public float trashAnimationDuration = 5f;
    public float trashAnimationDistance;

    /* Used to detect if puzzle pieces within this puzzle are c licked*/
    public event Action<GameObject> OnPuzzleClicked;

    public void ClickPuzzle(GameObject obj)
    {
        if (OnPuzzleClicked != null)
        {
            OnPuzzleClicked(obj);
        }
    }

    private void Awake()
    {
        Button[] buttons = gameObject.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            Button capturedButton = button;
            capturedButton.onClick.AddListener(() => HandlePuzzleClicked(capturedButton.gameObject));
        }
        Debug.Log("Setting up puzzle");

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {

        Debug.Log("Limit player movement");
        EventsManager.instance.SetMovement(false);
    }

    private void OnDisable()
    {
        Debug.Log("Allow player movement");
        EventsManager.instance.SetMovement(true);
    }

    public void HandlePuzzleClicked(GameObject obj)
    {
        Debug.Log("clicked");
        if (obj.CompareTag("Trash"))
        {
            StartCoroutine(MoveTrash(obj));
        }
        else if (obj.CompareTag("Key"))
        {
            EventsManager.instance.PickupItem(obj.GetComponent<Item>());
            Destroy(obj);
            Debug.Log("key!");
        }
    }

    private IEnumerator MoveTrash(GameObject trash)
    {
        MoveOutOfMask(trash);

        // Handle translation of trash
        RectTransform rectTransform = trash.GetComponent<RectTransform>();
        Vector2 startPos, targetPos;
        GetAnimationCheckpoints2(rectTransform, out startPos, out targetPos);

        // Handle opacity of trash
        Image sp = trash.GetComponent<Image>();
        Color color = sp.color;
        float elapsed = 0f;  

        while (elapsed < trashAnimationDuration)  
        {
            elapsed += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / trashAnimationDuration);
            color.a = Mathf.Lerp(1, 0, elapsed / trashAnimationDuration);
            sp.color = color;

            yield return null;
        }

        if (trash.TryGetComponent(out Button button))
        {
            button.onClick.RemoveAllListeners();
        }

        Destroy(trash);
    }


    // Gets random direction
    private void GetAnimationCheckpoints(RectTransform rectTransform, out Vector2 startPos, out Vector2 targetPos)
    {
        float randX = UnityEngine.Random.Range(-1f, 1f);
        float randY = UnityEngine.Random.Range(-1f, 1f);
        Vector2 randDir = new Vector2(randX, randY);
        randDir = randDir.normalized;

        Vector2 displacement = randDir * trashAnimationDistance;

        startPos = rectTransform.localPosition;
        targetPos = startPos + displacement;
    }

    // Gets direction pointing outwards from the origin
    private void GetAnimationCheckpoints2(RectTransform rectTransform, out Vector2 startPos, out Vector2 targetPos)
    {
        // Get center of the puzzle
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        Vector3 origin = rt.localPosition;

        // The target of this projectile is away from the center
        Vector2 dir = (rectTransform.localPosition - origin);
        dir = dir.normalized;
        Debug.Log(dir);
        Vector2 displacement = dir * trashAnimationDistance;

        startPos = rectTransform.localPosition;
        targetPos = startPos + displacement;
    }

    private static void MoveOutOfMask(GameObject trash)
    {
        trash.transform.SetParent(trash.transform.parent.parent);
    }
}