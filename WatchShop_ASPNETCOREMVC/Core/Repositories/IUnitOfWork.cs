namespace WatchShop_ASPNETCOREMVC.Core.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IRoleRepository Role { get; }
    }
}
