using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

using System.Security.Cryptography; // SHA512 �ؽ��� ����
using System.Text; // StringBuilder�� ����ϱ� ����

public static class UserTableInfo
{
    public static readonly string table_name = "UserData";

    public static readonly string user_id = "user_id";
    public static readonly string user_pw = "user_pw";

    public static readonly string nickname = "nickname";
    public static readonly string coin = "coin";

    public static readonly string create_at = "create_at"; // join_date�� ����
    public static readonly string update_at = "update_at"; // �ֱ� ������ ����� ����
    public static readonly string is_delete = "is_delete";
}

public class DataBase : SingletonBehaviour<DataBase>
{
    MySqlConnection sqlconnection = null;

    [Header("AWS RDS ����")]
    [SerializeField] string sqlDatabaseIP;
    [SerializeField] string sqlDatabaseName;
    [SerializeField] string sqlDatabaseID;
    [SerializeField] string sqlDatabasePW;

    string securityString = "�ް�"; // ������ ���� ��ȣ




    void sqlConnect()
    {
        string sqlDataBase = "Server=" + sqlDatabaseIP + ";Database=" + sqlDatabaseName + ";UserId=" + sqlDatabaseID + ";Password=" + sqlDatabasePW + "";

        try
        {
            sqlconnection = new MySqlConnection(sqlDataBase);
            sqlconnection.Open();

            UnityEngine.Debug.Log("<color=blue>SQL�� ���� ���� : </color>" + sqlconnection.State);
        }
        catch (Exception msg)
        {
            UnityEngine.Debug.Log(msg);
        }
    }

    void sqldisConnect()
    {
        sqlconnection.Close();
        UnityEngine.Debug.Log("<color=red>SQL�� ���� ���� : </color>" + sqlconnection.State);
    }

    public void sqlcmdall(string allcmd)
    {
        sqlConnect();

        MySqlCommand dbcmd = new MySqlCommand(allcmd, sqlconnection); // ��ɾ Ŀ�ǵ忡 �Է�
        dbcmd.ExecuteNonQuery(); // ��ɾ SQL�� ����
        sqldisConnect();
    }

    public DataTable selsql(string sqlcmd)
    {
        DataTable dataTable = new DataTable();

        sqlConnect();
        MySqlDataAdapter adapter = new MySqlDataAdapter(sqlcmd, sqlconnection);
        adapter.Fill(dataTable); // TODO : DataReader& DataAdapter ã�ƺ���
        sqldisConnect();

        return dataTable;
    }

    // ��ȣȭ
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

    // ������ ã��
    public DataTable FindDB(string _tableName, string _findColumn, string _checkColumn, string _checkData)
    {
        DataTable dataTable = selsql($"SELECT {_findColumn} FROM {_tableName} WHERE {_checkColumn} = '{_checkData}'");
        return dataTable;
    }

    // �α��� ���
    public bool Login(string _id, string _pw)
    {
        // �Է°� SQL Injection ���� �� �н����� ��ȣȭ (���� + �ؽ�)
        string securityPW = SHA512Hash(_pw + securityString);

        DataTable dataTable = FindDB(UserTableInfo.table_name, UserTableInfo.user_pw, UserTableInfo.user_id, _id);

        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (securityPW == row[UserTableInfo.user_pw].ToString())
                {
                    UnityEngine.Debug.Log("�α��ο� �����߽��ϴ�.");
                    UpdateAt(UserTableInfo.table_name, UserTableInfo.user_id, _id);

                    return true;
                }
                else
                {
                    UnityEngine.Debug.Log("��й�ȣ�� ��ġ���� �ʽ��ϴ�. Ȯ�� �� �ٽ� �Է����ּ���.");
                }
            }
        }
        else
        {
            UnityEngine.Debug.Log("����� ������ �����ϴ�");
        }
        return false;
    }

    // ȸ�� ����
    public void CreateUser(string _id, string _pw, string _nickName)
    {
        if (CheckUse(UserTableInfo.user_id,_id))
        {
            // �Է°� SQL Injection ���� �� �н����� ��ȣȭ (���� + �ؽ�)
            string securityPW = SHA512Hash(_pw + securityString);

            // ������ create_at, update_at ������ �������ش�.
            InsertDB(UserTableInfo.table_name, $"{UserTableInfo.user_id}, {UserTableInfo.user_pw}, {UserTableInfo.nickname}, {UserTableInfo.create_at}, {UserTableInfo.update_at}", $"'{_id}','{securityPW}','{_nickName}', NOW(), NOW()");

            UnityEngine.Debug.Log("ID :" + _id);
            UnityEngine.Debug.Log("PW :" + securityPW);
            UnityEngine.Debug.Log("PW :" + _nickName);
        }

    }

    // ���̵� �ߺ� ó��
    public bool CheckUse(string _column, string _id)
    {
        // �Է��� �� �ִ� ���̵�/�г����ΰ�
        
        DataTable dataTable = selsql($"SELECT {UserTableInfo.user_id} FROM {UserTableInfo.table_name} WHERE {_column} = '{_id}'");

        if (dataTable.Rows.Count == 0)
        {
            return true;
        }
        else
        {
            UnityEngine.Debug.Log("������� ID�Դϴ�.");
            return false;
        }
    }

}
