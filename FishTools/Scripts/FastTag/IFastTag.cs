using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishTools
{
    public interface IFastTag
    {
        string FID { get; }
    }

}

///
/// 使用例
///


//  public class MyBAg : MonoBehaviour, IFastTag
//     {
//         public string fid = "mybag01";
//         public string FID => fid;

//         private void Awake()
//         {
//             FastTag<MyBAg>.Register(this);
//         }
//         private void OnDestroy() //使用这个可以当物体为激活时也能找到
//         {
//             FastTag<MyBAg>.UnRegister(this);
//         }
//     }
//     public class TestFindFastTag : MonoBehaviour
//     {
//         public void TestFind()
//         {
//             Debug.Log(FastTag<MyBAg>.FindByID("mybag01"));
//         }
//     }
