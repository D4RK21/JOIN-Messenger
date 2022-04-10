using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppContext _context;
    private IDbContextTransaction _objTran;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IGenericRepository<Room> _roomRepository;
    private readonly IGenericRepository<TextChannel> _textChannelRepository;
    private readonly IGenericRepository<PersonalChat> _personalChatRepository;
    private readonly IGenericRepository<InviteLink> _inviteLinkRepository;
    private readonly IGenericRepository<InviteLinksUsers> _inviteLinksUsersRepository;
    private readonly IGenericRepository<UsersPersonalChats> _usersPersonalChatsRepository;

    public UnitOfWork(AppContext context)
    {
        this._context = context;
    }

    public IGenericRepository<User> UserRepository => _userRepository ?? new GenericRepositoryDb<User>(_context);

    public IGenericRepository<Role> RoleRepository => _roleRepository ?? new GenericRepositoryDb<Role>(_context);

    public IGenericRepository<Room> RoomRepository => _roomRepository ?? new GenericRepositoryDb<Room>(_context);

    public IGenericRepository<TextChannel> TextChannelRepository =>
        _textChannelRepository ?? new GenericRepositoryDb<TextChannel>(_context);

    public IGenericRepository<PersonalChat> PersonalChatRepository =>
        _personalChatRepository ?? new GenericRepositoryDb<PersonalChat>(_context);

    public IGenericRepository<InviteLink> InviteLinkRepository =>
        _inviteLinkRepository ?? new GenericRepositoryDb<InviteLink>(_context);

    public IGenericRepository<InviteLinksUsers> InviteLinksUsersRepository =>
        _inviteLinksUsersRepository ?? new GenericRepositoryDb<InviteLinksUsers>(_context);

    public IGenericRepository<UsersPersonalChats> UsersPersonalChats =>
        _usersPersonalChatsRepository ?? new GenericRepositoryDb<UsersPersonalChats>(_context);

    public void CreateTransaction()
    {
        _objTran = _context.Database.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            _context.SaveChanges();
            _objTran.Commit();
        }
        catch
        {
            this.Rollback();
        }
    }

    public void Rollback()
    {
        _objTran.Rollback();
        _objTran.Dispose();
    }

    public void Save()
    {
        // try
        // {
        _context.SaveChanges();
        // }
        // catch (DbEntityValidationException dbEx)
        // {
        //     foreach (var validationErrors in dbEx.EntityValidationErrors)
        //         foreach (var validationError in validationErrors.ValidationErrors)
        //             _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
        //     throw new Exception(_errorMessage, dbEx);
        // }
    }

    private bool _disposed = false;

    protected virtual async Task Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                await _context.DisposeAsync();
            }
        }

        this._disposed = true;
    }

    public async void Dispose()
    {
        await Dispose(true);
        GC.SuppressFinalize(this);
    }
}
