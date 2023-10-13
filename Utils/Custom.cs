using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom 
{
    public static T FindChild<T>(GameObject obj, string name) where T : Object
    {
        if (obj == null)
            return null;

        foreach(T child in obj.GetComponentsInChildren<T>())
        {
            // 동일한 이름의 컴포넌트 발견 시 반환
            if(child.name == name) 
                return child;
        }
        // 검색 실패 시 null 반환
        return null;
    }
}