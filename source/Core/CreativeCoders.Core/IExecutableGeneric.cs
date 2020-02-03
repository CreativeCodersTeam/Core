namespace CreativeCoders.Core
{
    public interface IExecutable<in T>
    {
        void Execute(T parameter);
    }
}