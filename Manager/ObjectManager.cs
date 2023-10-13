using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 오브젝트 풀링용 매니저 클래스
public class ObjectManager 
{
    public void Init() { }

    #region Make Obj
    Dictionary<string, GameObject[]> _objs = new Dictionary<string, GameObject[]>();

    Dictionary<string, int> _loadCounts = new Dictionary<string, int>();
    Action _onLoadComplete;
    public void SetCompleteCallBack(Action callback) { _onLoadComplete = callback; }

    // 컴포넌트 추가 X
    public void MakeObj(string name, int capacity, Transform parent = null)
    {
        _loadCounts.Add(name, 0);

        GameObject[] arr = new GameObject[capacity];
        for (int i = 0; i < capacity; i++)
        {
            Managers.Resc.InstantiateByIdx(name, i, parent, (obj, idx) =>
            {
                arr[idx] = obj;
                obj.SetActive(false);
                LoadCount(name, arr);
            });
        }
    }
    // 컴포넌트 추가 O
    public void MakeObj<T>(string name, int capacity, Transform parent = null) where T : Component
    {
        _loadCounts.Add(name, 0);

        GameObject[] arr = new GameObject[capacity];
        for(int i = 0; i < capacity;i++)
        {
            Managers.Resc.InstantiateByIdx(name, i, parent, (obj, idx) =>
            {
                arr[idx] = obj;
                obj.SetActive(false);
                obj.AddComponent<T>();
                LoadCount(name, arr);
            });
        }
    }
    void LoadCount(string name, GameObject[] arr)
    {
        _loadCounts[name]++;

        if (_loadCounts[name] < arr.Length)
            return;

        _objs.Add(name, arr);
        _loadCounts.Remove(name);

        if (_loadCounts.Count == 0)
        {
            _onLoadComplete?.Invoke();
            _onLoadComplete = null;
        }
    }
    #endregion

    #region Get & Clear
    public GameObject GetObj(string name)
    {
        if( _objs.TryGetValue(name, out GameObject[] objs) == false)
        {
            Debug.Log($"### Wrong obj name: {name}");
            return null;
        }

        for(int i = 0; i < objs.Length; i++)
        {
            if (objs[i].activeSelf)
                continue;

            objs[i].SetActive(true);
            return objs[i];
        }
        Debug.Log($"### Object capacity is over: {objs.Length}");
        return null;
    }

    public void DisableObjs()
    {
        List<string> keys = _objs.Keys.ToList();

        for(int i = 0; i < keys.Count;i++)
        {
            GameObject[] arr = _objs[keys[i]];
            for(int j = 0; j< arr.Length; j++)
                arr[j].SetActive(false);
        }
    }

    public void ClearObjs()
    {
        List<string> keys = _objs.Keys.ToList();

        for (int i = 0; i < keys.Count; i++)
        {
            GameObject[] arr = _objs[keys[i]];
            for (int j = 0; j < arr.Length; j++)
                UnityEngine.Object.Destroy(arr[j]);
        }
    }
    #endregion
}