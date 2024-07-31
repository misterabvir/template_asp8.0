namespace Domain.Abstractions;

/// <summary>
/// Authorization constants
/// </summary>
public static class AuthorizationConstants
{
	/// <summary>
	/// Roles
	/// </summary>
	public static class Roles
	{
		public const string Administrator = "Administrator";
		public const string User = "User";
	}

	/// <summary>
	/// Policies
	/// </summary>
	public static class Policies
	{
		public const string Administrator = "AdministratorPolicy";
		public const string User = "User";
	}

	/// <summary>
	/// All roles
	/// </summary>
	public static IEnumerable<string> AllRoles => [Roles.Administrator, Roles.User];

	/// <summary>
	/// All policies
	/// </summary>
	public static IEnumerable<string> AllPolicies => [Policies.Administrator, Policies.User];
}
