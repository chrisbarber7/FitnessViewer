namespace FitnessViewer.Infrastructure.Core.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
