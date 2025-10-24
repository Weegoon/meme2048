using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using TMPro;
using Spine;
using Spine.Unity;
using DG.Tweening;
using ObjectPooling;

public class Map : MonoBehaviour
{
    public static Map Instance;

    public ObjectPoolManager objectPoolManager;

    [SerializeField]
    List<ItemPlay> listItems;
    [SerializeField]
    List<LineHandle> listLine;
    // //
    Dictionary<string, ItemPlay> dicMap;
    // //
    [HideInInspector]
    public long currentAmount;
    [SerializeField]
    GameObject bgItem;
    [SerializeField]
    ItemDrag CurentItem;
    [SerializeField]
    ItemDiamondEfect ItemDiamondEfect;
    [SerializeField]
    ItemTemplate ItemTemplate;
    //[SerializeField]
    //ItemMove ItemMove;
    [SerializeField]
    ItemSkill ItemSwap, ItemDrop;
    [HideInInspector]
    public float rt2, rt4, rt8, rt16, rt32, rt64;
    [HideInInspector]
    public long Score, matchTime;
    [HideInInspector]
    public bool IsPlaying;
    // //
    //[SerializeField]
    //Sprite[] sprites;
    [SerializeField]
    GameObject[] sprites;

    [SerializeField]
    Sprite[] amountSprites;
    [SerializeField]
    Color[] colors;
    [SerializeField]
    TMP_Text txtScore, txtTime, txtHightScore, txtDiamond;
    [SerializeField]
    PopupSummary popupSummary;
    [SerializeField]
    PopupTest popupTest;
    //[SerializeField]
    //PopupNewScore popupNewScore;
    //[SerializeField]
    //PopupRevive popupRevive;
    [SerializeField]
    PopupLose popupLose;
    //[SerializeField]
    //PopupShop popupShop;
    [SerializeField]
    PopupSetting popupSetting;
    //[SerializeField]
    //PopupRank popupRank;
    [SerializeField]
    SkillGuide skillGuide;
    [SerializeField]
    GameObject Container;
    [SerializeField]
    ParticleSystem broken_square_2, impact_diamond, impact_square;
    [SerializeField]
    GameObject DiamondTaget, ImageBgSkill;
    [SerializeField]
    Tutorial tutorial;

    // //

    private void Awake()
    {
        currentAmount = 2;
        Instance = this;
    }
    private void Start()
    {
        isFirst = LocalStore.IsFirst();
        GameManager.Register(Event_e.DiamonChange, OnDiamondChange);
        InitMap();
    }
    #region // // Game
    public void InitMap()
    {
        dicMap = new Dictionary<string, ItemPlay>();
        listAmount = LocalStore.GetListAmount();

        countBlockChange = LocalStore.GetInt(LocalStore.LS_Block_Change);
        List<int> listMap = LocalStore.LoadMap();
        if (isFirst)
        {
            string map = "0-0-0-4-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0";
            listMap = LocalStore.LoadMap(map);
            countTutorial = 0;
        }
        //listItems.Count
        for (int i = 0; i < listItems.Count; i++)
        {
            int row = i / GameConfig.MaxCol;
            int col = i % GameConfig.MaxCol;
            string key = row + "-" + col;
            listItems[i].Init(MapType.Nome, key, listMap[i], OnItemPlayClicked);
            dicMap.Add(key, listItems[i]);

        }
        if (CheckEndGame())
        {
            Replay();
        }
        // // InitLine
        for (int i = 0; i < GameConfig.MaxCol; i++)
        {
            listLine[i].InitLine(i, PointerClick);
        }
        InitStart();
        ReSize();
        StartMatch();
    }
    private void OnItemPlayClicked(string key)
    {
        if (enableSwap)
        {
            SoundManager.Instance.PlaySound("sfx_ui_select");
            if (string.IsNullOrEmpty(itemSwap_0))
            {
                itemSwap_0 = key;
                dicMap[key].Tick(true);
            }
            else
            {
                if (key == itemSwap_0)
                {
                    itemSwap_0 = null;
                    dicMap[key].Tick(false);
                    return;
                }
                else if (string.IsNullOrEmpty(itemSwap_1))
                {
                    itemSwap_1 = key;
                    dicMap[key].Tick(true);
                    skillGuide.EffectSwapBroken(dicMap[itemSwap_0].transform.position, dicMap[itemSwap_1].transform.position, () =>
                    {
                        DoSwap(dicMap[itemSwap_0], dicMap[itemSwap_1], () =>
                         {
                             Swap();
                             OnUseSkill(SkillType.Swap);
                         });
                        //ChangeEffect(dicMap[itemSwap_0], dicMap[itemSwap_1]);

                    });
                }
            }
        }
        else if (enableDrop && !isUseSkill)
        {
            SoundManager.Instance.PlaySound("sfx_ui_select");
            isUseSkill = true;

            skillGuide.EffectDropBroken(dicMap[key], () =>
            {
                dicMap[key].gameObject.SetActive(false);
            }, () =>
            {
                dicMap[key].gameObject.SetActive(false);
                EffectDrop(key);
                OnUseSkill(SkillType.Drop);
            });
        }
        else
        {
            var col = System.Convert.ToInt32(key.Split('-')[1]);
            PointerClick(col);
        }
    }

    private void DoSwap(ItemPlay item0, ItemPlay item1, Action p)
    {
        float time = 0.5f;
        // // 
        SoundManager.Instance.PlaySound("sfx_item_swap");
        var itemMove0 = PoolManager.Instance.GetItemMove(this.transform.parent).GetComponent<ItemMove>();
        itemMove0.transform.position = item0.transform.position;
        if (aspect > 1.7778f)
            itemMove0.transform.localScale = new Vector3(scale, scale, scale);
        itemMove0.SetData(item0.Amount);
        itemMove0.gameObject.SetActive(true);
        // // 
        var itemMove1 = PoolManager.Instance.GetItemMove(this.transform.parent).GetComponent<ItemMove>();
        itemMove1.transform.position = item1.transform.position;
        if (aspect > 1.7778f)
            itemMove1.transform.localScale = new Vector3(scale, scale, scale);
        itemMove1.SetData(item1.Amount);
        itemMove1.gameObject.SetActive(true);
        item0.gameObject.SetActive(false);
        item1.gameObject.SetActive(false);
        // //
        //itemMove0.transform.DOLocalRotate(new Vector3(0, 0, 360), time, DG.Tweening.RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
        itemMove0.transform.DOLocalRotate(new Vector3(0, 0, 360), time, DG.Tweening.RotateMode.FastBeyond360);
        itemMove0.transform.DOMove(item1.transform.position, time).OnComplete(() =>
        {
            itemMove0.gameObject.SetActive(false);
        });
        itemMove1.transform.DOLocalRotate(new Vector3(0, 0, 360), time, DG.Tweening.RotateMode.FastBeyond360);
        itemMove1.transform.DOMove(item0.transform.position, time).OnComplete(() =>
        {
            p();
            itemMove1.gameObject.SetActive(false);
        });
    }

    private void Swap()
    {
        enableSwap = false;
        // // effect

        var amount = dicMap[itemSwap_0].Amount;
        var diamond = dicMap[itemSwap_0].IsDiamond;
        dicMap[itemSwap_0].SetData(dicMap[itemSwap_1].Amount);
        dicMap[itemSwap_0].Diamond(dicMap[itemSwap_1].IsDiamond);
        dicMap[itemSwap_1].SetData(amount);
        dicMap[itemSwap_1].Diamond(diamond);
        dicMap[itemSwap_0].Tick(false);
        dicMap[itemSwap_1].Tick(false);
        //
        dicMap[itemSwap_0].transform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
        {
            var temp = GetKeyGroup(dicMap[itemSwap_0]);
            var temp1 = GetKeyGroup(dicMap[itemSwap_1]);
            if (temp.Count > 0 || temp1.Count > 0)
            {
                if (temp.Count > temp1.Count)
                    CheckGroup(dicMap[itemSwap_0]);
                else
                    CheckGroup(dicMap[itemSwap_1]);
            }
            itemSwap_0 = null;
            itemSwap_1 = null;
        });

    }
    private void EffectDrop(string key)
    {
        enableDrop = false;
        // // effect
        dicMap[key].SetData(0);
        //dicMap[key].OffAllParticle();
        var Item = dicMap[key];
        List<string> listkey = new List<string>() { key };
        ReSortMap(Item, listkey, time * 1.3f);
    }
    private void InitStart()
    {
        rt2 = 20;
        rt4 = rt2 + 20;
        rt8 = rt4 + 20;
        rt16 = rt8 + 20;
        rt32 = rt16 + 10;
        rt64 = rt32 + 10;
        CurentItem.InitDrag(OnBeginDrag, Ondrag, OnStopdrag);
        listrt = new List<float>() { 20, 20, 20, 20, 10, 10 };
    }
    private void ReSize()
    {
        aspect = 1f / Camera.main.aspect;
        scale = 1.7778f / aspect;
        //if (aspect > 2f)
        //{
        //    this.Container.transform.localScale = new Vector3(scale, scale, 1);
        //    this.ItemTemplate.transform.localScale = new Vector3(scale, scale, 1);
        //}
        if (aspect > 1.7778f)
        {
            scale = 1.7778f / aspect;
            this.tutorial.transform.localScale = new Vector3(scale, scale, 1);
            this.Container.transform.localScale = new Vector3(scale, scale, 1);
            this.ItemTemplate.transform.localScale = new Vector3(scale, scale, 1);
            this.transform.localScale = new Vector3(scale, scale, 1);
        }
        else
        {
            scale = 0.95f;
            this.tutorial.transform.localScale = new Vector3(scale, scale, 1);
            this.Container.transform.localScale = new Vector3(scale, scale, 1);
            this.ItemTemplate.transform.localScale = new Vector3(scale, scale, 1);
            this.transform.localScale = new Vector3(scale, scale, 1);
        }

    }
    private void StartMatch()
    {
        combo = 0;
        Score = LocalStore.Score();
        count_highScore = LocalStore.GetInt(LocalStore.LS_Count_High_Score);
        SwapPrice = LocalStore.SwapPrice();
        SwapCount = LocalStore.GetInt(LocalStore.LS_Swap);
        countBlockChange = LocalStore.GetInt(LocalStore.LS_Block_Change);
        ItemSwap.Init(SwapCount, SwapPrice, OnUsingItem, OnBuyItem);
        DropPrice = LocalStore.DropPrice();
        DropCount = LocalStore.GetInt(LocalStore.LS_Drop);
        ItemDrop.Init(DropCount, DropPrice, OnUsingItem, OnBuyItem);
        count_ChangeBlock = 0;
        matchTime = 0;
        txtScore.text = Utils.FormatNumber1(Score);
        OnDiamondChange(LocalStore.GetDiamond());
        txtHightScore.text = Utils.FormatNumber1(LocalStore.HighScore());
        IsPlaying = true;
        CurentLineCol = -1;
        MaxBlock = LocalStore.GetMaxBlock();
        maxBlockAmount = MaxBlock;
        listAmount = LocalStore.GetListAmount();
        count_2 = 0;
        count_4 = 0;
        count_8 = 0;
        count_16 = 0;
        count_32 = 0;
        count_64 = 0;
        ReviveCount = LocalStore.GetInt(LocalStore.LS_Revive);
        ScoreNewBlock = LocalStore.GetScoreNewBlock();
        blockAmount = 0;
        isChangeBlock = false;
        enableSwap = false;
        enableDrop = false;
        isMoveComplete = true;
        isDiaMond = false;
        itemSwap_0 = null;
        itemSwap_1 = null;
        Debug.Log(isFirst);
        if (isFirst)
        {
            if (countTutorial == 0)
            {
                currentAmount = listAmount[0];
                CurentItem.SetData(currentAmount);
                CurentItem.Diamond(isDiaMond);
                string key = (GameConfig.MaxRow - 1) + "-" + (GameConfig.MaxCol - 3);
                ItemPlay Item = dicMap[key];
                tutorial.Show(Item, TutorialCallBack);
            }
        }
        else
            RandomAmount();
        coroutine = StartCoroutine(StartTimer());
    }

    private void TutorialCallBack()
    {
        countTutorial++;
        if (isFirst)
        {
            if (countTutorial == 1)
            {
                PointerClick(2);
                string key = (GameConfig.MaxRow - 1) + "-" + +1;
                ItemPlay Item = dicMap[key];
                tutorial.ChangeItem(Item);
            }
            if (countTutorial == 2)
            {
                PointerClick(1);
                string key = (GameConfig.MaxRow - 1) + "-" + (GameConfig.MaxCol - 3);
                ItemPlay Item = dicMap[key];
                tutorial.ChangeItem(Item);
            }
            if (countTutorial == 3)
            {
                PointerClick(2);
                isFirst = false;
                LocalStore.ChangeFirst();
                tutorial.ShowGoodJob();
            }
        }
    }

    private void OnBeginDrag()
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        LineHandle line = GetLine(point.x);
        if (CurentLineCol == -1 || CurentLineCol != line.Col)
        {
            CurentLineCol = line.Col;
            Activeline(CurentLineCol);
            PointerEnter(CurentLineCol);
        }
    }
    private void Ondrag()
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        LineHandle line = GetLine(point.x);
        if (CurentLineCol == -1 || CurentLineCol != line.Col)
        {
            CurentLineCol = line.Col;
            Activeline(CurentLineCol);
            PointerEnter(CurentLineCol);
        }

    }
    private void OnStopdrag()
    {
        InActiveline();
        PointerClick(CurentLineCol);
        CurentItem.transform.position = bgItem.transform.position;
    }
    private LineHandle GetLine(float x)
    {
        LineHandle line = listLine[0];
        var disMin = Mathf.Abs(line.transform.position.x - x);
        foreach (LineHandle li in listLine)
        {
            var disTemp = Mathf.Abs(li.transform.position.x - x);
            if (disTemp < disMin)
            {
                disMin = disTemp;
                line = li;
            }
        }
        return line;
    }
    private void Activeline(int col)
    {
        foreach (LineHandle line in listLine)
        {
            line.ActiveLine(line.Col == col);
        }
    }
    private void InActiveline()
    {
        foreach (LineHandle line in listLine)
        {
            line.ActiveLine(false);
        }
    }
    private void PointerEnter(int col)
    {
        // //
        var key = GetKeyEnter(col);
        if (!string.IsNullOrEmpty(key))
        {
            var item = dicMap[key];
            ItemTemplate.transform.position = item.transform.position;
            ItemTemplate.SetData(currentAmount);
            ItemTemplate.gameObject.SetActive(true);
            ItemTemplate.transform.parent = listLine[col].transform;
        }
    }
    private void PointerClick(int col)
    {
        timeTest = Utils.TimeStemp(DateTime.Now);
        comboCount = 0;
        if (!isMoveComplete || enableSwap || enableDrop)
            return;
        var key = GetKeyEnter(col);
        if (!string.IsNullOrEmpty(key))
        {
            SoundManager.Instance.PlaySound("sfx_block");
            isMoveComplete = false;
            var item = dicMap[key];
            if (isDiaMond || item.IsDiamond)
            {
                item.Diamond(true);
            }
            if (item.Amount > 0 && item.Amount == currentAmount)
            {
                item.SetData(currentAmount * 2);
                RandomAmount();
                item.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.12f).SetEase(Ease.OutCubic).OnComplete(() =>
                {
#if !UNITY_EDITOR
                     if (GameManager.Instance.IsVibrate)
                     {
                         Vibration.VibratePredefined(Vibration.PredefinedEffect.EFFECT_CLICK);
                     }
#endif
                    item.transform.DOScale(Vector3.one, 0.12f).SetEase(Ease.OutCubic).OnComplete(() =>
                    {
                        CheckGroup(item);
                    });
                });
            }
            else
            {
                var startkey = GetKey(GameConfig.MaxRow - 1, item.Col);
                var itemStart = dicMap[startkey];
                var itemMove = PoolManager.Instance.GetItemMove(this.transform.parent).GetComponent<ItemMove>();
                itemMove.transform.position = itemStart.transform.position;
                if (aspect > 1.7778f)
                    itemMove.transform.localScale = new Vector3(scale, scale, scale);
                itemMove.SetData(currentAmount, true);
                itemMove.gameObject.SetActive(true);
                itemMove.MoveTaget(item.transform.position, time, () =>
                {
                    item.SetData(currentAmount);
                    itemMove.gameObject.SetActive(false);
                    List<string> listKey = GetKeyGroup(item);
                    if (listKey.Count <= 0)
                        SoundManager.Instance.PlaySound("sfx_block_impinge");
                    if (isFirst)
                    {
                        if (countTutorial == 1)
                        {
                            currentAmount = listAmount[0];
                            CurentItem.SetData(currentAmount);
                            CurentItem.Diamond(isDiaMond);
                        }
                        if (countTutorial == 2)
                        {
                            currentAmount = listAmount[1];
                            CurentItem.SetData(currentAmount);
                            CurentItem.Diamond(isDiaMond);
                        }
                    }
                    else
                    {
                        RandomAmount();
                    }
                    CheckGroup(item);
                });
            }

        }
        else
        {
            isMoveComplete = true;
        }

    }
    private void CheckGroup(ItemPlay item)
    {
        //Debug.Log("CheckGroupkey : " + item.Key + " Amount " + item.Amount);
        ItemTemplate.gameObject.SetActive(false);
        List<string> listKey = GetKeyGroup(item);
        if (listKey.Count > 0)
        {
            if (listKey.Count == 1)
            {
                ItemPlay itemTemp = dicMap[listKey[0]];
                List<string> listKeyTemp = GetKeyGroup(itemTemp);
                if (listKeyTemp.Count > 1)
                {
                    GroupItem(itemTemp, listKeyTemp);
                }
                else
                {
                    GroupItem(item, listKey);
                }
            }
            else
            {
                GroupItem(item, listKey);
            }

        }
        else
        {
            var ItemCheck = CheckItemGroup();
            if (ItemCheck != null)
            {
                CheckGroup(ItemCheck);
            }
            else
            {
                // //
                List<long> listMap = new List<long>();
                for (int i = 0; i < 35; i++)
                {
                    listMap.Add(listItems[i].Amount);
                }
                LocalStore.SaveMap(listMap);
                isMoveComplete = true;
                // //
                if (CheckEndGame())
                {
                    //if (ReviveCount < 5)
                    //{
                    //    int price = 100;
                    //    if (ReviveCount < 3)
                    //    {
                    //        price = 100;
                    //    }
                    //    else if (ReviveCount == 3)
                    //    {
                    //        price = 500;
                    //    }
                    //    else
                    //        price = 1000;
                    //    popupRevive.Show(price, OnReviveCallback);
                    //}
                    //else
                    //{
                    //    OnEndGame();
                    //}
                    OnEndGame();
                }
                else
                {
                    CheckNewScore();
                    CheckNewBlockNumber();
                }
            }
        }
    }
    private void OnReviveCallback(bool value)
    {
        if (value)
        {
            ReviveMap();
            ReviveCount++;
            if (ReviveCount == 5)
                Bridge.instance.SendEvent(GameConfig.Event_Rivive_Five);
            LocalStore.SetInt(LocalStore.LS_Revive, ReviveCount);
            broken_square_2.Play();
        }
        else
        {
            OnEndGame();
        }
    }
    private void ReviveMap()
    {
        // // Remove Item min
        foreach (var key in dicMap.Keys)
        {
            if (dicMap[key].Amount <= listAmount[2])
                dicMap[key].SetData(0);
        }
        // //  dịch chuyển
        for (int j = 0; j < GameConfig.MaxCol; j++)
        {
            for (int i = 0; i < GameConfig.MaxRow; i++)
            {
                var key = GetKey(i, j);
                if (dicMap[key].Amount > 0)
                {
                    if (GetRow(j) < i)
                    {
                        dicMap[GetKeyEnter(j)].SetData(dicMap[key].Amount);
                        dicMap[key].SetData(0);
                    }
                }

            }
        }
        // //
        var ItemCheck = CheckItemGroup();
        if (ItemCheck != null)
        {
            CheckGroup(ItemCheck);
        }
    }
    private int GetRow(int col)
    {
        int row = 0;
        for (int i = 0; i < GameConfig.MaxRow; i++)
        {
            row = i;
            string temp = GetKey(row, col);
            if (dicMap[temp].Amount == 0)
            {
                break;
            }
        }
        return row;

    }
    private void OnEndGame()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        LocalStore.SaveAmount(null);
        LocalStore.SetInt(LocalStore.LS_Block_Change, 0);
        LocalStore.SaveMap();
        LocalStore.SetMaxBlock(GameConfig.MaxBlock);
        LocalStore.SetScoreNewBlock(GameConfig.ScoreNewBlock);
        LocalStore.SetScore(0);
        LocalStore.SetSwapPrice(GameConfig.Price_Skill);
        LocalStore.SetDropPrice(GameConfig.Price_Skill);
        LocalStore.SetInt(LocalStore.LS_Block_Number_Min, 2);
        var hightScore = LocalStore.HighScore();
        if (Score > hightScore)
        {
            hightScore = Score;
            LocalStore.SetHightScore(Score);
            count_highScore++;
            LocalStore.SetInt(LocalStore.LS_Count_High_Score, count_highScore);
            if (count_highScore == 1)
                Bridge.instance.SendEvent(GameConfig.Event_HighScore_First);
            else if (count_highScore == 2)
                Bridge.instance.SendEvent(GameConfig.Event_HighScore_Second);
        }
        SaveTopRank(Score);
        //popupSummary.Show(Replay, Score, matchTime, count_2, count_4, count_8, count_16, count_32, count_64);
        popupLose.Show(Score, hightScore);
    }
    private bool CheckEndGame()
    {
        for (int i = 0; i < GameConfig.MaxRow; i++)
        {
            for (int j = 0; j < GameConfig.MaxCol; j++)
            {
                string key = GetKey(i, j);

                if (dicMap[key].Amount <= 0)
                    return false;
            }
        }
        return true;
    }
    private ItemPlay CheckItemGroup()
    {
        ItemPlay itemResult = null;
        int max = 0;
        for (int i = 0; i < GameConfig.MaxRow; i++)
        {
            for (int j = 0; j < GameConfig.MaxCol; j++)
            {
                string key = GetKey(i, j);
                if (dicMap[key].Amount > 0)
                {
                    var temp = GetKeyGroup(dicMap[key]);
                    if (temp.Count > max)
                    {
                        max = temp.Count;
                        itemResult = dicMap[key];
                    }
                }

            }
        }
        return itemResult;
    }
    private List<string> GetKeyGroup(ItemPlay item)
    {
        List<string> listKey = new List<string>();
        if (item.Amount <= 0)
            return listKey;

        int row = item.Row;
        int col = item.Col;

        // // Left
        if (col > 0)
        {
            string temp = GetKey(row, col - 1);
            if (dicMap[temp].Amount == item.Amount)
                listKey.Add(temp);
        }
        // // Right
        if (col < GameConfig.MaxCol - 1)
        {
            string temp = GetKey(row, col + 1);
            if (dicMap[temp].Amount == item.Amount)
                listKey.Add(temp);
        }
        // // Up
        if (row > 0)
        {
            string temp = GetKey(row - 1, col);
            if (dicMap[temp].Amount == item.Amount)
                listKey.Add(temp);
        }
        // // Down
        if (row < GameConfig.MaxRow - 1)
        {
            string temp = GetKey(row + 1, col);
            if (dicMap[temp].Amount == item.Amount)
                listKey.Add(temp);
        }
        return listKey;
    }
    private void GroupItem(ItemPlay item, List<string> listkey)
    {
        //var now = Utils.TimeStemp(DateTime.Now);
        //Debug.LogError("GroupItem: " + (now - timeTest));
        //timeTest = now;
        if (listkey.Count == 1)
        {
            var row = System.Convert.ToInt32(listkey[0].Split('-')[0]);
            var col = System.Convert.ToInt32(listkey[0].Split('-')[1]);
            if (item.Col == col && item.Row - 1 == row)
            {
                var key = item.Key;
                item = dicMap[listkey[0]];
                listkey = new List<string>();
                listkey.Add(key);
            }
        }
        if (item.IsDiamond)
        {
            var value = UnityEngine.Random.Range(5, 9) * 10;
            EffectDiamond(item, value);
        }
        // // calculate Score
        blockAmount = item.Amount * (int)Math.Pow(2, listkey.Count);
        if (maxBlockAmount < blockAmount)
            maxBlockAmount = blockAmount;
        Score += blockAmount;
        LocalStore.SetScore(Score);
        txtScore.text = Utils.FormatNumber1(Score);
        comboCount++;
        if (comboCount > 5)
            comboCount = 5;
        SoundManager.Instance.PlaySound("sfx_block_combine" + comboCount);
        // // MoveEffect
        for (int i = 0; i < listkey.Count; i++)
        {
            string key = listkey[i];
            var itemStart = dicMap[key];
            if (itemStart.IsDiamond)
            {
                var value = UnityEngine.Random.Range(5, 9) * 10;
                EffectDiamond(itemStart, value);
            }
            var itemGround = PoolManager.Instance.GetItemGroup(this.transform.parent).GetComponent<ItemGroup>();
            itemGround.transform.position = item.transform.position;
            if (aspect > 1.7778f)
                itemGround.transform.localScale = new Vector3(scale, scale, scale);
            itemGround.transform.up = item.transform.position - itemStart.transform.position;
            itemGround.gameObject.SetActive(true);
            dicMap[key].SetData(0);
            itemGround.SetData(item.Amount);
            itemGround.TweenGroup(GetColor(blockAmount), time * 1.3f);
        }
        // // Effect
        item.transform.DOScale(Vector3.one, time * 1.1f).OnComplete(() =>
         {
             dicMap[item.Key].SetData(blockAmount);
             dicMap[item.Key].Diamond(false);
             item.transform.SetAsLastSibling();
             item.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.12f).SetEase(Ease.OutCubic).OnComplete(() =>
                 {
#if !UNITY_EDITOR
                     if (GameManager.Instance.IsVibrate)
                     {
                         Vibration.VibratePredefined(Vibration.PredefinedEffect.EFFECT_CLICK);
                     }
#endif
                     item.transform.DOScale(Vector3.one, 0.12f).SetEase(Ease.OutCubic).OnComplete(() =>
                     {
                         ReSortMap(item, listkey, time * 1.2f);
                     });
                 });
         });

    }
    public void BrokenEffect(ItemPlay item)
    {
        //broken_square.transform.position = item.transform.position;
        //broken_square.gameObject.SetActive(true);
        //foreach (Transform child in broken_square.transform)
        //{
        //    ParticleSystem.MainModule settings = child.GetComponent<ParticleSystem>().main;
        //    settings.startColor = new ParticleSystem.MinMaxGradient(Map.Instance.GetColor(item.Amount));
        //}
        //broken_square.Play();
    }
    public void ImpactEffect(ItemPlay item)
    {

        impact_square.transform.position = item.GetImpactPostion();
        impact_square.gameObject.SetActive(true);
        foreach (Transform child in impact_square.transform)
        {
            ParticleSystem.MainModule settings = child.GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(Map.Instance.GetColor(item.Amount));
        }
        impact_square.Play();
    }
    public void ChangeEffect(ItemPlay item0, ItemPlay item1)
    {

        //change_square0.transform.position = item0.transform.position;
        //change_square0.gameObject.SetActive(true);
        //change_square1.transform.position = item1.transform.position;
        //change_square1.gameObject.SetActive(true);
        //foreach (Transform child in change_square0.transform)
        //{
        //    ParticleSystem.MainModule settings = child.GetComponent<ParticleSystem>().main;
        //    settings.startColor = new ParticleSystem.MinMaxGradient(Map.Instance.GetColor(item0.Amount));
        //}
        //foreach (Transform child in change_square1.transform)
        //{
        //    ParticleSystem.MainModule settings = child.GetComponent<ParticleSystem>().main;
        //    settings.startColor = new ParticleSystem.MinMaxGradient(Map.Instance.GetColor(item1.Amount));
        //}
        //change_square0.Play();
        //change_square1.Play();
    }
    public void MergeEffect(ItemPlay item)
    {


    }
    private void EffectDiamond(ItemPlay item, int value)
    {
        SoundManager.Instance.PlaySound("sfx_block_diamond");
        StartCoroutine(EffectDiamond());
        IEnumerator EffectDiamond()
        {
            int ndiamond = 1;
            while (ndiamond > 0)
            {
                yield return new WaitForSeconds(0.2f);
                var ItemEfect = Instantiate(ItemDiamondEfect, item.transform.position, Quaternion.identity, this.transform.parent.transform);
                ItemEfect.gameObject.SetActive(true);
                ItemEfect.MoveTaget(DiamondTaget.transform.position, 1f, () =>
                {
                    GameManager.ChangeDiamond(value);
                    impact_diamond.Play();
                    ItemEfect.gameObject.SetActive(false);
                });
                ndiamond--;
            }
        }

    }
    private void ReSortMap(ItemPlay item, List<string> listkey, float timeSort)
    {
        // // Sort
        ItemPlay itemcheck = null;
        bool hasmove = false;
        for (int k = 0; k < listkey.Count; k++)
        {
            string key = listkey[k];
            dicMap[key].SetData(0);
            int row = System.Convert.ToInt32(key.Split('-')[0]);
            int col = System.Convert.ToInt32(key.Split('-')[1]);
            for (int i = row; i < GameConfig.MaxRow; i++)
            {
                if (i == GameConfig.MaxRow - 1)
                {
                    string curent = GetKey(i, col);
                    dicMap[curent].SetData(0);
                }
                else
                {
                    string curent = GetKey(i, col);
                    string next = GetKey(i + 1, col);
                    var taget = dicMap[curent];
                    var itemStart = dicMap[next];
                    var amount = itemStart.Amount;
                    var diamond = itemStart.IsDiamond;
                    if (amount > 0)
                    {
                        SoundManager.Instance.PlaySound("sfx_block_replace");

                        hasmove = true;
                        var itemMove = PoolManager.Instance.GetItemMove(this.transform.parent).GetComponent<ItemMove>();
                        if (aspect > 1.7778f)
                            itemMove.transform.localScale = new Vector3(scale, scale, scale);
                        itemMove.transform.position = itemStart.transform.position;
                        itemMove.SetData(amount);
                        itemMove.gameObject.SetActive(true);
                        dicMap[next].SetData(0);
                        dicMap[curent].SetData(amount);
                        dicMap[curent].Diamond(diamond);
                        itemMove.MoveTaget(taget.transform.position, timeSort - 0.01f, () =>
                        {
                            itemMove.gameObject.SetActive(false);
                            dicMap[curent].SetData(amount);
                        });
                    }
                    else if (i + 2 < GameConfig.MaxRow)
                    {
                        string next1 = GetKey(i + 2, col);
                        itemStart = dicMap[next1];
                        amount = itemStart.Amount;
                        diamond = itemStart.IsDiamond;
                        if (amount > 0)
                        {
                            SoundManager.Instance.PlaySound("sfx_block_replace");
                            hasmove = true;
                            var itemMove = PoolManager.Instance.GetItemMove(this.transform.parent).GetComponent<ItemMove>();
                            if (aspect > 1.7778f)
                                itemMove.transform.localScale = new Vector3(scale, scale, scale);
                            itemMove.transform.position = itemStart.transform.position;
                            //var itemMove = Instantiate(ItemMove, itemStart.transform.position, Quaternion.identity, this.transform.parent.transform).GetComponent<ItemMove>();
                            itemMove.SetData(amount);
                            itemMove.gameObject.SetActive(true);
                            dicMap[next].SetData(0);
                            dicMap[curent].SetData(amount);
                            dicMap[curent].Diamond(diamond);
                            itemMove.MoveTaget(taget.transform.position, timeSort - 0.01f, () =>
                            {
                                itemMove.gameObject.SetActive(false);
                                dicMap[curent].SetData(amount);
                            });
                        }
                    }

                }

            }
            if (col == item.Col)
            {
                if (listkey.Count == 1)
                {
                    itemcheck = item;
                }
                else
                {
                    itemcheck = dicMap[key];
                }

            }
        }
        item.transform.DOScale(Vector3.one, hasmove ? 2 * timeSort + 0.02f : timeSort + 0.02f).OnComplete(() =>
             {
                 if (itemcheck != null)
                 {
                     CheckGroup(itemcheck);
                 }
                 else
                 {
                     CheckGroup(item);
                 }
             });
    }
    private void RandomAmount()
    {
        var index = Random.Range(0, 100);
        // //
        if (index < rt2)
            currentAmount = listAmount[0];
        else if (index < rt4)
            currentAmount = listAmount[1];
        else if (index < rt8)
            currentAmount = listAmount[2];
        else if (index < rt16)
            currentAmount = listAmount[3];
        else if (index < rt32)
            currentAmount = listAmount[4];
        else if (index < rt64)
            currentAmount = listAmount[5];
        // //
        switch (currentAmount)
        {
            case 2:
                count_2++;
                break;
            case 4:
                count_4++;
                break;
            case 8:
                count_8++;
                break;
            case 16:
                count_16++;
                break;
            case 32:
                count_32++;
                break;
            case 64:
                count_64++;
                break;

        }
        isDiaMond = false;
        count_ChangeBlock++;
        if (count_ChangeBlock > 20)
        {
            int check = UnityEngine.Random.Range(0, 10);
            if (check < 2)
            {
                isDiaMond = true;
                count_ChangeBlock = 0;
            }

        }
        CurentItem.SetData(currentAmount);
        CurentItem.Diamond(isDiaMond);

    }
    #endregion
    private void OnDiamondChange(object obj)
    {
        this.diamond = (int)obj;
        txtDiamond.text = Utils.FormatNumber(this.diamond);
    }
    public void StartAnimition()
    {
        //skeletonDrop.AnimationState.SetAnimation(0, "idle", false);
        //a++;
        //switch (a)
        //{
        //    case 0:
        //        {
        //            skeletonDrop.AnimationState.SetAnimation(0, "idle", true);
        //            break;
        //        }
        //    case 1:
        //        {
        //            skeletonDrop.AnimationState.SetAnimation(0, "broken", true);
        //            break;
        //        }
        //    case 2:
        //        {
        //            skeletonDrop.AnimationState.SetAnimation(0, "click-idle", true);
        //            //skeletonDrop.AnimationState.Complete += CompleteClickIdle;
        //            break;
        //        }
        //    case 3:
        //        {
        //            skeletonDrop.AnimationState.SetAnimation(0, "click", true);
        //            break;
        //        }
        //    default:
        //        {
        //            a = 0;
        //            skeletonDrop.AnimationState.SetAnimation(0, "idle", true);
        //            break;
        //        }
        //}

    }
    private void SaveTopRank(long Score)
    {
        List<long> listRank = LocalStore.GetTopRank();
        listRank.Add(Score);
        LocalStore.SaveRank(listRank);
    }
    public void Replay()
    {
        // // Refresh map
        LocalStore.SetMaxBlock(GameConfig.MaxBlock);
        LocalStore.SetScoreNewBlock(GameConfig.ScoreNewBlock);
        LocalStore.SetInt(LocalStore.LS_Block_Number_Min, 2);
        LocalStore.SetScore(0);
        LocalStore.SetSwapPrice(GameConfig.Price_Skill);
        LocalStore.SetDropPrice(GameConfig.Price_Skill);
        LocalStore.SaveMap();
        LocalStore.SetInt(LocalStore.LS_Revive, 0);
        foreach (var key in dicMap.Keys)
        {
            dicMap[key].SetData(0);
        }
        LocalStore.SaveAmount(null);
        LocalStore.SetInt(LocalStore.LS_Block_Change, 0);
        StartMatch();
    }
    private void CheckNewScore()
    {
        if (maxBlockAmount >= GameConfig.MaxBlock && maxBlockAmount > MaxBlock)
        {
            isNewScore = true;
            //popupNewScore.Show(MaxBlock, maxBlockAmount, () =>
            // {
            //     isNewScore = false;
            //     if (isChangeBlock)
            //     {
            //         if (IsAdBlock())
            //         {
            //             ChangeBlock();
            //         }
            //     }
            // });

            isNewScore = false;
            if (isChangeBlock)
            {
                if (IsAdBlock())
                {
                    ChangeBlock();
                }
            }

            MaxBlock = maxBlockAmount;
            LocalStore.SetMaxBlock(MaxBlock);
            foreach (var key in dicMap.Keys)
            {
                dicMap[key].ChangeMaxBlock(MaxBlock);
            }
        }
    }
    private void CheckNewBlockNumber()
    {
        if (maxBlockAmount > ScoreNewBlock)
        {
            isChangeBlock = true;
            if (IsAdBlock() && !isNewScore)
            {
                ChangeBlock();
            }
        }
        else if (isChangeBlock && IsAdBlock() && !isNewScore)
        {
            ChangeBlock();
        }

    }
    private void ChangeBlock()
    {
        ScoreNewBlock = ScoreNewBlock * 2;
        LocalStore.SetScoreNewBlock(ScoreNewBlock);
        var remove = listAmount[0];
        var ads = listAmount[listAmount.Count - 1] * 2;
        listAmount.Add(ads);
        listAmount.RemoveAt(0);
        countBlockChange++;
        LocalStore.SetInt(LocalStore.LS_Block_Change, countBlockChange);
        LocalStore.SaveAmount(listAmount);
        currentAmount = listAmount[0];
        CurentItem.SetData(currentAmount);
        isChangeBlock = false;
        foreach (var key in dicMap.Keys)
        {
            dicMap[key].SetData(dicMap[key].Amount);
        }
        Bridge.instance.ShowInterstitial();
        if (ads == 128)
            Bridge.instance.SendEvent(GameConfig.Event_Unlock_128);
    }
    private bool IsAdBlock()
    {
        foreach (var key in dicMap.Keys)
        {
            if (dicMap[key].Amount == listAmount[0])
                return false;

        }
        return true;
    }
    private string GetKeyEnter(int col)
    {
        string key = null;
        for (int i = 0; i < GameConfig.MaxRow; i++)
        {
            int row = i;
            string temp = GetKey(row, col);
            if (dicMap[temp].Amount == 0)
            {
                key = temp;
                break;
            }
            if (i == GameConfig.MaxRow - 1 && dicMap[temp].Amount == currentAmount)
            {
                key = temp;
            }
        }
        return key;

    }
    private string GetKeyExit(int col)
    {
        string key = null;
        for (int i = GameConfig.MaxRow - 1; i >= 0; i--)
        {
            int row = i;
            string temp = GetKey(row, col);
            if (dicMap[temp].Amount > 0)
            {
                key = temp;
                break;
            }
        }
        return key;

    }
    private string GetKey(int row, int col)
    {
        return row + "-" + col;
    }
    IEnumerator StartTimer()
    {
        while (IsPlaying)
        {
            yield return new WaitForSeconds(1);
            matchTime++;
            if (matchTime == 180 || matchTime == 900 || matchTime == 2700 || matchTime == 4200)
            {
                Bridge.instance.ShowInterstitial();
            }
            txtTime.text = Utils.TimeString(matchTime);
        }

    }

    // // 
    public void TopRank()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        //popupRank.Show();
    }
    public void Setting()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        popupSetting.Show();
    }
    public void Shop()
    {
        //popupShop.Show();
    }

    public void OnUsingItem(SkillType type)
    {
        if (type == SkillType.Swap)
        {
            enableSwap = true;
        }
        else if (type == SkillType.Drop)
        {
            enableDrop = true;
        }
        ImageBgSkill.gameObject.SetActive(true);
        skillGuide.Show(type, OnCancleUseSkill);
    }

    private void OnCancleUseSkill(SkillType type)
    {
        isUseSkill = false;
        ImageBgSkill.gameObject.SetActive(false);
        if (type == SkillType.Swap)
        {
            if (!string.IsNullOrEmpty(itemSwap_0))
                dicMap[itemSwap_0].Tick(false);
            if (!string.IsNullOrEmpty(itemSwap_1))
                dicMap[itemSwap_1].Tick(false);
            // //
            itemSwap_0 = null;
            itemSwap_1 = null;
            enableSwap = false;
        }
        else if (type == SkillType.Drop)
        {
            enableDrop = false;
        }
    }
    private void OnUseSkill(SkillType type)
    {
        isUseSkill = false;
        ImageBgSkill.gameObject.SetActive(false);
        if (type == SkillType.Swap)
        {
            if (SwapCount > 0)
            {
                SwapCount--;
                LocalStore.SetInt(LocalStore.LS_Swap, SwapCount);
                ItemSwap.SetData(SwapCount, SwapPrice);
            }
            else
            {
                LocalStore.SetSwapPrice(SwapPrice);
                ItemSwap.SetData(SwapCount, SwapPrice);
            }
        }
        else if (type == SkillType.Drop)
        {
            //Debug.Log("OnUseSkill: " + DropCount);
            if (DropCount > 0)
            {
                DropCount--;
                LocalStore.SetInt(LocalStore.LS_Drop, DropCount);
                ItemDrop.SetData(DropCount, DropPrice);
            }
            else
            {
                LocalStore.SetDropPrice(DropPrice);
                ItemDrop.SetData(DropCount, DropPrice);
            }
        }
        skillGuide.gameObject.SetActive(false);
    }

    public void OnBuyItem(SkillType type)
    {
        //if (type == SkillType.Swap)
        //{
        //    if (diamond >= SwapPrice)
        //    {
        //        GameManager.ChangeDiamond(-SwapPrice);
        //        SwapCount++;
        //        SwapPrice += 10;
        //        LocalStore.SetInt(LocalStore.LS_Swap, SwapCount);
        //        LocalStore.SetSwapPrice(SwapPrice);
        //        ItemSwap.SetData(SwapCount, SwapPrice);
        //        OnUsingItem(type);
        //    }
        //    else
        //    {
        //        popupShop.Show();
        //    }
        //}
        //else if (type == SkillType.Drop)
        //{
        //    if (diamond >= DropPrice)
        //    {
        //        GameManager.ChangeDiamond(-DropPrice);
        //        DropCount++;
        //        DropPrice += 10;
        //        LocalStore.SetInt(LocalStore.LS_Drop, SwapCount);
        //        LocalStore.SetDropPrice(SwapPrice);
        //        ItemDrop.SetData(SwapCount, DropPrice);
        //        OnUsingItem(type);
        //    }
        //    else
        //    {
        //        popupShop.Show();
        //    }
        //}
    }
    public void OnBySwap()
    {
        if (diamond > SwapPrice)
        {
            enableSwap = true;
        }
        else
        {
            //popupShop.Show();
        }
    }
    public void OnByDrop()
    {
        if (diamond > DropPrice)
        {
            enableDrop = true;
        }
        else
        {
            //popupShop.Show();
        }
    }
    public void ShowTest()
    {
        popupTest.Show(OnchangeRate, listrt);
    }
    //public Sprite GetSprite(long amount)
    //{
    //    long min = listAmount[0];
    //    if (amount == min)
    //        return sprites[countBlockChange % 17];
    //    if (amount == min * Math.Pow(2, 1))
    //        return sprites[(countBlockChange + 1) % 17];
    //    if (amount == min * Math.Pow(2, 2))
    //        return sprites[(countBlockChange + 2) % 17];
    //    if (amount == min * Math.Pow(2, 3))
    //        return sprites[(countBlockChange + 3) % 17];
    //    if (amount == min * Math.Pow(2, 4))
    //        return sprites[(countBlockChange + 4) % 17];
    //    if (amount == min * Math.Pow(2, 5))
    //        return sprites[(countBlockChange + 5) % 17];
    //    if (amount == min * Math.Pow(2, 6))
    //        return sprites[(countBlockChange + 6) % 17];
    //    if (amount == min * Math.Pow(2, 7))
    //        return sprites[(countBlockChange + 7) % 17];
    //    if (amount == min * Math.Pow(2, 8))
    //        return sprites[(countBlockChange + 8) % 17];
    //    if (amount == min * Math.Pow(2, 9))
    //        return sprites[(countBlockChange + 9) % 17];
    //    if (amount == min * Math.Pow(2, 10))
    //        return sprites[(countBlockChange + 10) % 17];
    //    if (amount == min * Math.Pow(2, 11))
    //        return sprites[(countBlockChange + 11) % 17];
    //    if (amount == min * Math.Pow(2, 12))
    //        return sprites[(countBlockChange + 12) % 17];
    //    if (amount == min * Math.Pow(2, 13))
    //        return sprites[(countBlockChange + 13) % 17];
    //    if (amount == min * Math.Pow(2, 14))
    //        return sprites[(countBlockChange + 14) % 17];
    //    if (amount == min * Math.Pow(2, 15))
    //        return sprites[(countBlockChange + 15) % 17];
    //    if (amount == min * Math.Pow(2, 16))
    //        return sprites[(countBlockChange + 16) % 17];

    //    return sprites[(countBlockChange + 16) % 17];
    //}

    public GameObject SetMemePrefab (long amount, Transform parent)
    {
        long min = listAmount[0];
        GameObject obj = null;
        if (amount == min)
            obj = sprites[countBlockChange % 17];
        else if (amount == min * Math.Pow(2, 1))
            obj = sprites[(countBlockChange + 1) % 17];
        else if (amount == min * Math.Pow(2, 2))
            obj = sprites[(countBlockChange + 2) % 17];
        else if (amount == min * Math.Pow(2, 3))
            obj = sprites[(countBlockChange + 3) % 17];
        else if (amount == min * Math.Pow(2, 4))
            obj = sprites[(countBlockChange + 4) % 17];
        else if (amount == min * Math.Pow(2, 5))
            obj = sprites[(countBlockChange + 5) % 17];
        else if (amount == min * Math.Pow(2, 6))
            obj = sprites[(countBlockChange + 6) % 17];
        else if (amount == min * Math.Pow(2, 7))
            obj = sprites[(countBlockChange + 7) % 17];
        else if (amount == min * Math.Pow(2, 8))
            obj = sprites[(countBlockChange + 8) % 17];
        else if (amount == min * Math.Pow(2, 9))
            obj = sprites[(countBlockChange + 9) % 17];
        else if (amount == min * Math.Pow(2, 10))
            obj = sprites[(countBlockChange + 10) % 17];
        else if (amount == min * Math.Pow(2, 11))
            obj = sprites[(countBlockChange + 11) % 17];
        else if (amount == min * Math.Pow(2, 12))
            obj = sprites[(countBlockChange + 12) % 17];
        else if (amount == min * Math.Pow(2, 13))
            obj = sprites[(countBlockChange + 13) % 17];
        else if (amount == min * Math.Pow(2, 14))
            obj = sprites[(countBlockChange + 14) % 17];
        else if (amount == min * Math.Pow(2, 15))
            obj = sprites[(countBlockChange + 15) % 17];
        else if (amount == min * Math.Pow(2, 16))
            obj = sprites[(countBlockChange + 16) % 17];
        else 
            obj = sprites[(countBlockChange + 16) % 17];

        objectPoolManager.SpawnGameObject(obj);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.SetParent(parent);

        return obj;
    }    

    public Sprite GetAmountSpriteByChar(char v)
    {
        switch (v)
        {
            case '0':
                return amountSprites[0];
            case '1':
                return amountSprites[1];
            case '2':
                return amountSprites[2];
            case '3':
                return amountSprites[3];
            case '4':
                return amountSprites[4];
            case '5':
                return amountSprites[5];
            case '6':
                return amountSprites[6];
            case '7':
                return amountSprites[7];
            case '8':
                return amountSprites[8];
            case '9':
                return amountSprites[9];
            case 'K':
                return amountSprites[10];
            case 'M':
                return amountSprites[11];
            case 'B':
                return amountSprites[12];
            case 'L':
                return amountSprites[13];
            default:
                return amountSprites[0];
        }
    }
    public Color GetColor(long amount)
    {
        long min = listAmount[0];
        if (amount == min)
            return colors[countBlockChange % 17];
        if (amount == min * Math.Pow(2, 1))
            return colors[(countBlockChange + 1) % 17];
        if (amount == min * Math.Pow(2, 2))
            return colors[(countBlockChange + 2) % 17];
        if (amount == min * Math.Pow(2, 3))
            return colors[(countBlockChange + 3) % 17];
        if (amount == min * Math.Pow(2, 4))
            return colors[(countBlockChange + 4) % 17];
        if (amount == min * Math.Pow(2, 5))
            return colors[(countBlockChange + 5) % 17];
        if (amount == min * Math.Pow(2, 6))
            return colors[(countBlockChange + 6) % 17];
        if (amount == min * Math.Pow(2, 7))
            return colors[(countBlockChange + 7) % 17];
        if (amount == min * Math.Pow(2, 8))
            return colors[(countBlockChange + 8) % 17];
        if (amount == min * Math.Pow(2, 9))
            return colors[(countBlockChange + 9) % 17];
        if (amount == min * Math.Pow(2, 11))
            return colors[(countBlockChange + 10) % 17];
        if (amount == min * Math.Pow(2, 12))
            return colors[(countBlockChange + 11) % 17];
        if (amount == min * Math.Pow(2, 13))
            return colors[(countBlockChange + 12) % 17];
        if (amount == min * Math.Pow(2, 14))
            return colors[(countBlockChange + 13) % 17];
        if (amount == min * Math.Pow(2, 15))
            return colors[(countBlockChange + 14) % 17];
        if (amount == min * Math.Pow(2, 16))
            return colors[(countBlockChange + 15) % 17];
        if (amount == min * Math.Pow(2, 17))
            return colors[(countBlockChange + 16) % 17];
        return colors[(countBlockChange + 16) % 17];

    }
    private void OnchangeRate(List<float> list)
    {
        listrt = list;
        rt2 = list[0];
        rt4 = rt2 + list[1];
        rt8 = rt4 + list[2];
        rt16 = rt8 + list[3];
        rt32 = rt16 + list[4];
        rt64 = rt32 + list[5];
        if (coroutine != null)
            StopCoroutine(coroutine);
        Replay();
    }
    private void OnDestroy()
    {
        GameManager.UnRegister(Event_e.DiamonChange, OnDiamondChange);
    }
    List<float> listrt = new List<float>();
    Coroutine coroutine;
    private bool isDiaMond;
    private bool enableSwap;
    private bool enableDrop;
    private bool isMoveComplete;
    private bool isChangeBlock;
    private int combo, SwapCount, DropCount, SwapPrice, DropPrice, ReviveCount;
    string itemSwap_0, itemSwap_1;
    int CurentLineCol, count_ChangeBlock;
    long blockAmount, maxBlockAmount, MaxBlock, diamond;
    long ScoreNewBlock;
    int countBlockChange;
    int count_2, count_4, count_8, count_16, count_32, count_64;
    List<long> listAmount = new List<long>();
    float aspect, scale;
    float time = 0.12f;
    long timeTest = 0;
    bool isNewScore = false;
    bool isUseSkill = false;
    bool isFirst = true;
    int countTutorial;
    int comboCount;
    private int count_highScore;
}
