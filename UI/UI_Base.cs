using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Object = UnityEngine.Object;

public class UI_Base : MonoBehaviour
{
    public enum BtnEvent
    {
        OnClicked,
        OnClickUp,
        OnClickDown,
        OnPressed
    }

    Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();

    void Start()
    {
        Init();
    }

    protected bool _initialized;
    protected virtual bool Init()
    {
        if (_initialized)
            return false;

        _initialized = true;
        return true;
    }

    #region Bind Object
    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];
        _objects.Add(typeof(T), objects);

        for(int i = 0; i < names.Length; i++)
        {
            T child = Custom.FindChild<T>(gameObject, names[i]);
            objects[i] = child;

            if (child == null)
                Debug.Log($"### Bind Error: {names[i]}");
        }
    }

    protected void BindTexts(Type type) { Bind<TextMeshProUGUI>(type); }
    protected void BindIamags(Type type) { Bind<Image>(type); }
    protected void BindButtons(Type type) { Bind<Button>(type); }
    protected void BindSlider(Type type) { Bind<Slider>(type); }
    #endregion

    #region Get Object
    protected T Get<T>(int idx) where T : Object
    {
        Object[] objs = null;
        if(_objects.TryGetValue(typeof(T), out objs) == false)
            Debug.Log($"### Get Error - Type: {typeof(T)}, idx: {idx}");

        return objs[idx] as T;
    }

    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Slider GetSlider(int idx) { return Get<Slider>(idx); }
    #endregion

    #region Bind Event 
    public void BindEvent(Action callback, BtnEvent btnEvent = BtnEvent.OnClicked)
    {
        UI_EventHandler handler = gameObject.AddComponent<UI_EventHandler>();
        switch(btnEvent)
        {
            case BtnEvent.OnClicked:
                handler.OnClicked += callback;
                break;
            case BtnEvent.OnClickUp:
                handler.OnClickUp += callback;
                break;
            case BtnEvent.OnClickDown:
                handler.OnClickDown += callback;
                break;
            case BtnEvent.OnPressed:
                handler.OnPressed += callback;
                break;
        }
    } 
    #endregion
}