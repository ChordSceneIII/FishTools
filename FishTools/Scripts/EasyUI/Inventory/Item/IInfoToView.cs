using System;

public interface IInfoToView
{
    public int ICount { get; set; }
    public int IMaxCount { get; }
    public Enum ITypeValue { get; }
}