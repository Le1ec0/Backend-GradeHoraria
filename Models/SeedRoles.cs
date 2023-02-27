using Microsoft.AspNetCore.Identity;

namespace GradeHoraria.Models
{
    public class SeedRoles : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedRoles(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await CreateRoleAsync(roleManager, UserRoles.AdminMaster);
                await CreateRoleAsync(roleManager, UserRoles.Admin);
                await CreateRoleAsync(roleManager, UserRoles.Coordenador);
                await CreateRoleAsync(roleManager, UserRoles.Professor);
                await CreateRoleAsync(roleManager, UserRoles.Usuario);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
