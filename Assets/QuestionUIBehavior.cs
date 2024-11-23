using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionUIBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.transition == Selectable.Transition.None)
            return;

        EventsManager.instance.NotifyQuestionUIEvent(QuestionUIEvent.Entered);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.transition == Selectable.Transition.None)
            return;

        EventsManager.instance.NotifyQuestionUIEvent(QuestionUIEvent.Exited);
    }
}

public enum QuestionUIEvent
{
    Entered,
    Exited
}