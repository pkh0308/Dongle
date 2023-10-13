using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action OnClicked;
    public Action OnClickUp;
    public Action OnClickDown;
    public Action OnPressed;

    bool _isPressed;

    void Update()
    {
        if (_isPressed)
            OnPressed?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClicked?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        OnClickUp?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        OnClickDown?.Invoke();
    }
}
