namespace GradeHoraria.Models
{
    public class UserRoles
    {
        public const string AdminMaster = "AdminMaster";
        public const string Admin = "Admin";
        public const string Coordenador = "Coordenador";
        public const string Professor = "Professor";
        public const string Usuario = "Usuário";
    }
}

/*private async Task CreateRoles()
    {
        // Check if the role already exists
        if (!await _roleManager.RoleExistsAsync(UserRoles.AdminMaster))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.AdminMaster));
        }
        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        }
        if (!await _roleManager.RoleExistsAsync(UserRoles.Coordenador))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Coordenador));
        }
        if (!await _roleManager.RoleExistsAsync(UserRoles.Professor))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Professor));
        }
        if (!await _roleManager.RoleExistsAsync(UserRoles.Usuario))
        {
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Usuario));
        }
    }*/