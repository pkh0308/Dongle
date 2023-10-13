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
            // ������ �̸��� ������Ʈ �߰� �� ��ȯ
            if(child.name == name) 
                return child;
        }
        // �˻� ���� �� null ��ȯ
        return null;
    }
}