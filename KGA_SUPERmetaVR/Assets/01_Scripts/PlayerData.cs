using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    string id;
    public string ID { get { return id; } }

    string nickName;
    public string Nickname { get { return nickName;} set { nickName = value; } }

    int coin;
    public int Coin { get { return coin; } set { coin = value; } }

    public void SetDataBase()
    {

    }

    public void GetDataBase()
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.TableName, "*",UserTableInfo.ID, id);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                nickName = row[UserTableInfo.NickName].ToString();
                coin = (int)row[UserTableInfo.Coin];
            }
        }
    }
}
