using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public static readonly int MaxCol = 5;
    public static readonly int MaxRow = 7;
    // //
    public static readonly int Free_Diamond = 15;
    public static readonly int Reward_Diamond = 100;
    // //
    public static readonly int Price_Skill = 150;
    public static readonly long ScoreNewBlock = 16000;
    public static readonly long MaxBlock = 1024;
    // //
    public static readonly string Item_Coin_600 = "Item_Coin_600";
    public static readonly string Item_Coin_1600 = "Item_Coin_1600";
    public static readonly string Item_Coin_3400 = "Item_Coin_3400";
    public static readonly string Item_Coin_7200 = "Item_Coin_7200";
    public static readonly string Item_Coin_12000 = "Item_Coin_12000";
    public static readonly string Item_Coin_48000 = "Item_Coin_48000";
    // // 
    public static readonly string Event_Rivive_Five = "Event_Rivive_Five";
    public static readonly string Event_HighScore_First = "Event_HighScore_First";
    public static readonly string Event_HighScore_Second = "Event_HighScore_Second";
    public static readonly string Event_Unlock_128 = "Event_Unlock_128";
}
