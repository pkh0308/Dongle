using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;

public class GameManager
{
    #region enums & variables
    string[] _dongleNames;

    public enum GameState
    {
        Title,
        OnGame,
        GameOver
    }
    public GameState CurState { get; private set; }
    public bool IsGameOver { get { return  CurState == GameState.GameOver; } }
    public bool Paused { get; private set; }

    enum Scenes
    {
        TitleScene,
        MainScene
    }
    #endregion

    #region Initialize
    public void Init()
    {
        _dongleNames = Enum.GetNames(typeof(ConstVal.Dongles));
        LoadDongleDatas();
    }

    public Dictionary<int, DongleData> DongleDatas { get; private set; }
    const string DONGLE_DATA_PATH = "Json/DongleDatas";

    void LoadDongleDatas()
    {
        TextAsset ta = Resources.Load<TextAsset>(DONGLE_DATA_PATH);
        DongleDatas = JsonUtility.FromJson<DongleDataLoader>(ta.text).MakeDic();
    }
    #endregion

    #region GameStart
    public void GameStart()
    {
        // 메인 씬 로드
        SceneManager.LoadScene(Scenes.MainScene.ToString());
        // 로딩 전부 완료 후 실행할 내용 등록
        Managers.Obj.SetCompleteCallBack(InitGame);
        // 오브젝트 풀링
        Transform parent = new GameObject() { name = "Dongles" }.transform;
        for (int i = 0; i < _dongleNames.Length; i++)
            Managers.Obj.MakeObj<Dongle>(_dongleNames[i], 100, parent);
    }

    void InitGame()
    {
        CurState = GameState.OnGame;
        PauseOff();
        CreateNewDongle(true);
    }

    public void Restart()
    {
        Managers.Obj.DisableObjs();
        CurScore = 0;
        CurDongle = null;

        CurState = GameState.OnGame;
        PauseOff();
        Managers.Sound.PlayBgm((int)SoundManager.Bgms.Bgm_Main);
        CreateNewDongle(true);
    }
    #endregion

    #region Dongle
    public Dongle CurDongle { get; private set; }
    public DongleData NextDongle { get; private set; }

    public void CreateNewDongle(bool init = false)
    {
        int level = init ? Random.Range(0, ConstVal.CREATE_MAX_LEVEL + 1) : NextDongle.Level;
        CurDongle = Managers.Obj.GetObj(_dongleNames[level]).GetComponent<Dongle>();
        CurDongle.SetLevel(level);
        NextDongle = DongleDatas[Random.Range(0, ConstVal.CREATE_MAX_LEVEL + 1)];
    }

    public void DropDongle()
    {
        CurDongle.Drop();
        CurDongle = null;
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_Next);
    }

    public void UpagrageDongle(GameObject go1, GameObject go2, int level)
    {
        go1.SetActive(false);
        go2.SetActive(false);
        GetScore(++level);

        GameObject obj = Managers.Obj.GetObj(_dongleNames[level]);
        obj.transform.position = (go1.transform.position + go2.transform.position) / 2;
        Dongle dongle = obj.GetComponent<Dongle>();
        dongle.SetLevel(level);
        dongle.Drop();

        Managers.Sound.PlayRandomUpgradeSfx();
    }
    #endregion

    #region Score
    public int CurScore { get; private set; }
    public void GetScore(int level)
    {
        if(DongleDatas.TryGetValue(level, out DongleData data) == false)
        {
            Debug.Log($"### Wrong Dongle Level: {level}");
            return;
        }

        CurScore += data.Score;
    }
    #endregion

    #region Pause
    public void PauseOn() { Paused = true; Cursor.visible = true; }
    public void PauseOff() { Paused = false; Cursor.visible = false; }
    #endregion

    #region GameOver
    public void GameOver()
    {
        if (CurState == GameState.GameOver)
            return;

        CurState = GameState.GameOver;
        PauseOn();
        Managers.Sound.Pause();
        Managers.Sound.PlaySfx((int)SoundManager.Sfxs.Sfx_GameOver);

        // 점수 기록
        Managers.DB.AddScoreToRank(CurScore);
    }

    public void ToTitle()
    {
        CurState = GameState.Title;
        Managers.Obj.ClearObjs();
        Cursor.visible = true;
        SceneManager.LoadScene(Scenes.TitleScene.ToString());
    }
    #endregion
}