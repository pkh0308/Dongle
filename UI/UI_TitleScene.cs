using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_TitleScene : UI_Base
{
    #region Enums & Variables
    enum Texts
    {
        StartBtnText,
        RankBtnText,
        ExitBtnText
    }

    enum Images
    {
        Backgorund,
        Title
    }

    enum Buttons
    {
        StartBtn,
        RankBtn,
        ExitBtn
    }
    #endregion

    #region Initiailize
    protected override bool Init()
    {
        BindTexts(typeof(Texts));
        BindIamags(typeof(Images));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.StartBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Start);
        GetButton((int)Buttons.RankBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Rank);
        GetButton((int)Buttons.ExitBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Exit);

        Destroy(GameObject.Find(ConstVal.LOADING_SCENE));
        Managers.Sound.PlayBgm((int)SoundManager.Bgms.Bgm_Main);
        return true;
    }
    #endregion

    #region Button Handler
    void Btn_Start()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
        Managers.Game.GameStart();
    }

    void Btn_Rank()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
        Managers.UI.OpenPopUp<UI_RankPopUp>();
    }

    void Btn_Exit()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    #endregion

    #region Input Action
    void OnEscape()
    {
        Managers.UI.ClosePopUp();
    }
    #endregion
}