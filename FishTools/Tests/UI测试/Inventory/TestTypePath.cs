using UnityEngine;
using UnityEditor;
using FishTools.EasyUI;

namespace FishTools.Tests
{

[CreateAssetMenu(fileName = "TestType", menuName = "FishTools/DEMO/TestType")]
public class TestPath : BaseItemPath<TestType>
{

}

public enum TestType
{
    A,
    B,
    C
}
}