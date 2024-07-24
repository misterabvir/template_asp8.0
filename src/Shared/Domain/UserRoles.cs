namespace Shared.Domain;

public static class UserRoles
{
        public const string AdministratorRole = "Administrator";
        public const string UserRole = "User";

        public static List<string> AppRoles = [AdministratorRole, UserRole];
}