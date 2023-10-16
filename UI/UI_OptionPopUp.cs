using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using SliderEvent = UnityEngine.UI.Slider.SliderEvent;

public class UI_OptionPopUp : UI_PopUp
{
    #region Enums & Variables
    enum Texts
    {
        HeadText,
        BgmText,
        SfxText,
        ToTitleBtnText,
        ExitBtnText
    }

    enum Images
    {
        BgmSliderHandle,
        SfxSliderHandle
    }

    enum Buttons
    {
        ToTitleBtn,
        ExitBtn
    }

    enum Sliders
    {
        BgmSlider,
        SfxSlider
    }
    #endregion

    #region Initiailize
    protected override bool Init()
    {
        BindTexts(typeof(Texts));
        BindIamags(typeof(Images));
        BindButtons(typeof(Buttons));
        BindSlider(typeof(Sliders));

        GetSlider((int)Sliders.BgmSlider).onValueChanged = OnSlideBgm();
        GetSlider((int)Sliders.SfxSlider).onValueChanged = OnSlideSfx();

        GetButton((int)Buttons.ToTitleBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_ToTitle);
        GetButton((int)Buttons.ExitBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Exit);

        return true;
    }
    #endregion

    #region Button Handler
    void Btn_ToTitle()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
        Managers.Game.ToTitle();
    }

    void Btn_Exit()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
        Managers.UI.ClosePopUp(true);
    }
    #endregion

    #region Slider Event
    SliderEvent OnSlideBgm()
    {
        SliderEvent se = new SliderEvent();
        se.AddListener((value) => { Managers.Sound.SetBgmVolume(value); });
        return se;
    }

    SliderEvent OnSlideSfx()
    {
        SliderEvent se = new SliderEvent();
        se.AddListener((value) => { Managers.Sound.SetSfxVolume(value); });
        return se;
    }
    #endregion
}
