using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Photon.Pun;
using System;

public static class FeefawfumTableInfo
{
    public static readonly string table_name = "FeefawfumData";

    public static readonly string user_id = "user_id";

    public static readonly string cooltime = "cooltime"; // 쿨타임 (이벤트를 진행한 시간)
    public static readonly string today_count = "today_count"; // 오늘 미션 완료 횟수

    public static readonly string total_count = "total_count"; // 총 미션 완료 횟수

    public static readonly string create_at = "create_at";
    public static readonly string update_at = "update_at";
    public static readonly string is_delete = "is_delete";
}


public class FeefawfumDataBase : SingletonBehaviour<FeefawfumDataBase>
{
    private PlayerData playerData;

    void Start()
    {
        playerData = GameManager.Instance.PlayerData;
    }

    // 테스트 코드
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(CheckCooltime(3))
            {
                UpdatePlayData();
            }
        }
    }
    // 테스트 코드

    public void CreateFeefawfumData() // 데이터가 없으면 생성
    {
        DataBase.Instance.InsertDB(FeefawfumTableInfo.table_name, $"{FeefawfumTableInfo.user_id}, {FeefawfumTableInfo.create_at}, {FeefawfumTableInfo.update_at}", $"'{playerData.ID}', NOW(), NOW()");
    }

    public void LoadFeefawfumData()
    {
        DataTable dataTable = DataBase.Instance.FindDB(FeefawfumTableInfo.table_name, "*", FeefawfumTableInfo.user_id, GameManager.Instance.PlayerData.ID);
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                CheckTodayData(row);

                playerData.PlayerFeefawfumData.CoolTime = row[FeefawfumTableInfo.cooltime].ToString();
                playerData.PlayerFeefawfumData.TodayCount = int.Parse(row[FeefawfumTableInfo.today_count].ToString());
                playerData.PlayerFeefawfumData.TotalCount = int.Parse(row[FeefawfumTableInfo.total_count].ToString());
            }


        }
        else if (dataTable.Rows.Count <= 0)
        {
            UnityEngine.Debug.Log("<color=red>FFF 데이터가 없습니다</color>");
        }
    }

    public bool UpdatePlayData()
    {
        LoadFeefawfumData();

        if (playerData.PlayerFeefawfumData.TodayCount < 3) // TODO : 플레이 횟수는 나중에 Datatable로 관리될 수 있으니 수정이 발생할 수 있습니다.
        {
            ++playerData.PlayerFeefawfumData.TodayCount;
            ++playerData.PlayerFeefawfumData.TotalCount;

            DataBase.Instance.sqlcmdall($"UPDATE {FeefawfumTableInfo.table_name} SET " +
                                        $"{FeefawfumTableInfo.cooltime} = NOW(), " +
                                        $"{FeefawfumTableInfo.today_count} = {playerData.PlayerFeefawfumData.TodayCount}, " +
                                        $"{FeefawfumTableInfo.total_count} = {playerData.PlayerFeefawfumData.TotalCount} " +
                                        $"WHERE {FeefawfumTableInfo.user_id} = '{playerData.ID}'");
            return true;
        }

        UnityEngine.Debug.Log("<color=red>오늘 플레이 횟수를 모두 사용하였습니다.</color>");
        return false;
    }

    public bool CheckCooltime(float _cooltime)
    {
        if (playerData.PlayerFeefawfumData.CoolTime == "") return true;

        DateTime cooltime = DateTime.Parse(playerData.PlayerFeefawfumData.CoolTime);
        DateTime nowtime = DateTime.UtcNow; // TODO : 현재 UTC 기준으로 9시간 차이가 있습니다.
        cooltime = cooltime.AddHours(_cooltime);

        if(cooltime < nowtime) // 우선 로컬 데이터를 기준으로 판단한다.
        {
            DataTable dataTable = DataBase.Instance.FindDB(FeefawfumTableInfo.table_name, "*", FeefawfumTableInfo.user_id, GameManager.Instance.PlayerData.ID);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    DateTime cooltimeData = DateTime.Parse(row[FeefawfumTableInfo.cooltime].ToString());
                    cooltimeData = cooltimeData.AddHours(_cooltime);
                    if(cooltimeData < nowtime) // 로컬 데이터 변조를 방지하기 위하여 DB데이터 확인을 다시 진행 한다.
                    {
                        UnityEngine.Debug.Log($"{nowtime - cooltimeData} <color=blue> : 쿨타임이 지나 실행되었습니다.</color>");
                        return true;
                    }
                }
            }
        }
        
        UnityEngine.Debug.Log($"{nowtime - cooltime} <color=red> : 아직 쿨타임이 지나지 않았습니다.</color>");
        return false;
    }

    public void CheckTodayData(DataRow _row)
    {
        DateTime updateTime = DateTime.Parse(_row[FeefawfumTableInfo.update_at].ToString());
        DateTime nowtime = DateTime.UtcNow; // TODO : 현재 UTC 기준으로 9시간 차이가 있습니다.

        if((nowtime - updateTime).Days > 0) // 날짜가 지났을 경우
        {
            DataBase.Instance.sqlcmdall($"UPDATE {FeefawfumTableInfo.table_name} " +
                                        $"SET {FeefawfumTableInfo.today_count} = 0, " +
                                        $"{FeefawfumTableInfo.update_at} = NOW() " +
                                        $"WHERE {FeefawfumTableInfo.user_id} = '{playerData.ID}'");
        }
        else
        {
            UnityEngine.Debug.Log($"<color=red>아직 하루가 지나지 않았습니다.</color> \n" +
                $"{nowtime} - {updateTime} = {(nowtime - updateTime).Days}");
        }
    }
}
