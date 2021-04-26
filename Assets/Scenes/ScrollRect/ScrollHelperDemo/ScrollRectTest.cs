using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class ScrollRectTest:MonoBehaviour
{
    [SerializeField]
    private GameObject Item;
    [SerializeField]
    private GameObject ViewPort;
    [SerializeField]
    private GameObject Content;

    List<ViewBaseDemo> viewBaseDemos = new List<ViewBaseDemo>();

    private ScrollRectHelperGeneric scrollRectHelper;

    private  ScrollRectHelper2 viewBase;
    private void Start()
    {        
        for(int index=50;index>0;index--)
        {
            viewBaseDemos.Add(new ViewBaseDemo());
        }
        //泛型
        viewBase = this.gameObject.AddComponent<ScrollRectHelper2>();
        viewBase.Init<ViewBaseDemo>(viewBaseDemos,Item,ViewPort,Content);

        //非泛型
        //scrollRectHelper= this.gameObject.AddComponent<ScrollRectHelperGeneric>();
        //scrollRectHelper.Init(viewBaseDemos, Item, ViewPort, Content);
    }
    public void DeleteItem()
    {
        viewBase.DeleteItem<ViewBaseDemo>();
    }
    public void AddItem()
    {
        viewBase.AddItem<ViewBaseDemo>();
        //scrollRectHelper.AddItem();
    }
    public void ScrollInit()
    {
        viewBase.Init<ViewBaseDemo>(viewBaseDemos, Item, ViewPort, Content);
    }
    public void HideScrollRect()
    {
        viewBase.Hide();
    }
}

