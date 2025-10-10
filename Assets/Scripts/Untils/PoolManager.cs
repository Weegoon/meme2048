using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using DG.Tweening;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private HashSet<GameObject> ItemMovePools = new HashSet<GameObject>();
    private HashSet<GameObject> ItemGroupPools = new HashSet<GameObject>();
    private HashSet<GameObject> mergePools = new HashSet<GameObject>();
    GameObject ItemMove;
    GameObject ItemGroup;
    GameObject merge;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InitPools();
    }
    public void ReFreshPools()
    {
        foreach (var f in ItemMovePools)
        {
            f.SetActive(false);
        }
        foreach (var f in ItemGroupPools)
        {
            f.SetActive(false);
        }
        foreach (var f in mergePools)
        {
            f.SetActive(false);
        }
    }
    private void InitPools()
    {

    }
   
    public GameObject GetItemMove(Transform parent)
    {
        foreach (var go in ItemMovePools)
        {
            if (!go.activeSelf)
            {
                return go;
            }
        }
        return InitItemMove(1, parent);
    }
    private GameObject InitItemMove(int originCount, Transform parent)
    {
        ItemMove = ItemMove ?? ResourcesCache.Load<GameObject>("Items/ItemMove");
        for (int i = 0; i < originCount; i++)
        {
            var item = Instantiate(ItemMove, Vector3.zero, Quaternion.identity, parent);
            item.gameObject.SetActive(false);
            ItemMovePools.Add(item.gameObject);
        }
        return ItemMovePools.Last();
    }
    public GameObject GetMerge(Transform parent)
    {
        foreach (var go in mergePools)
        {
            if (!go.GetComponent<ParticleSystem>().isStopped)
            {
                return go;
            }
        }
        return InitMerge(1, parent);
    }
    private GameObject InitMerge(int originCount, Transform parent)
    {
        merge = merge ?? ResourcesCache.Load<GameObject>("Particles/merge-square");
        for (int i = 0; i < originCount; i++)
        {
            var item = Instantiate(merge, Vector3.zero, Quaternion.identity, parent);
            item.gameObject.SetActive(false);
            mergePools.Add(item.gameObject);
        }
        return mergePools.Last();
    }
    public GameObject GetItemGroup(Transform parent)
    {
        foreach (var go in ItemGroupPools)
        {
            if (!go.activeSelf)
            {
                return go;
            }
        }
        return InitItemGroup(1, parent);
    }
    private GameObject InitItemGroup(int originCount, Transform parent)
    {
        ItemGroup = ItemGroup ?? ResourcesCache.Load<GameObject>("Items/ItemGroup");
        for (int i = 0; i < originCount; i++)
        {
            var item = Instantiate(ItemGroup, Vector3.zero, Quaternion.identity, parent);
            item.gameObject.SetActive(false);
            ItemGroupPools.Add(item.gameObject);
        }
        return ItemGroupPools.Last();
    }
}
