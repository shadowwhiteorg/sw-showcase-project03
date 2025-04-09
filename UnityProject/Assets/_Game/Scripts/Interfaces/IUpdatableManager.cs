namespace _Game.Interfaces
{
    public interface IUpdatableManager
    {
        void Update(float deltaTime);
        public void FixedUpdate(float fixedDeltaTime);
    }
}