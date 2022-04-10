using System;
using System.Threading.Tasks;
using Core;

namespace DAL.Abstractions.Interfaces;

public interface IUnitOfWork : IDisposable
{
    void CreateTransaction();

    void Commit();

    void Rollback();

    void Save();

    // IGenericRepository<T> GenericRepositoryDb<T>() where T : class;

    IGenericRepository<User> UserRepository { get; }
    
    IGenericRepository<Role> RoleRepository { get; }

    IGenericRepository<Room> RoomRepository { get; }

    IGenericRepository<TextChannel> TextChannelRepository { get; }

    IGenericRepository<PersonalChat> PersonalChatRepository { get; }

    IGenericRepository<InviteLink> InviteLinkRepository { get; }
    IGenericRepository<InviteLinksUsers> InviteLinksUsersRepository { get; }
    IGenericRepository<UsersPersonalChats> UsersPersonalChats { get; }
}
