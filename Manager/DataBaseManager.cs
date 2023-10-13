using System;
using System.Collections;
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

    const string USER_DATA_PATH = "/Data/UserData.json";
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
        cmd.CommandText = $"SELECT TOP 1 userId FROM ScoreDatas";
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "ScoreDatas");

        DataTable table = ds.Tables[0];
        int count = table.Rows.Count;
        // 가장 높은 userId + 1, 데이터가 없다면 이니셜 값 사용 
        int userId = count > 0 ? Convert.ToInt32(table.Rows[0]["userId"]) + 1 : ConstVal.INITIAL_USER_ID;
        CurUserData = new UserData() { UserId = userId };
        // 연결 해제
        connection.Close();

        // 신규 UserData 생성
        string userDataStr = JsonUtility.ToJson(CurUserData);

        FileStream stream = new FileStream(path, FileMode.Create);
        byte[] datas = Encoding.UTF8.GetBytes(userDataStr);
        stream.Write(datas, 0, datas.Length);
        stream.Close();
    }
    #endregion

    #region Add Score
    public void AddScoreToRank(int score)
    {
        // DB 연결
        connection.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;

        // 쿼리 작성 및 실행
        cmd.CommandText = $"INSERT INTO ScoreDatas VALUES(@userId, @score, @recordedTime)";
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
    public List<int> GetScoresFromRank(int capacity)
    {
        List<int> scores = new List<int>();

        // ToDo: 검색 쿼리 작성

        return scores;
    }
    #endregion
}