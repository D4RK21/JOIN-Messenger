using System;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DAL;

public class AppContext : DbContext
{
    private readonly AppSettings _appSettings;
    
    // public AppContext(IOptions<AppSettings> appSettings)
    // {
    //     _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
    // }
    
    public DbSet<User> Users { get; set; }

    public DbSet<Room> Rooms { get; set; }
    
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<TextChannel> TextChannels { get; set; }

    public DbSet<ParticipantInfo> ParticipantInfos { get; set; }

    public DbSet<PersonalChat> PersonalChats { get; set; }
    
    public DbSet<UsersPersonalChats> UsersPersonalChats { get; set; }

    public DbSet<InviteLink> InviteLinks { get; set; }
    
    public DbSet<InviteLinksUsers> InviteLinksUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // _appSettings.ConnectionString
        optionsBuilder.UseSqlServer("Server=LAPTOP-AJN7TDOI\\SQLEXPRESS;Database=AimAppDb;Trusted_connection=true");
    }
}
