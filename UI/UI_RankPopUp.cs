using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class UI_RankPopUp : UI_PopUp
{
    #region Enums & Variables
    enum Texts
    {
        HeadText,
        TodayRankText,
        TodayRankText_0,
        TodayRankText_1,
        TodayRankText_2,
        TodayRankText_3,
        TodayRankText_4,
        TodayRankText_5,
        TodayRankText_6,
        MyRankText,
        MyRankText_0,
        MyRankText_1,
        MyRankText_2,
        MyRankText_3,
        MyRankText_4,
        MyRankText_5,
        MyRankText_6,
        AllRankText,
        AllRankText_0,
        AllRankText_1,
        AllRankText_2,
        AllRankText_3,
        AllRankText_4,
        AllRankText_5,
        AllRankText_6,
        LoadingText
    }
    TextMeshProUGUI[] _todayRanks;
    TextMeshProUGUI[] _myRanks;
    TextMeshProUGUI[] _allRanks;

    enum Images
    {
        Today,
        TodayRank_0,
        TodayRank_1,
        TodayRank_2,
        TodayRank_3,
        TodayRank_4,
        TodayRank_5,
        TodayRank_6,
        Mine,
        MyRank_0,
        MyRank_1,
        MyRank_2,
        MyRank_3,
        MyRank_4,
        MyRank_5,
        MyRank_6,
        All,
        AllRank_0,
        AllRank_1,
        AllRank_2,
        AllRank_3,
        AllRank_4,
        AllRank_5,
        AllRank_6,
        LoadingScreen
    }
    Image _loadingScreen;

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

        _loadingScreen = GetImage((int)Images.LoadingScreen);

        GetButton((int)Buttons.ExitBtn).gameObject.AddComponent<UI_Base>().BindEvent(Btn_Exit);

        InitRankTexts();

        return true;
    }

    void InitRankTexts()
    {
        // 텍스트 배열 초기화
        _todayRanks = new TextMeshProUGUI[ConstVal.RANK_CAPACITY_TITLE];
        for(int i = 0; i < _todayRanks.Length; i++)
            _todayRanks[i] = GetText((int)Texts.TodayRankText_0 + i);

        _myRanks = new TextMeshProUGUI[ConstVal.RANK_CAPACITY_TITLE];
        for (int i = 0; i < _myRanks.Length; i++)
            _myRanks[i] = GetText((int)Texts.MyRankText_0 + i);

        _allRanks = new TextMeshProUGUI[ConstVal.RANK_CAPACITY_TITLE];
        for (int i = 0; i < _allRanks.Length; i++)
            _allRanks[i] = GetText((int)Texts.AllRankText_0 + i);

        // 오늘의 랭킹 갱신
        List<int> today = Managers.DB.GetTodayScores(ConstVal.RANK_CAPACITY_TITLE);
        for (int i = 0; i < today.Count; i++)
            _todayRanks[i].text = today[i] > 0 ? string.Format("{0:000000}", today[i]) : ConstVal.ZERO_SCORE;
        for (int i = today.Count; i < _todayRanks.Length; i++)
            _todayRanks[i].text = ConstVal.ZERO_SCORE;

        // 나의 랭킹 갱신
        List<int> mine = Managers.DB.GetMyScores(ConstVal.RANK_CAPACITY_TITLE);
        for (int i = 0; i < mine.Count; i++)
            _myRanks[i].text = mine[i] > 0 ? string.Format("{0:000000}", mine[i]) : ConstVal.ZERO_SCORE;
        for (int i = mine.Count; i < _myRanks.Length; i++)
            _myRanks[i].text = ConstVal.ZERO_SCORE;

        // 전체 랭킹 갱신
        List<int> all = Managers.DB.GetAllScores(ConstVal.RANK_CAPACITY_TITLE);
        for (int i = 0; i < all.Count; i++)
            _allRanks[i].text = all[i] > 0 ? string.Format("{0:000000}", all[i]) : ConstVal.ZERO_SCORE;
        for (int i = all.Count; i < _allRanks.Length; i++)
            _allRanks[i].text = ConstVal.ZERO_SCORE;

        // 로딩 화면 해제
        _loadingScreen.gameObject.SetActive(false);
    }
    #endregion

    #region Button Handler
    void Btn_Exit()
    {
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Button);
        ClosePopUp();
    }
    #endregion
}