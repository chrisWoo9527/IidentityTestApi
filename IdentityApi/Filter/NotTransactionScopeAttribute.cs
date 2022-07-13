namespace IdentityApi.Filter
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NotTransactionScopeAttribute : Attribute
    {
    }
}
