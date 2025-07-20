namespace OrderManagement.Api.Utils;

public static class JwtInfo
{
    public const string SECRET_KEY = "sua-chave-secreta-muito-forte-aqui";
    public const string ISSUER = "OrderManagementIssuer";
    public const string AUDIENCE = "OrderManagementAudience";
    public const string EXPIRATION = "exp";
    public const string USER_ID = "userId";
}