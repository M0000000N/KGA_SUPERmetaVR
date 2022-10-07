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
    public static readonly string TableName = "UserData";
    public static readonly string ID = "UserID";
    public static readonly string PW = "UserPW";
    public static readonly string NickName = "NickName";
    public static readonly string JoinDate = "JoinDate";
    public static readonly string AccessDate = "AccessDate";
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
        sqlcmdall("INSERT INTO " + _tableName + " (" + _column + ") VALUES (" + _data + ")");
    }

    public void UpdateDB(string _tableName, string _updateColumn, string _updateData, string _findColum, string _findData)
    {
        sqlcmdall("UPDATE " + _tableName + " SET " + _updateColumn + "= '" + _updateData + "' WHERE " + _findColum + "= '" + _findData + "'");

    }

    public DataTable SelectDB(string _tableName, string _findColumn, string _checkColumn, string _checkData)
    {
        DataTable dataTable = selsql("SELECT " + _findColumn + " FROM " + _tableName + " WHERE " + _checkColumn + "='" + _checkData + "'");
        return dataTable;
    }

    // �α��� ���
    public bool Login(string _id, string _pw)
    {
        // �Է°� SQL Injection ���� �� �н����� ��ȣȭ (���� + �ؽ�)
        string securityPW = SHA512Hash(_pw + securityString);

        DataTable dataTable = SelectDB(UserTableInfo.TableName, UserTableInfo.PW, UserTableInfo.ID, _id);

        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (securityPW == row[UserTableInfo.PW].ToString())
                {
                    UnityEngine.Debug.Log("�α��ο� �����߽��ϴ�.");

                    UpdateDB(UserTableInfo.TableName, UserTableInfo.AccessDate, "NOW()", UserTableInfo.ID, _id);
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
        if (CheckUse(UserTableInfo.ID,_id))
        {
            // �Է°� SQL Injection ���� �� �н����� ��ȣȭ (���� + �ؽ�)
            string securityPW = SHA512Hash(_pw + securityString);

            InsertDB(UserTableInfo.TableName, $"{UserTableInfo.ID}, {UserTableInfo.PW}, {UserTableInfo.NickName}, {UserTableInfo.JoinDate}", $"'{_id}','{securityPW}','{_nickName}', NOW()");

            UnityEngine.Debug.Log("ID :" + _id);
            UnityEngine.Debug.Log("PW :" + securityPW);
            UnityEngine.Debug.Log("PW :" + _nickName);
        }

    }

    // ���̵� �ߺ� ó��
    public bool CheckUse(string _column, string _id)
    {
        // �Է��� �� �ִ� ���̵�/�г����ΰ�
        
        DataTable dataTable = selsql($"SELECT {UserTableInfo.ID} FROM {UserTableInfo.TableName} WHERE {_column} = '{_id}'");

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
