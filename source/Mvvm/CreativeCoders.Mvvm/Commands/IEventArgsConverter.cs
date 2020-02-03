namespace CreativeCoders.Mvvm.Commands
{
    public interface IEventArgsConverter
    {
        object Convert(object value, object parameter);
    }
}