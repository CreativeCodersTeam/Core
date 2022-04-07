namespace CreativeCoders.Net.XmlRpc.Model;

public abstract class XmlRpcValue
{
    protected XmlRpcValue(object data)
    {
        Data = data;
    }

    public object Data { get; }

    public T GetValue<T>()
    {
        if (Data is T data)
        {
            return data;
        }

        return default;
    }
}

public abstract class XmlRpcValue<T> : XmlRpcValue
{
    protected XmlRpcValue(T value) : base(value)
    {
        Value = value;
    }

    public T Value { get; }
}
