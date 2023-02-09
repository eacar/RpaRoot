using Rpa.Contracts;
using System;
using System.IO;

namespace Rpa.Handlers
{
    public sealed class FileHandler : IFileHandler
    {
        #region Methods - Public - IFileHelper

        public void InitFolder(string path, bool isRemoveContent = false)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else if (isRemoveContent)
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }

        public void Move(string sourceFilePath, string targetFilePath, bool isOverwrite = true)
        {
            InitFolder(Path.GetDirectoryName(targetFilePath));

            if (isOverwrite)
            {
                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }
            }

            File.Move(sourceFilePath, targetFilePath);
        }

        public void Copy(string sourceFilePath, string targetFilePath, bool isOverwrite = true)
        {
            InitFolder(Path.GetDirectoryName(targetFilePath));

            File.Copy(sourceFilePath, targetFilePath, isOverwrite);
        }
        public bool IsPathFullyQualified(string path)
        {
            return Path.IsPathRooted(path)
                   && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }

        #endregion
    }
}