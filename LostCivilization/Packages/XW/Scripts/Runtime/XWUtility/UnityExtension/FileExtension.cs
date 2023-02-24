using System.IO;

namespace XWUtility
{
    public static class FileExtension
    {
        public static void CheckDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(path);
        }
    }
}