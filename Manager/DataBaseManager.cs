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
        // DB ����
        connection.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;

        // ���� �ۼ� �� ����
        cmd.CommandText = $"SELECT TOP 1 userId FROM ScoreDatas";
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        adapter.Fill(ds, "ScoreDatas");

        DataTable table = ds.Tables[0];
        int count = table.Rows.Count;
        // ���� ���� userId + 1, �����Ͱ� ���ٸ� �̴ϼ� �� ��� 
        int userId = count > 0 ? Convert.ToInt32(table.Rows[0]["userId"]) + 1 : ConstVal.INITIAL_USER_ID;
        CurUserData = new UserData() { UserId = userId };
        // ���� ����
        connection.Close();

        // �ű� UserData ����
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
        // DB ����
        connection.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = connection;

        // ���� �ۼ� �� ����
        cmd.CommandText = $"INSERT INTO ScoreDatas VALUES(@userId, @score, @recordedTime)";
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        cmd.Parameters.Add("@userId", SqlDbType.Int).Value = CurUserData.UserId;
        cmd.Parameters.Add("@score", SqlDbType.Int).Value = score;
        cmd.Parameters.Add("@recordedTime", SqlDbType.DateTime).Value = date;
        cmd.ExecuteNonQuery();

        // ���� ����
        connection.Close();
    }
    #endregion

    #region Get Score
    public List<int> GetScoresFromRank(int capacity)
    {
        List<int> scores = new List<int>();

        // ToDo: �˻� ���� �ۼ�

        return scores;
    }
    #endregion
}