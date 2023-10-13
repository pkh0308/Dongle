using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameOverPopUp : UI_PopUp
{
    #region Enums & Variables
    enum Texts
    {
        HeadText,
        ScoreText,
        ContinueBtnText,
        ToTitleBtnText
    }
    TextMeshProUGUI _scoreText;

    enum Images
    {
        Background,
        HeadRibbon
    }

    enum Buttons
    {
        ContinueBtn,
        ToTitleBtn
    }
    #endregion

    #region Initiailize
    protected override bool Init()
    {
        BindTexts(typeof(Texts));
        BindIamags(typeof(Images));
        BindButtons(typeof(Buttons));

        _scoreText = GetText((int)Texts.ScoreText);
        _scoreText.text = string.Format("È¹µæ ½ºÄÚ¾î: {0:000000}", Managers.Game.CurScore);

        GetButton((int)Buttons.ContinueBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Continue);
        GetButton((int)Buttons.ToTitleBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_ToTitle);

        return true;
    }
    #endregion

    #region Button Handler
    void Btn_Continue()
    {
        Managers.Game.Restart();
        ClosePopUp();
    }

    void Btn_ToTitle()
    {
        
        ClosePopUp();
    }
    #endregion
}