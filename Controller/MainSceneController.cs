using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    void Start()
    {
        // ¾À UI ¿ÀÇÂ
        Managers.UI.OpenScene<UI_MainScene>();
    }

    void Update()
    {
        MouseCheck();
    }

    Vector3 _curPos;
    float leftBorder;
    float rightBorder;
    void MouseCheck()
    {
        if (Managers.Game.IsGameOver || Managers.Game.CurDongle == null)
            return;

        // °æ°è ¼³Á¤
        float halfSize = Managers.Game.CurDongle.transform.localScale.x / 2.0f;
        leftBorder = -2.4f + halfSize;
        rightBorder = 2.4f - halfSize;

        _curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _curPos.x = Mathf.Clamp(_curPos.x, leftBorder, rightBorder);
        _curPos.y = ConstVal.DROP_LINE;
        _curPos.z = 0;
        Managers.Game.CurDongle.transform.position = _curPos;

        if (Input.GetMouseButtonUp(0))
            StartCoroutine(DropDongleRoutine());
    }

    IEnumerator DropDongleRoutine()
    {
        Managers.Game.CurDongle.Drop();
        //yield return Managers.Wfs.GetSecond(ConstVal.DROP_INTERVAL);
        yield return null;

        Managers.Game.CreateNewDongle();
    }
}