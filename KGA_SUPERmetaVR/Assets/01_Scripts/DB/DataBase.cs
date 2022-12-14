using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

using System.Security.Cryptography; // SHA512 해싱을 위함
using System.Text; // StringBuilder를 사용하기 위함

public class DataBase : SingletonBehaviour<DataBase>
{
    MySqlConnection sqlconnection = null;
    //MySqlDataReader

    [Header("AWS RDS 정보")]
    [SerializeField] string sqlDatabaseIP;
    [SerializeField] string sqlDatabaseName;
    [SerializeField] string sqlDatabaseID;
    [SerializeField] string sqlDatabasePW;

    string securityString = "뒷간"; // 솔팅을 위한 암호
    public string SecurityString { get { return securityString; } }

    void sqlConnect()
    {
        string sqlDataBase = "Server=" + sqlDatabaseIP + ";Database=" + sqlDatabaseName + ";UserId=" + sqlDatabaseID + ";Password=" + sqlDatabasePW + ";CharSet=utf8;";

        try
        {
            sqlconnection = new MySqlConnection(sqlDataBase);
            sqlconnection.Open();

            //UnityEngine.Debug.Log("<color=blue>SQL의 접속 상태 : </color>" + sqlconnection.State);
        }
        catch (Exception msg)
        {
            UnityEngine.Debug.Log(msg);
        }
    }

    void sqldisConnect()
    {
        sqlconnection.Close();
        //UnityEngine.Debug.Log("<color=red>SQL의 접속 상태 : </color>" + sqlconnection.State);
    }

    public void sqlcmdall(string allcmd)
    {
        sqlConnect();

        MySqlCommand dbcmd = new MySqlCommand(allcmd, sqlconnection); // 명령어를 커맨드에 입력
        dbcmd.ExecuteNonQuery(); // 명령어를 SQL에 보냄
        sqldisConnect();
    }

    public DataTable selsql(string sqlcmd)
    {
        DataTable dataTable = new DataTable();

        sqlConnect();
        MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmd, sqlconnection);
        adapter.Fill(dataTable); // TODO : DataReader& DataAdapter 찾아보기
        sqldisConnect();

        return dataTable;
    }

    // 암호화
    public string SHA512Hash(string data)
    {
        SHA512 sha = new SHA512Managed();
        byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in hash)
        {
            stringBuilder.AppendFormat("{0:x2}", b);
        }
        return stringBuilder.ToString();
    }

    // SQL
    public void InsertDB(string _tableName, string _column, string _data)
    {
        sqlcmdall($"INSERT INTO {_tableName} ({_column}) VALUES ({_data})");
    }

    public void UpdateDB(string _tableName, string _updateColumn, string _updateData, string _findColum, string _findData)
    {
        sqlcmdall($"UPDATE {_tableName} SET {_updateColumn} = '{_updateData}' WHERE {_findColum} = '{_findData}'");
        UpdateAt(_tableName, _findColum, _findData);
    }

    public void UpdateAt(string _tableName, string _findColum, string _findData)
    {
        sqlcmdall($"UPDATE {_tableName} SET {UserTableInfo.update_at} = NOW() WHERE {_findColum} = '{_findData}'");
    }

    // 데이터 찾기
    public DataTable FindDB(string _tableName, string _findColumn, string _checkColumn, string _checkData)
    {
        DataTable dataTable = selsql($"SELECT {_findColumn} FROM {_tableName} WHERE {_checkColumn} = '{_checkData}'");
        return dataTable;
    }

    public DataTable SortDB(string _tableName, string _findColumn, string _orderBy, string _sort = "DESC")
    {
        DataTable dataTable = selsql($"SELECT {_findColumn} FROM {_tableName} ORDER BY {_orderBy} {_sort}");
        return dataTable;
    }

    // 로그인 기능
    public bool Login(string _id, string _pw)
    {
        // 입력값 SQL Injection 방지 및 패스워드 암호화 (솔팅 + 해싱)
        string securityPW = SHA512Hash(_pw + securityString);

        DataTable dataTable = FindDB(UserTableInfo.table_name, UserTableInfo.user_pw, UserTableInfo.user_id, _id);

        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (securityPW == row[UserTableInfo.user_pw].ToString())
                {
                    UpdateAt(UserTableInfo.table_name, UserTableInfo.user_id, _id);

                    return true;
                }
                else
                {
                    LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.IncorrectPWPopupCanvas);
                    UnityEngine.Debug.Log("비밀번호가 일치하지 않습니다. 확인 후 다시 입력해주세요.");
                }
            }
        }
        else
        {
            LoginManager.Instance.SetPopupUICanvas(LoginManager.Instance.IncorrectIDPopupCanvas);
            UnityEngine.Debug.Log("사용자 정보가 없습니다");
        }
        return false;
    }

    // 회원 가입
    public void CreateUser(string _id, string _pw, string _name, string _birth, string _hint, string _hintAnswer)
    {
        if (CheckUse(UserTableInfo.user_id,_id))
        {
            // 입력값 SQL Injection 방지 및 패스워드 암호화 (솔팅 + 해싱)
            string securityPW = SHA512Hash(_pw + securityString);

            // 생성시 create_at, update_at 설정을 포함해준다.
            InsertDB(UserTableInfo.table_name, $"{UserTableInfo.user_id}, {UserTableInfo.user_pw}, {UserTableInfo.name}, {UserTableInfo.birth}, {UserTableInfo.hint}, {UserTableInfo.hint_answer}, {UserTableInfo.create_at}, {UserTableInfo.update_at}", $"'{_id}','{securityPW}','{_name}','{_birth}','{_hint}','{_hintAnswer}', NOW(), NOW()");
        }
    }

    // 아이디 중복 처리
    public bool CheckUse(string _column, string _id)
    {
        // 입력할 수 있는 아이디/닉네임인가
        
        DataTable dataTable = selsql($"SELECT {UserTableInfo.user_id} FROM {UserTableInfo.table_name} WHERE {_column} = '{_id}'");

        if (dataTable.Rows.Count == 0)
        {
            return true;
        }
        else
        {
            UnityEngine.Debug.Log("사용중인 ID입니다.");
            return false;
        }
    }

}
