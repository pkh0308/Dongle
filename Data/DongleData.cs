using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DongleData
{
    public int Level;
    public string Name;
    public int Score;
}

[Serializable]
public class DongleDataLoader : ILoader<int, DongleData>
{
    public List<DongleData> DongleDatas = new List<DongleData>();

    public Dictionary<int, DongleData> MakeDic()
    {
        Dictionary<int, DongleData> dic = new Dictionary<int, DongleData>();

        foreach(DongleData data in DongleDatas)
            dic.Add(data.Level, data);
        return dic;
    }
}