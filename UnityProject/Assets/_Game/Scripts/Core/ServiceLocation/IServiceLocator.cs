namespace _Game.Core.ServiceLocation
{
    public interface IServiceLocator
    {
        void Register<T>(T service);
        T Get<T>() where T : class;
        void Unregister<T>();
    }
}