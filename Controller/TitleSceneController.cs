using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneController : MonoBehaviour
{
    void Start()
    {
        // Ÿ��Ʋ �� UI �ε�
        Managers.UI.OpenScene<UI_TitleScene>();
    }
}
