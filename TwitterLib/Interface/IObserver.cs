namespace TwitterLib.Interface
{
    public interface IObserver
    {
        Task Update(string tweet);
    }
}