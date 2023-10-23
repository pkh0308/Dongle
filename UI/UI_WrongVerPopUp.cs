using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WrongVerPopUp : UI_PopUp
{
    #region Enums & Variables
    enum Texts
    {
        HeadText,
        ExitBtnText
    }

    enum Images
    {

    }

    enum Buttons
    {
        ExitBtn
    }
    #endregion

    #region Initiailize
    protected override bool Init()
    {
        BindTexts(typeof(Texts));
        BindIamags(typeof(Images));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.ExitBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Exit);

        return true;
    }
    #endregion

    #region Button Handler
    void Btn_Exit()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
        Managers.Game.ExitGame();
    }
    #endregion
}
