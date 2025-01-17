using UnityEngine;
using UnityEditor;
using FishTools.EasyUI;

namespace FishToolsDEMO
{

[CreateAssetMenu(fileName = "TestType", menuName = "FishTools/DEMO/TestType")]
public class TestType : BaseItemType<TestTypeEnum>
{

}

public enum TestTypeEnum
{
    A,
    B,
    C
}
}