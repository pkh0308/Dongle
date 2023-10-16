using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainSceneController : MonoBehaviour
{
    #region Initialize
    void Start()
    {
        // ¾À UI ¿ÀÇÂ
        Managers.UI.OpenScene<UI_MainScene>();
    }
    #endregion

    #region Update
    void Update()
    {
        MouseCheck();
    }

    Vector3 _curPos;
    float leftBorder;
    float rightBorder;
    const float BORDER_LEFT = -2.7f;
    const float BORDER_RIGHT = 2.7f;
    void MouseCheck()
    {
        if (Managers.Game.Paused || Managers.Game.CurDongle == null)
            return;

        // °æ°è ¼³Á¤
        float halfSize = Managers.Game.CurDongle.transform.localScale.x / 2.0f;
        leftBorder = BORDER_LEFT + halfSize;
        rightBorder = BORDER_RIGHT - halfSize;

        _curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _curPos.x = Mathf.Clamp(_curPos.x, leftBorder, rightBorder);
        _curPos.y = ConstVal.DROP_LINE;
        _curPos.z = 0;
        Managers.Game.CurDongle.transform.position = _curPos;

        if (Input.GetMouseButtonDown(0))
            StartCoroutine(DropDongleRoutine());
    }

    IEnumerator DropDongleRoutine()
    {
        Managers.Game.DropDongle();
        yield return Managers.Wfs.GetSecond(ConstVal.DROP_INTERVAL);

        Managers.Game.CreateNewDongle();
    }
    #endregion
}