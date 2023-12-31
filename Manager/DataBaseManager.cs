using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using UnityEngine;

public class DataBaseManager
{
    const string DB_CONNECTION = "Data Source=192.168.0.2,1433;Initial Catalog=DongleDB;User ID=pkh0308;Password=db79park";
    SqlConnection connection;

    const string USER_DATA_PATH = "/UserData.json";
    public UserData CurUserData { get; private set; }

    #region Initialize
    public void Init()
    {
        connection = new SqlConnection(DB_CONNECTION);
        LoadUserId();
    }

    void LoadUserId()
    {
        string path = Application.persistentDataPath + USER_DATA_PATH;
        if(File.Exists(path) == false)
        {
            CreateUserData(path);
            return;
        }

        string userData = File.ReadAllText(path);
        CurUserData = JsonUtility.FromJson<UserData>(userData);
    }

    void CreateUserData(string path)
    {
        // DB 연결
        connection.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;

        // 쿼리 작성 및 실행
        cmd.CommandText = $"SELECT TOP 1 userId FROM ScoreDatas ORDER BY userId DESC";
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "ScoreDatas");

        DataTable table = ds.Tables[0];
        int count = table.Rows.Count;
        // 가장 높은 userId + 1, 데이터가 없다면 이니셜 값 사용 
        int userId = count > 0 ? Convert.ToInt32(table.Rows[0]["userId"]) + 1 : ConstVal.INITIAL_USER_ID;
        CurUserData = new UserData() { UserId = userId, BgmVolume = 1.0f, SfxVolume = 1.0f };
        // 연결 해제
        connection.Close();

        // 신규 UserData 생성
        string userDataStr = JsonUtility.ToJson(CurUserData);

        FileStream stream = new FileStream(path, FileMode.Create);
        byte[] datas = Encoding.UTF8.GetBytes(userDataStr);
        stream.Write(datas, 0, datas.Length);
        stream.Close();

        // 더미데이터 기록(userId 중복 생성 방지용)
        AddScoreToRank(0);
    }

    public bool GetGameVer()
    {
        // DB 연결
        connection.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;

        // 쿼리 작성 및 실행
        cmd.CommandText = $"SELECT TOP 1 GameVersion FROM GameData";
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "GameData");

        DataTable table = ds.Tables[0];
        if (table.Rows.Count <= 0)
            return false;
        bool result = Convert.ToDouble(table.Rows[0]["GameVersion"]) == double.Parse(Application.version);
        connection.Close();
        return result;
    }
    #endregion

    #region Add Score
    public void AddScoreToRank(int score)
    {
        // DB 연결
        connection.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;

        // 쿼리 작성 및 실행(트랜잭션 테스트)
        cmd.CommandText = $"BEGIN TRY BEGIN TRAN INSERT INTO ScoreDatas VALUES(@userId, @score, @recordedTime); COMMIT END TRY BEGIN CATCH ROLLBACK; END CATCH";
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        cmd.Parameters.Add("@userId", SqlDbType.Int).Value = CurUserData.UserId;
        cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
        cmd.Parameters.Add("@recordedTime", SqlDbType.DateTime).Value = date;
        cmd.ExecuteNonQuery();

        // 연결 해제
        connection.Close();
    }
    #endregion

    #region Get Score
    enum RankType
    {
        Mine,
        Today,
        All
    }

    List<int> GetScores(int capacity, RankType type)
    {
        List<int> scoreList = new List<int>(capacity);

        // DB 연결
        connection.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;

        // 검색 쿼리 실행
        switch(type)
        {
            case RankType.Mine:
                cmd.CommandText = $"SELECT TOP {capacity} score FROM ScoreDatas WHERE userID = {CurUserData.UserId} ORDER BY score DESC";
                break;
            case RankType.Today:
                cmd.CommandText = $"SELECT TOP {capacity} score FROM ScoreDatas WHERE DATEDIFF(dd, recordedTime, GETDATE()) = 0 ORDER BY score DESC";
                break;
            case RankType.All:
                cmd.CommandText = $"SELECT TOP {capacity} score FROM ScoreDatas ORDER BY score DESC";
                break;
        }
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "ScoreDatas");

        DataTable table = ds.Tables[0];
        int count = table.Rows.Count;
        for (int i = 0; i < count; i++)
            scoreList.Add(Convert.ToInt32(table.Rows[i]["score"]));

        // 연결 해제
        connection.Close();
        return scoreList;
    }

    public List<int> GetMyScores(int capacity) { return GetScores(capacity, RankType.Mine); }
    public List<int> GetTodayScores(int capacity) { return GetScores(capacity, RankType.Today); }
    public List<int> GetAllScores(int capacity) { return GetScores(capacity, RankType.All); }
    #endregion

    #region Save
    public void SaveUserData()
    {
        // 볼륨 옵션 저장
        CurUserData.BgmVolume = Managers.Sound.BgmVol;
        CurUserData.SfxVolume = Managers.Sound.SfxVol;

        string path = Application.persistentDataPath + USER_DATA_PATH;
        string userDataStr = JsonUtility.ToJson(CurUserData);

        FileStream stream = new FileStream(path, FileMode.Create);
        byte[] datas = Encoding.UTF8.GetBytes(userDataStr);
        stream.Write(datas, 0, datas.Length);
        stream.Close();
    }
    #endregion
}