namespace Truss;

public static class GuidExtensions
{
    public static string Take(this Guid guid, int count)
    {
        var guidString = guid.ToString();
        var chars = new char[count];

        for (int i = 0; i < count; i++)
        {
            chars[i] = guidString[i];
        }

        return string.Join("", chars);
    }
    
}