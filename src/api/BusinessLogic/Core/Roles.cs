namespace BusinessLogic.Core
{
    public static class Roles
    {
        public const string Admin = "admin";

        public const string Manager = "manager";

        public const string Foreman = "foreman";

        public static string[] AllowedRoles => new [] { Admin, Manager, Foreman };
    }
}
