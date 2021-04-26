using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectHelper: MonoBehaviour
{
    #region Field
    /// <summary>
    /// 实例化的预制体
    /// </summary>
    private GameObject Item;
    /// <summary>
    /// 滑动条上的视野物体
    /// </summary>
    private GameObject ViewPort;
    /// <summary>
    /// 滑动条上实例化物体的父物体
    /// </summary>
    private GameObject Content;
    /// <summary>
    /// 实例化物体的总数
    /// </summary>
    private int Total;
    /// <summary>
    /// 当前实例化物体的系数
    /// </summary>
    private int CurrentIndex;
    /// <summary>
    /// 滑动列表初始化数据
    /// </summary>
    private List<object> DataList = new List<object>();

    /// <summary>
    /// 删除元素时使用的栈
    /// </summary>
    private Stack<int> DeleteItemStack = new Stack<int>();
    /// <summary>
    /// 初始化时实例化的子物体数量
    /// </summary>
    private int InitNumber;
    #endregion

    #region Public Method
    /// <summary>
    /// 掉用初始化接口
    /// </summary>
    /// <param name="data"></param>
    public void Init<DataType>(List<DataType> datalist, GameObject item,GameObject ViewPort,GameObject content)where DataType : ViewBaseDemo
    {
        this.gameObject.GetComponent<ScrollRect>().verticalScrollbar.value = 1;
        this.DataList = datalist.ConvertAll(s => (object)s); ;
        this.Item = item;
        this.ViewPort = ViewPort;
        this.Content = content;
        this.InitNumber = InitCountInit();
        Total = DataList.Count;
        this.gameObject.GetComponent<ScrollRect>().onValueChanged.RemoveListener(ContentUpdate<DataType>);
        this.gameObject.GetComponent<ScrollRect>().onValueChanged.AddListener(ContentUpdate<DataType>);
        Refresh<DataType>();
        CurrentIndex = GetActiveChildCount();
    }
    public void DeleteItem<DataType>() where DataType : ViewBaseDemo
    {
        int nub = Random.Range(0, DataList.Count);
        if(DataList.Count<= nub)
        {
            return;
        }
        DataList[nub] = null;
        RefreshData();
        if (Content.transform.childCount>= nub)
        {
            Content.transform.GetChild(nub).gameObject.SetActive(false);
            Content.transform.GetChild(nub).SetAsLastSibling();
        }
        //DeleteRefresh<DataType>();
    }
    public void AddItem<DataType>() where DataType : ViewBaseDemo
    {
        var item = new ViewBaseDemo();
        DataList.Add(item);
        RefreshData();
        CurrentIndex = GetActiveChildCount();
        //AddRefresh<DataType>();
    }
    public void Hide()
    {
        foreach(Transform trans in Content.transform)
        {
            trans.gameObject.SetActive(false);
        }
        CurrentIndex = GetActiveChildCount();
    }
    #endregion
    #region Private Method
    /// <summary>
    /// 刷新界面以及界面的信息
    /// </summary>
    private void Refresh<DataType>() where DataType: ViewBaseDemo
    {
        DataType item;
        CurrentIndex = 0;
        foreach(Transform child in Content.transform)
        {
            child.gameObject.SetActive(false);
        }

        foreach (var data in DataList)
        {
            if(data==null)
            {
                DataList.Remove(data);
                continue;
            }
            if (Content.transform.childCount > CurrentIndex)
            {
                item = Content.transform.GetChild(CurrentIndex).GetComponent<DataType>();
            }
            else
            {
                item = Instantiate(Item, Content.transform).GetComponent<DataType>();                
            }
            item.gameObject.SetActive(true);
            item.transform.localScale = Vector3.one;
            item.Init(DataList[CurrentIndex]);
            CurrentIndex++;
            //实例化物体区域已经超出视野范围，不再继续实例化物体
            if(CurrentIndex>= this.InitNumber)
            {
                return;
            }
        }
    }

    private void ContentUpdate<DataType>(Vector2 rect) where DataType : ViewBaseDemo
    {
        DataType item;
       
        if (IsContentRate())
        {
            return;
        }
        if (GetActiveChildCount() >= Total)
        {
            return;
        }
        if (Content.transform.childCount > CurrentIndex)
        {
            item = Content.transform.GetChild(CurrentIndex).GetComponent<DataType>();
        }
        else
        {
            item = Instantiate(Item, Content.transform).GetComponent<DataType>();            
        }
        item.gameObject.SetActive(true);
        item.transform.localScale = Vector3.one;
        item.Init(DataList[CurrentIndex-1]);
        CurrentIndex++;
    }
    /// <summary>
    /// 判断content底部是否低于viewport底部
    /// init循环加载时，获取到的conten的hight始终没有发生变化
    /// </summary>
    /// <returns> </returns>
    private bool IsContentRate()
    {
        float viewBottom = ViewPort.transform.position.y - ViewPort.GetComponent<RectTransform>().rect.height;
        float contentBottom= Content.transform.position.y- Content.GetComponent<RectTransform>().rect.height;        
        if (contentBottom >= viewBottom)
        {
            //Debug.LogError("Content 底部更高，返回false");
        }
        else
        {
            //Debug.LogError("Viewport 底部更高,返回true");
        }
        //Debug.LogError($"Content= {contentBottom}，viewBottom= {viewBottom}");
        //Debug.LogError($"Content.transform.position.y= {Content.transform.position.y}，Content.GetComponent<RectTransform>().rect.height= {Content.GetComponent<RectTransform>().rect.height}");
        //Debug.LogError($"Content.GetComponent<RectTransform>().sizeDelta.y= {Content.GetComponent<RectTransform>().sizeDelta.y}");
        return contentBottom >= viewBottom ? false : true;
    }
    /// <summary>
    /// 刷新初始化所需要的数据
    /// </summary>
    private void RefreshData()
    {
        DeleteItemStack.Clear();
        int index = 0;
        foreach (var data in DataList)
        {
            if (data == null)
            {
                DeleteItemStack.Push(index);
            }
            index++;
        }
        while(DeleteItemStack.Count>0)
        {
            DataList.Remove(DataList[DeleteItemStack.Pop()]);
        }
        Total = DataList.Count;
    }
    private int GetActiveChildCount()
    {
        int ActiveCount = 0;
        foreach(Transform trans in Content.transform)
        {
            if(trans.gameObject.activeSelf)
            {
                ActiveCount++;
            }
        }
        return ActiveCount;
    }
    private int InitCountInit()
    {
        return (int)(ViewPort.GetComponent<RectTransform>().rect.height / Item.GetComponent<RectTransform>().rect.height+1);
    }
    #endregion
}

