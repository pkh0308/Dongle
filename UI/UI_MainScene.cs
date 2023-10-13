using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainScene : UI_Base
{
    #region Enums & Variables
    enum Texts
    {
        ScoreHeadText,
        ScoreText,
        RankingHeadText,
        FirstScoreText,
        SecondScoreText,
        ThirdScoreText,
        NextText,
        EvolutionText
    }
    TextMeshProUGUI _scoreText;
    TextMeshProUGUI[] _rankScoreTexts;

    enum Images
    {
        ScoreBoard,
        RankingBoard,
        RankFirst,
        FirstMedal,
        RankSecond,
        SecondMedal,
        RankThird,
        ThirdMedal,
        NextBoard,
        NextDongle,
        EvolutionBoard,
        EvolutionTree
    }
    Image _nextDongleImg;

    enum Buttons
    {
        ToTodayRankBtn,
        ToMyRankBtn
    }
    #endregion

    #region Initiailize
    protected override bool Init()
    {
        BindTexts(typeof(Texts));
        BindIamags(typeof(Images));
        BindButtons(typeof(Buttons));

        _scoreText = GetText((int)Texts.ScoreText);
        _rankScoreTexts = new TextMeshProUGUI[3];
        _rankScoreTexts[0] = GetText((int)Texts.FirstScoreText);
        _rankScoreTexts[1] = GetText((int)Texts.SecondScoreText);
        _rankScoreTexts[2] = GetText((int)Texts.ThirdScoreText);

        _nextDongleImg = GetImage((int)Images.NextDongle);

        GetButton((int)Buttons.ToTodayRankBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_ToTodayRank);
        GetButton((int)Buttons.ToMyRankBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_ToMyRank);

        Managers.Game.GameStart();
        return true;
    }
    #endregion

    #region Button Handler
    void Btn_ToTodayRank()
    {

    }

    void Btn_ToMyRank()
    {

    }
    #endregion

    #region UI Update
    void Update()
    {
        UpdateScoreText();
        UpdateNextDongle();
    }

    int _befScore;
    void UpdateScoreText()
    {
        if (Managers.Game.CurScore == _befScore)
            return;

        _scoreText.text = string.Format("{0:000000}", Managers.Game.CurScore);
        _befScore = Managers.Game.CurScore;
    }

    int _befLevel;
    void UpdateNextDongle()
    {
        if (Managers.Game.NextDongle == null)
            return;

        if (_befLevel == Managers.Game.NextDongle.Level)
            return;

        string name = "Sprite_" + Managers.Game.NextDongle.Name;
        Managers.Resc.Load<Sprite>(name, (sp) =>
        {
            _nextDongleImg.sprite = sp;
            _befLevel = Managers.Game.NextDongle.Level;
        });
    }
    #endregion
}