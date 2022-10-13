using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class PeekabooLogin : MonoBehaviour
{
    string playerPeekabooDataJson;

    public void SavePeekabooData()
    {
        playerPeekabooDataJson = JsonUtility.ToJson(GameManager.Instance.PlayerData.PlayerPeekabooData);
        UnityEngine.Debug.Log("playerPeekabooDataJson : " + playerPeekabooDataJson);
        DataBase.Instance.UpdateDB(UserTableInfo.table_name, UserTableInfo.peekaboo, playerPeekabooDataJson,UserTableInfo.user_id, GameManager.Instance.PlayerData.ID);
    }

    public void LoadPeekabooData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(UserTableInfo.table_name, "*", UserTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                playerPeekabooDataJson = row[UserTableInfo.peekaboo].ToString();
            }
        }
        GameManager.Instance.PlayerData.PlayerPeekabooData = JsonUtility.FromJson<PlayerPeekabooData>(playerPeekabooDataJson);
    }
}
