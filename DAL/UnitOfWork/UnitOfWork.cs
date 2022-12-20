using DatingApp.DAL.Repository;
using System;

namespace DatingApp.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProfileContext _context;
        private readonly Lazy<IMessageGroupRepository> _messageGroupRepository;
        private readonly Lazy<IMessageRepository> _messageRepository;
        private readonly Lazy<IUserRepository> _userRepository;

        public UnitOfWork(ProfileContext context)
        {
            _context ??= context;
            _messageGroupRepository ??= new Lazy<IMessageGroupRepository>(new MessageGroupRepository(_context));
            _messageRepository ??= new Lazy<IMessageRepository>(new MessageRepository(_context));
            _userRepository ??= new Lazy<IUserRepository>(new UserRepository(_context));
        }

        public IMessageGroupRepository MessageGroupRepository => _messageGroupRepository.Value;
        public IMessageRepository MessageRepository => _messageRepository.Value;
        public IUserRepository UserRepository => _userRepository.Value;
    }
}
