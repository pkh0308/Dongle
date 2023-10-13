using System.Collections.Generic;

public interface ILoader<Key, Value>
{
    public abstract Dictionary<Key, Value> MakeDic();
}