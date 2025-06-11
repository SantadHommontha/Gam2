using UnityEngine;

public static class GenerateCode
{
    private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public static string GenerateRandomCode(int length = 4)
    {
        string result = "";

        for (int i = 0; i < length; i++)
        {
            int index = Random.Range(0, chars.Length);
            result += chars[index];
        }

        return result;
    }
}
