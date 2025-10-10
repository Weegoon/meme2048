using base58;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LocalStore
{
    // // key
    public static readonly string LS_Map = "LS_Map";
    public static readonly string LS_First = "LS_First";
    public static readonly string LS_Rate = "LS_Rate";
    public static readonly string LS_HightScore = "LS_HightScore";
    public static readonly string LS_Block_HightScore = "LS_Block_HightScore";
    public static readonly string LS_Diamond = "LS_Diamond";
    public static readonly string LS_Rank = "LS_Rank";
    public static readonly string LS_Amount = "LS_Amount";
    public static readonly string LS_MaxBlock = "LS_MaxBlock";
    public static readonly string LS_Score_NewBlock = "LS_Score_NewBlock";
    public static readonly string LS_Block_Number_Min = "LS_Block_Number_Min";
    public static readonly string LS_Block_Change = "LS_Block_Change";
    public static readonly string LS_Swap = "LS_Swap";
    public static readonly string LS_Score = "LS_Score";
    public static readonly string LS_Drop = "LS_Drop";
    public static readonly string LS_Sound = "LS_Sound";
    public static readonly string LS_Vibrate = "LS_Vibrate"; 
    public static readonly string LS_SwapPrice = "LS_SwapPrice";
    public static readonly string LS_DropPrice = "LS_DropPrice";
    public static readonly string LS_Revive = "LS_Revive";
    public static readonly string LS_Remove_Ads = "LS_Remove_Ads";
    public static readonly string LS_Count_High_Score = "LS_Count_High_Score";

    // //

    // //
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
    // //
    public static int GetInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    public static float GetFloat(string key, float defaultValue = 0)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
    public static string GetString(string key, string df = null)
    {
        return PlayerPrefs.GetString(key, df);
    }
    // // 
    public static void SaveMap(List<long> listData = null)
    {
        string map = null;
        if (listData == null)
        {
            for (int i = 0; i < 35; i++)
            {
                if (i == 34)
                    map += "" + 0;
                else
                    map += 0 + "-";
            }
        }
        else
        {
            for (int i = 0; i < 35; i++)
            {
                if (i == 34)
                    map += listData[i];
                else
                    map += listData[i] + "-";
            }
        }

        SetString(LS_Map, map);
    }
    public static List<int> LoadMap(string map)
    {
        List<int> listResult = new List<int>();
        if (string.IsNullOrEmpty(map))
        {
            for (int i = 0; i < 35; i++)
            {
                listResult.Add(0);
            }
        }
        else
        {
            var arr = map.Split('-');
            for (int i = 0; i < arr.Length; i++)
            {
                listResult.Add(System.Convert.ToInt32(arr[i]));
            }
        }
        return listResult;
    }
    public static List<int> LoadMap()
    {
        List<int> listResult = new List<int>();
        string map = GetString(LS_Map);
        //map = "2048-524288-65536-524288-256-128-65536-8192-65536-128-64-8192-1024-8192-64-32-1024-128-1024-32-16-128-16-128-16-8-16-2-16-8-4-2-0-2-4";
        //Debug.LogError(map);
        if (string.IsNullOrEmpty(map))
        {
            for (int i = 0; i < 35; i++)
            {
                listResult.Add(0);
            }
        }
        else
        {
            var arr = map.Split('-');
            for (int i = 0; i < arr.Length; i++)
            {
                listResult.Add(System.Convert.ToInt32(arr[i]));
            }
        }
        return listResult;
    }
    public static bool IsFirst()
    {
        return GetInt(LS_First) != 1;
    }
    public static bool IsRemoveAds()
    {
        return GetInt(LS_Remove_Ads) == 1;
    }
    public static void ChangeFirst()
    {
        SetInt(LS_First, 1);
    }
    // //
    public static long HighScore()
    {
        return System.Convert.ToInt64(GetString(LS_HightScore, "0"));
    }
    public static void SetHightScore(long hightScore)
    {
        SetString(LS_HightScore, hightScore.ToString());
    }
    public static long Score()
    {
        return System.Convert.ToInt64(GetString(LS_Score, "0"));
    }
    public static void SetScore(long Score)
    {
        SetString(LS_Score, Score.ToString());
    }
    public static int SwapPrice()
    {
        return GetInt(LS_SwapPrice, GameConfig.Price_Skill);
    }
    public static void SetSwapPrice(int swapPrice)
    {
        SetInt(LS_SwapPrice, swapPrice);
    }
    public static int DropPrice()
    {
        return GetInt(LS_DropPrice, GameConfig.Price_Skill);
    }
    public static void SetDropPrice(int dropPrice)
    {
        SetInt(LS_DropPrice, dropPrice);
    }
    // //
    public static int GetDiamond()
    {
        string cvalue = GetString(LS_Diamond);
        if (string.IsNullOrEmpty(cvalue))
        {
            return 0;
        }
        else
        {
            byte[] bytes = Base58.Decode(cvalue);
            string value = Encoding.UTF8.GetString(bytes);
            return System.Convert.ToInt32(value);
        }
    }
    public static void SetDiamond(int diamond)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(diamond.ToString());
        var value = Base58.Encode(bytes);
        SetString(LS_Diamond, value);
    }
    public static List<long> GetTopRank()
    {
        List<long> listResult = new List<long>();
        string ranks = GetString(LS_Rank);
        if (string.IsNullOrEmpty(ranks))
        {
            for (int i = 0; i < 5; i++)
            {
                listResult.Add(0);
            }
        }
        else
        {
            var arr = ranks.Split('-');
            for (int i = 0; i < arr.Length; i++)
            {
                listResult.Add(System.Convert.ToInt64(arr[i]));
            }
        }
        return listResult;
    }
    public static void SaveRank(List<long> listData = null)
    {
        var descendingOrder = listData.OrderByDescending(i => i).ToList();
        string ranks = null;
        if (descendingOrder == null || descendingOrder.Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                    ranks += "" + 0;
                else
                    ranks += 0 + "-";
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                    ranks += "" + descendingOrder[i];
                else
                    ranks += descendingOrder[i] + "-";
            }
        }


        SetString(LS_Rank, ranks);
    }
    public static void SetMaxBlock(long value)
    {
        SetString(LocalStore.LS_MaxBlock, value.ToString());
    }
    public static long GetMaxBlock()
    {
        string strmb = GetString(LocalStore.LS_MaxBlock);
        if (string.IsNullOrEmpty(strmb))
            return 1024;
        else
            return System.Convert.ToInt64(strmb);
    }
    public static void SetScoreNewBlock(long value)
    {
        SetString(LocalStore.LS_Score_NewBlock, value.ToString());
    }
    public static long GetScoreNewBlock()
    {
        string strmb = GetString(LocalStore.LS_Score_NewBlock);
        if (string.IsNullOrEmpty(strmb))
            return 16000;
        else
            return System.Convert.ToInt64(strmb);
    }
    public static List<long> GetListAmount()
    {
        List<long> listResult = new List<long>();
        string amounts = GetString(LS_Amount);
        if (string.IsNullOrEmpty(amounts))
        {
            listResult = new List<long> { 2, 4, 8, 16, 32, 64 };
            //listResult = new List<long> { 20000000, 40000000, 8000000, 16000000, 32000000, 64000000 };
        }
        else
        {
            var arr = amounts.Split('-');
            for (int i = 0; i < arr.Length; i++)
            {
                listResult.Add(System.Convert.ToInt64(arr[i]));
            }
        }
        return listResult;
    }
    public static void SaveAmount(List<long> listData = null)
    {
        string amounts = "";
        if (listData != null)
        {
            for (int i = 0; i < 6; i++)
            {
                if (i == 5)
                    amounts += "" + listData[i];
                else
                    amounts += listData[i] + "-";
            }
        }
        SetString(LS_Amount, amounts);
    }

    public static bool GetSound()
    {
        return GetInt(LS_Sound) == 0 ? true : false;
    }
    public static bool GetVibrate()
    {
        return GetInt(LS_Vibrate) == 0 ? true : false;
    }

    
    public static void SetSound(bool isopen)
    {
        SetInt(LS_Sound, isopen ? 0 : 1);
    }
}
