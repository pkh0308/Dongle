using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    Stack<UI_PopUp> _stack = new Stack<UI_PopUp>();
    Transform _baseTransform;

    public void Init() { }

    #region UI Open
    public void OpenScene<T>(Action callback = null) where T : Component
    {
        Managers.Resc.Instantiate(typeof(T).Name, null, (obj) => 
        {
            _baseTransform = obj.transform;
            obj.AddComponent<T>();
            callback?.Invoke();
        });
    }

    public void OpenPopUp<T>(string name = null, Transform parent = null, Action callback = null) where T : UI_PopUp
    {
        if (name == null)
            name = typeof(T).Name;
        if (parent == null)
            parent = _baseTransform;

        Managers.Resc.Instantiate(name, parent, (obj) =>
        {
            obj.transform.SetParent(parent);
            _stack.Push(obj.AddComponent<T>());
            callback?.Invoke();
        });
    }
    #endregion

    #region UI Close
    public void ClosePopUp(UI_PopUp popUp)
    {
        if(_stack.Count == 0) 
            return;

        if (_stack.Peek() != popUp)
        {
            Debug.Log($"### There is another PopUP: {_stack.Peek().name}");
            return;
        }

        UnityEngine.Object.Destroy(_stack.Pop().gameObject);
    }
    #endregion
}