using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppCustom.Utils
{
  public static class DirectoryUtils
  {
    private static bool IsValidDirectoriesOrFiles(FileSystemInfo fsi)
    {
      try
      {
        
        bool isHidden = (fsi.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
      
        bool isSystem = (fsi.Attributes & FileAttributes.System) == FileAttributes.System;

        return !(isHidden || isSystem);
      }
      catch (UnauthorizedAccessException)
      {
        return false;
      }
    }

    public static bool IsDirectoryOrFileExists(string path)
    {
      IEnumerable<DirectoryInfo> directories = GetDirectories(path);
      if (directories.Any()) return true;

      IEnumerable<FileInfo> files = GetFiles(path);
      if (files.Any()) return true;

      return false;
    }

    public static IEnumerable<DirectoryInfo> GetDirectories(string path)
    {
      DirectoryInfo di = new DirectoryInfo(path);
      if (di.Exists)
      {
        return di.GetDirectories("*", SearchOption.TopDirectoryOnly).Where(IsValidDirectoriesOrFiles);
      }
      else
      {
        return Enumerable.Empty<DirectoryInfo>();
      }
    }

    public static IEnumerable<FileInfo> GetFiles(string path)
    {
      DirectoryInfo di = new DirectoryInfo(path);
      if (di.Exists)
      {
        return di.GetFiles("*", SearchOption.TopDirectoryOnly).Where(IsValidDirectoriesOrFiles);
      }
      else
      {
        return Enumerable.Empty<FileInfo>();
      }
    }
  }
}
