using System.Security.Cryptography;
using System.Text;

namespace OrderManagement.Shared.Extensions;

public static class CryptographyExtension
{
    public static string Encrypt(this string value)
    {
        // Converter a String para array de bytes, que é como a biblioteca trabalha.
        byte[] data = MD5.HashData(Encoding.UTF8.GetBytes(value));

        // Cria-se um StringBuilder para recompôr a string.
        var sb = new StringBuilder();

        // Loop para formatar cada byte como uma string em hexadecimal
        for (int i = 0; i < data.Length; i++)
            sb.Append(data[i].ToString("x2"));

        return sb.ToString();
    }
}