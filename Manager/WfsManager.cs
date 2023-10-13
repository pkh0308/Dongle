using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WfsManager 
{
    public void Init() { }

    Dictionary<float, WaitForSeconds> _seconds = new Dictionary<float, WaitForSeconds>();
    public WaitForSeconds GetSecond(float sec)
    {
        // 해당 시간의 WaitForSeconds가 없다면 추가
        if (_seconds.TryGetValue(sec, out WaitForSeconds value) == false)
            _seconds.Add(sec, new WaitForSeconds(sec));
        return _seconds[sec];
    }
}