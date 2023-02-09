namespace Rpa.Contracts
{
    public interface IFileHandler
    {
        void InitFolder(string path, bool isRemoveContent = false);
        void Move(string sourceFilePath, string targetFilePath, bool isOverwrite = true);
        void Copy(string sourceFilePath, string targetFilePath, bool isOverwrite = true);
        bool IsPathFullyQualified(string path);
    }
}