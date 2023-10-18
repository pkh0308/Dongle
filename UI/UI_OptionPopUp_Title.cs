using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SliderEvent = UnityEngine.UI.Slider.SliderEvent;

public class UI_OptionPopUp_Title : UI_PopUp
{
    #region Enums & Variables
    enum Texts
    {
        HeadText,
        BgmText,
        SfxText,
        ExitBtnText
    }

    enum Images
    {
        BgmSliderHandle,
        SfxSliderHandle
    }

    enum Buttons
    {
        ExitBtn
    }

    enum Sliders
    {
        BgmSlider,
        SfxSlider
    }
    Slider _bgmSlider;
    Slider _sfxSlider;
    #endregion

    #region Initiailize
    protected override bool Init()
    {
        BindTexts(typeof(Texts));
        BindIamags(typeof(Images));
        BindButtons(typeof(Buttons));
        BindSlider(typeof(Sliders));

        InitSlider();

        GetButton((int)Buttons.ExitBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Exit);

        return true;
    }

    void InitSlider()
    {
        _bgmSlider = GetSlider((int)Sliders.BgmSlider);
        _sfxSlider = GetSlider((int)Sliders.SfxSlider);
        _bgmSlider.value = Managers.Sound.BgmVol;
        _sfxSlider.value = Managers.Sound.SfxVol;
        _bgmSlider.onValueChanged = OnSlideBgm();
        _sfxSlider.onValueChanged = OnSlideSfx();
    }
    #endregion

    #region Button Handler
    void Btn_Exit()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
        Managers.UI.ClosePopUp();
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