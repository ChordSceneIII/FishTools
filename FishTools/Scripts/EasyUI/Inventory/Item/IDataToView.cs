using System;

public interface IDataToView
{
    public int ICount { get; set; }
    public int IMaxCount { get; }
    public Enum ITypeValue { get; }
}