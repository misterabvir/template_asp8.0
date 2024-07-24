namespace Shared.Domain;

public static class AppPermissions
{
        public const string AdministratorRole = "Administrator";
        public const string UserRole = "User";

        public static IEnumerable<string> Roles => [AdministratorRole, UserRole];

        public const string AdministratorPolicy = "AdministratorPolicy";
        public const string UserPolicy = "UserPolicy";
}