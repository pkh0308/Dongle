using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UI_MainScene : UI_Base
{
    #region Enums & Variables
    enum Texts
    {
        ScoreHeadText,
        ScoreText,
        RankHeadText,
        FirstScoreText,
        SecondScoreText,
        ThirdScoreText,
        NextText,
        EvolutionText
    }
    TextMeshProUGUI _scoreText;
    TextMeshProUGUI _rankHeadText;
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
        EvolutionTree,
        EndLogo
    }
    Image _nextDongleImg;
    Image _endLogo;

    enum Buttons
    {
        ToTodayRankBtn,
        ToMyRankBtn
    }
    Button _toTodayRankBtn;
    Button _toMyRankBtn;

    enum Ranks
    {
        Today,
        My
    }
    Ranks _curRank;
    List<int> _todayRanks;
    List<int> _myRanks;
    #endregion

    #region Initiailize
    protected override bool Init()
    {
        BindTexts(typeof(Texts));
        BindIamags(typeof(Images));
        BindButtons(typeof(Buttons));

        InitTexts();
        InitImgs();
        InitBtns();

        Destroy(GameObject.Find(ConstVal.LOADING_SCENE));
        return true;
    }

    void InitTexts()
    {
        _scoreText = GetText((int)Texts.ScoreText);
        _rankHeadText = GetText((int)Texts.RankHeadText);

        _rankScoreTexts = new TextMeshProUGUI[ConstVal.RANK_CAPACITY_MAIN];
        for (int i = 0; i < _rankScoreTexts.Length; i++)
            _rankScoreTexts[i] = GetText((int)Texts.FirstScoreText + i);

        LoadScores();
        UpdateRankScores();
    }

    void InitBtns()
    {
        _toTodayRankBtn = GetButton((int)Buttons.ToTodayRankBtn);
        _toTodayRankBtn.gameObject.AddComponent<UI_Base>().BindEvent(Btn_ToTodayRank);
        _toMyRankBtn = GetButton((int)Buttons.ToMyRankBtn);
        _toMyRankBtn.gameObject.AddComponent<UI_Base>().BindEvent(Btn_ToMyRank);
        // √ ±‚∞™¿∫ ø¿¥√¿« ∑©≈∑
        _toTodayRankBtn.gameObject.SetActive(false);
    }

    void InitImgs()
    {
        _nextDongleImg = GetImage((int)Images.NextDongle);
        _endLogo = GetImage((int)Images.EndLogo);
    }
    #endregion

    #region Button Handler
    void Btn_ToTodayRank()
    {
        if (_curRank == Ranks.Today || Managers.Game.Paused)
            return;

        _curRank = Ranks.Today;
        _rankHeadText.text = "ø¿¥√¿« ∑©≈∑";
        _toTodayRankBtn.gameObject.SetActive(false);
        _toMyRankBtn.gameObject.SetActive(true);
        UpdateRankScores();
    }

    void Btn_ToMyRank()
    {
        if (_curRank == Ranks.My || Managers.Game.Paused)
            return;

        _curRank = Ranks.My;
        _rankHeadText.text = "≥™¿« ∑©≈∑";
        _toTodayRankBtn.gameObject.SetActive(true);
        _toMyRankBtn.gameObject.SetActive(false);
        UpdateRankScores();
    }
    #endregion

    #region UI Update
    void Update()
    {
        UpdateScoreText();
        UpdateNextDongle();
        CheckGameOver();
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

    bool _gameOvered;
    void CheckGameOver()
    {
        if(_gameOvered == Managers.Game.IsGameOver) 
            return;

        _gameOvered = Managers.Game.IsGameOver;
        if (_gameOvered)
            StartCoroutine(GameOver());
    }

    const float ENDLOGO_UPPER_POS = 550;
    const float ENDLOGO_DOWN_POS = -550;
    IEnumerator GameOver()
    {
        LoadScores();
        UpdateRankScores();

        float count = 1;
        while (_endLogo.rectTransform.anchoredPosition.y > 0)
        {
            _endLogo.rectTransform.anchoredPosition = _endLogo.rectTransform.anchoredPosition + (Vector2.down * Mathf.Log(count) * 2.0f);
            count++;
            yield return null;
        }
        count = 0;
        while(_endLogo.rectTransform.anchoredPosition.y > ENDLOGO_DOWN_POS)
        {
            _endLogo.rectTransform.anchoredPosition = _endLogo.rectTransform.anchoredPosition + (Vector2.down * Mathf.Pow(count, 2) * 0.5f);
            count += 0.1f;
            yield return null;
        }
        _endLogo.rectTransform.anchoredPosition = Vector2.up * ENDLOGO_UPPER_POS;
        Managers.UI.OpenPopUp<UI_GameOverPopUp>();
    }
    #endregion

    #region Rank
    void LoadScores()
    {
        _todayRanks = Managers.DB.GetTodayScores(ConstVal.RANK_CAPACITY_MAIN);
        _myRanks = Managers.DB.GetMyScores(ConstVal.RANK_CAPACITY_MAIN);
    }

    void UpdateRankScores()
    {
        List<int> list = _curRank == Ranks.Today ? _todayRanks : _myRanks;

        // ∑©≈∑ µ•¿Ã≈Õ∑Œ ∞ªΩ≈
        for (int i = 0; i < list.Count; i++)
            _rankScoreTexts[i].text = list[i] > 0 ? string.Format("{0:000000}", list[i]) : ConstVal.ZERO_SCORE;
        // µ•¿Ã≈Õ∞° æ¯∞≈≥™ 0¿Ã∏È ∫Ûƒ≠ √≥∏Æ
        for (int i = list.Count; i < _rankScoreTexts.Length; i++)
            _rankScoreTexts[i].text = ConstVal.ZERO_SCORE;
    }
    #endregion

    #region Input Action
    void OnLeft()
    {
        if (_toTodayRankBtn.gameObject.activeSelf == false)
            return;
        Btn_ToTodayRank();
    }

    void OnRight()
    {
        if (_toMyRankBtn.gameObject.activeSelf == false)
            return;
        Btn_ToMyRank();
    }

    void OnEscape()
    {
        if (Managers.Game.IsGameOver)
            return;

        if (Managers.Game.Paused == false)
        {
            Managers.Game.PauseOn();
            Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
            Managers.UI.OpenPopUp<UI_OptionPopUp>();
        }
        else
        {
            Managers.Game.PauseOff();
            Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
            Managers.UI.ClosePopUp(true);
        }
    }
    #endregion
}