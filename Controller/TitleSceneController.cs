using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneController : MonoBehaviour
{
    void Start()
    {
        // 타이틀 씬 UI 로드
        Managers.UI.OpenScene<UI_TitleScene>();
    }
}
