namespace AppCore.Authorization;

public enum AppPolicies
{
    AdminOnly,
    ActiveUser,
    AnonymousUser,     
}
	
public static class AppPoliciesExtensions
{
    public static string Name(this AppPolicies policy) {
        return policy.ToString();
    }
}