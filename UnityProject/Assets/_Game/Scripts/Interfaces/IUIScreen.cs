namespace _Game.Interfaces
{
    public interface IUIScreen<TModel, TView> 
        where TModel : IUIModel 
        where TView : IUIView
    {
        void Initialize(TModel model, TView view, IEventBus eventBus);
        void Show();
        void Hide();
    }
}