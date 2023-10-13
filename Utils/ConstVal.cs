using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstVal
{
    // 정수
    public const int MAX_LEVEL = 7;
    public const int CREATE_MAX_LEVEL = 3;
    public const int INITIAL_USER_ID = 1001;

    // 소수
    public const float DROP_INTERVAL = 1.0f;
    public const float DROP_LINE = 4.0f;

    // 태그
    public const string DONGLE = "Dongle";
    public const string OUTLINE = "OutLine";
    public const string WALL = "Wall";

    public enum Dongles
    {
        Dongle_0,
        Dongle_1, 
        Dongle_2, 
        Dongle_3, 
        Dongle_4, 
        Dongle_5, 
        Dongle_6, 
        Dongle_7
    }
}