using System.IO;
using System.Threading.Tasks;
using BLL;
using Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PL.Console.Interfaces;
using PL.Console.RoomsControl;

namespace PL.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetService<App>()?.StartApp();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + @"..\..\..\..\ ")
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddScoped<App>();
            services.AddScoped<IRegistration, Registration.Registration>();
            services.AddScoped<IAuthorization, Authorization.Authorization>();
            services.AddScoped<IResetPasswordControl, ResetPassword.ResetPasswordControl>();
            services.AddScoped<IInvitation, Invitation>();
            services.AddScoped<IRoomsControl, RoomsControl.RoomsControl>();
            services.AddScoped<IRoleControl, RoleControl>();
            services.AddScoped<IUserControl, UserControl>();
            services.AddScoped<IPersonalChatControl, PersonalChatControl>();
            services.AddScoped<ITextChannelControl, TextChannelControl>();
            DependencyRegistrar.ConfigureServices(services);
        }
    }
}
