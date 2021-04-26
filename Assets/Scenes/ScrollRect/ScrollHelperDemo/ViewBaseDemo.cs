using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ViewBaseDemo : MonoBehaviour, IViewBase
{
    private static int ID;
    private int ID_Mine=-1;
    public void Init(object data = null)
    {
        if(ID_Mine==-1)
        {
            Debug.Log($"数据初始化ID={ID++}");
            ID_Mine = ID;
        }  
        else
        {
            Debug.Log($"数据初始化ID={ID_Mine}");
        }
        this.gameObject.name = ID_Mine.ToString();
    }
}

