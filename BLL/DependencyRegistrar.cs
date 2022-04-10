using BLL.Abstractions.Interfaces;
using BLL.Helpers;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class DependencyRegistrar
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IUserValidator, UserValidator>();
            services.AddScoped<IMailWorker, MailWorker>();
            services.AddSingleton<ICurrentUser, CurrentUserConsole>();
            services.AddScoped<IUrlInvitationService, UrlInvitationService>();
            services.AddSingleton<ICurrentUser, CurrentUserConsole>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITextChannelService, TextChannelService>();
            services.AddScoped<IPersonalChatService, PersonalChatService>();
            DAL.DependencyRegistrar.ConfigureServices(services);
        }
    }
}
