//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class FlyViewManager : MonoBehaviour
//{
//    private Camera TargetCamera
//    {
//        get
//        {
//            return Camera.main;
//        }

//    }

//    [SerializeField] private ObjFly coinFlyObj;
//    [SerializeField] private ObjFly hintFlyObj;

//    [SerializeField] private float storeTimeShow;

//    [SerializeField] Text storeSth;
//    Transform targetTrans;
//    Transform flyObjParent;
//    Transform storeObjParent;

//    public static FlyViewManager instance;

//    private List<ObjFly> listFlyObj = new List<ObjFly>();

//    public Transform FlyObjParent
//    {
//        get
//        {
//            return flyObjParent;
//        }
//        set
//        {
//            flyObjParent = value;
//        }
//    }

//    public Transform StoreObjParent
//    {
//        get
//        {
//            return storeObjParent;
//        }
//        set
//        {
//            storeObjParent = value;
//        }
//    }

//    public void SetTargetTran(Transform trans)
//    {
//        targetTrans = trans;
//    }

//    public Text StoreSth
//    {
//        get
//        {
//            return storeSth;
//        }
//        set
//        {
//            storeSth = value;
//        }
//    }

//    public Vector3 MouseUIPos
//    {
//        get
//        {
//            return TargetCamera.ScreenToWorldPoint(Input.mousePosition);
//        }
//    }

//    private Vector3 CenterUIPos
//    {
//        get
//        {
//            return TargetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
//        }
//    }

//    void Awake()
//    {
//        instance = this;
//    }

//    public virtual void Collect(double n, Vector3 position, ObjectFlyType objectFlyType, Callback callBack = null)
//    {
//        coinFake += n;
//        StartCoroutine(IECollectMore(n, position, objectFlyType, callBack));
//    }

//    public void CollectInCenter(double n, ObjectFlyType objectFlyType, Callback callBack = null)
//    {
//        if (n == 0)
//            return;
//        Collect(n, CenterUIPos, objectFlyType, callBack);
//    }

//    public void CollectFromMousePosition(double n, ObjectFlyType objectFlyType, Callback callBack = null)
//    {
//        if (n == 0)
//            return;
//        Collect(n, MouseUIPos, objectFlyType, callBack);
//    }

//    IEnumerator IECollectMore(double totalValue, Vector3 position, ObjectFlyType objectFlyType, Callback callBack = null)
//    {
//        int flyCount = 10;
//        float delayTime = 0.03f;

//        if (totalValue <= flyCount)
//        {
//            for (int i = 0; i < totalValue; i++)
//            {
//                StartCoroutine(IECollectOne(1, position, i, objectFlyType));
//                yield return new WaitForSeconds(delayTime);
//            }
//        }
//        else
//        {
//            double g1 = System.Math.Floor(totalValue / flyCount);
//            double gl = totalValue - flyCount * g1;
//            for (int i = 1; i < flyCount; i++)
//            {
//                StartCoroutine(IECollectOne(g1, position, i, objectFlyType));
//                yield return new WaitForSeconds(delayTime);
//            }
//            StartCoroutine(IECollectOne(gl + g1, position, flyCount, objectFlyType));
//        }

//        yield return new WaitUntil(new System.Func<bool>(delegate
//        {
//            return listFlyObj.Count == 0;
//        }));
//        if (callBack != null)
//            callBack();
//    }

//    IEnumerator IECollectOne(double n, Vector3 position, int index, ObjectFlyType flyType)
//    {
//        var obj = GenerateFlyObj(position, flyType);
//        var rndPos = position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0);
//        float timeWait = 0.5f;
//        obj.FlySlowDownTo(rndPos, timeWait);

//        yield return new WaitForSeconds(timeWait);
//        obj.FlyTo(targetTrans, 0.5f, delegate
//        {
//            OnReceive(n);
//            switch (flyType)
//            {
//                case ObjectFlyType.Coin:
//                    SectionSettings.Coin += (int)n;
//                    EventDispacher<int>.Dispatch(EventName.On_Coin_Changed_By_Fly, (int)n);
//                    break;
//                //case ObjectFlyType.Hint:
//                //    PenManager.instance.dataOfUser.AddHint((long)n);
//                //    break;
//            }

//            ZoomIcon();
//            RemoveFlyObjToList(obj);
//            Destroy(obj.gameObject);
//        }, index);
//    }

//    protected delegate void Receive(double n);

//    protected Receive OnReceive
//    {
//        get
//        {
//            return new Receive(delegate (double n)
//            {
//                coinFake -= n;
//            });
//        }
//    }

//    void AddFlyObjToList(ObjFly obj)
//    {
//        listFlyObj.Add(obj);
//        if (storeObjParent != null)
//        {           
//            if (listFlyObj.Count == 1)
//            {
//                ShowStoreObjParent();
//            }
//        }
//    }

//    void RemoveFlyObjToList(ObjFly obj)
//    {
//        listFlyObj.Remove(obj);
//        if (storeObjParent != null)
//        {
//            if (listFlyObj.Count == 0)
//            {
//                StartCoroutine(IEHideStore(1f));
//            }            
//        }
//    }
   
//    IEnumerator IEHideStore(float seconds)
//    {
//        yield return new WaitForSeconds(seconds);
//        storeObjParent.transform.DOMove(originalStoreParentPosition, 0.25f);
//        //Debug.LogError("Hide " + originalStoreParentPosition);
//    }

//    Vector3 originalStoreParentPosition;

//    void ShowStoreObjParent()
//    {
//        originalStoreParentPosition = storeObjParent.position;
//        Vector3 targetStoreParentPosition = new Vector3(storeObjParent.position.x, storeObjParent.position.y - 1.5f, storeObjParent.position.z);
//        storeObjParent.transform.DOMove(targetStoreParentPosition, 0.25f);
//        //Debug.LogError("Show " + targetStoreParentPosition);
//    }

//    ObjFly GenerateFlyObj(Vector3 position, ObjectFlyType flyType)
//    {
//        GameObject g = null;
//        if (flyType == ObjectFlyType.Coin)
//        {
//            g = Instantiate(coinFlyObj.gameObject, transform) as GameObject;
//        }
//        else if (flyType == ObjectFlyType.Hint)
//        {
//            g = Instantiate(hintFlyObj.gameObject, transform) as GameObject;
//        }
        
//        g.transform.SetParent(flyObjParent);
//        g.transform.position = position;
//        g.transform.localPosition = new Vector3(g.transform.localPosition.x, g.transform.localPosition.y, 0);
//        g.transform.localScale = Vector3.one;
//        AddFlyObjToList(g.GetComponent<ObjFly>());
//        return g.GetComponent<ObjFly>();
//    }

//    private void ZoomIcon()
//    {
//        Vector3 end = 1.2f * Vector3.one;
//        storeSth.transform.DOScale(end, 0.25f).OnComplete(delegate
//        {
//            storeSth.transform.DOScale(Vector3.one, 0.25f);
//        });
//    }

//    double coinFake = 0;
//}

//public enum ObjectFlyType
//{
//    Coin = 0,
//    Hint = 1,
//}