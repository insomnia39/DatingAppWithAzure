using DatingApp.DAL.Repository;

namespace DatingApp.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IMessageGroupRepository MessageGroupRepository { get; }
        IMessageRepository MessageRepository { get; }
        IUserRepository UserRepository { get; }
    }
}
