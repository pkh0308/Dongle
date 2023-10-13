using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WfsManager 
{
    public void Init() { }

    Dictionary<float, WaitForSeconds> _seconds = new Dictionary<float, WaitForSeconds>();
    public WaitForSeconds GetSecond(float sec)
    {
        // �ش� �ð��� WaitForSeconds�� ���ٸ� �߰�
        if (_seconds.TryGetValue(sec, out WaitForSeconds value) == false)
            _seconds.Add(sec, new WaitForSeconds(sec));
        return _seconds[sec];
    }
}