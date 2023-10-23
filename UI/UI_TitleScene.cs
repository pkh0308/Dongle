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
        OptionBtn,
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

        InitBtns();

        Managers.Game.VersionCheck();
        Destroy(GameObject.Find(ConstVal.LOADING_SCENE));
        Managers.Sound.PlayBgm((int)SoundManager.Bgms.Bgm_Main);
        return true;
    }

    void InitBtns()
    {
        GetButton((int)Buttons.OptionBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Option);
        GetButton((int)Buttons.StartBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Start);
        GetButton((int)Buttons.RankBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Rank);
        GetButton((int)Buttons.ExitBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Exit);
    }
    #endregion

    #region Button Handler
    void Btn_Option()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
        Managers.UI.OpenPopUp<UI_OptionPopUp_Title>();
    }

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
        Managers.Game.ExitGame();
    }
    #endregion

    #region Input Action
    void OnEscape()
    {
        if(Managers.UI.ClosePopUp())
            Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
    }
    #endregion
}