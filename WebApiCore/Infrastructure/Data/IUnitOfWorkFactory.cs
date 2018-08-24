namespace WebApiCore.Infrastructure.Data
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}