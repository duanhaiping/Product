using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace BIMPlatform.Application.Contracts.Common
{
    public class CommonDefine
    {
        public const string WebRootPath = "WebRootPath";
        public const string WebCachePath = "WebCachePath";
        public const string WebFilePath = "WebFilePath";
        public const string WebImagePath = "WebImagePath";
        public const string FileRootPath = "FileRootPath";
        public const string ImageRootPath = "ImageRootPath";
        public const string CachePath = "CachePath";
        public const string isBIMViewerApiAddress = "isBIM.ViewerApiAddress";
        public const string isBIMViewerApiToken = "isBIM.ViewerApiToken";
        public const string DocBluePrint = "doc.BluePrint";
        public const string DocQInspection = "doc.QInspection";
        public const string DocMaterialVerify = "doc.MaterialVerify";
        public const string DocSpecialStaff = "doc.SpecialStaff";
        public const string DocLargeMachine = "doc.LargeMachine";
        public const string DocConstruction = "doc.Construction";
        public const string DocProjectImg = "doc.ProjectImg";
        public const string DocDailyImg = "doc.DailyImg";
        public const string DocConstructionProject = "doc.Construction.Project";
        public const string Init = "Init";
        public const string AppDataPath = "App_Data";
        public const string StartPage = "StartPage";
        public const string AdminStartPage = "AdminStartPage";
        public const string DefaultPage = "DefaultPage";
        public const string LoginExpirationIntervalMinutes = "LoginExpirationIntervalMinutes";
        public const string ProjectOverviewPage = "ProjectOverviewPage";
        public const string LogPath = "Log";
        public const string UserFree ="doc.UserFree";
        public const string IssueComment = "doc.IssueComment";
        public const string Qualification = "doc.Qualification";
        public const string SupervisorLog = "doc.supervisorLog";
        public static string GetWebPath()
        {
            return ConfigurationManager.AppSettings[WebRootPath];
        }

        public static string GetWebCachePath()
        {
            return ConfigurationManager.AppSettings[WebCachePath];
        }

        public static string GetWebFilePath()
        {
            return ConfigurationManager.AppSettings[WebFilePath];
        }

        public static string GetWebImagePath()
        {
            return ConfigurationManager.AppSettings[WebImagePath];
        }

        public static string GetFilePath()
        {
            return ConfigurationManager.AppSettings[FileRootPath];
        }
        public static string GetDocQualification()
        {
            return ConfigurationManager.AppSettings[Qualification];
        }
        public static string GetImagePath()
        {
            return ConfigurationManager.AppSettings[ImageRootPath];
        }
        public static string GetDocUserFreePath()
        {
            return ConfigurationManager.AppSettings[UserFree];
        }
        public static string GetDocIssueCommentPath()
        {
            return ConfigurationManager.AppSettings[IssueComment];
        }
        public static string GetSupervisorLogPath()
        {
            return ConfigurationManager.AppSettings[SupervisorLog];
        }
        public static string GetCachePath()
        {
            return ConfigurationManager.AppSettings[CachePath];
        }

        public static string GetisBIMViewerApiAddress()
        {
            return ConfigurationManager.AppSettings[isBIMViewerApiAddress];
        }

        public static string GetisBIMViewerApiToken()
        {
            return ConfigurationManager.AppSettings[isBIMViewerApiToken];
        }

        public static string GetDocBluePrint()
        {
            return ConfigurationManager.AppSettings[DocBluePrint];
        }

        public static string GetDocQInspection()
        {
            return ConfigurationManager.AppSettings[DocQInspection];
        }
        public static string GetDocProjectImg()
        {
            return ConfigurationManager.AppSettings[DocProjectImg];
        }
        public static string GetDocDailyImg()
        {
            return ConfigurationManager.AppSettings[DocDailyImg];
        }
        public static string GetDocConstruction()
        {
            return ConfigurationManager.AppSettings[DocConstruction];
        }
        public static string GetDocMaterialVerify()
        {
            return ConfigurationManager.AppSettings[DocMaterialVerify];
        }
        public static string GetDocSpecialStaff()
        {
            return ConfigurationManager.AppSettings[DocSpecialStaff];
        }

        public static string GetLargeMachine()
        {
            return ConfigurationManager.AppSettings[DocLargeMachine];
        }
        public static string GetDocConstructionProject()
        {
            return ConfigurationManager.AppSettings[DocConstructionProject];
        }

        public static string GetAppDataPath()
        {
            string folder = System.AppDomain.CurrentDomain.BaseDirectory + AppDataPath;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        public static string GetStartPage()
        {
            return ConfigurationManager.AppSettings[StartPage];
        }
        public static string GetAdminStartPage()
        {
            return ConfigurationManager.AppSettings[AdminStartPage];
        }
        public static string GetProjectOverviewPage()
        {
            return ConfigurationManager.AppSettings[ProjectOverviewPage];
        }
        public static string GetDefaultPage()
        {
            return ConfigurationManager.AppSettings[DefaultPage];
        }
        public static int GetLoginExpirationIntervalMinutes()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings[LoginExpirationIntervalMinutes]);
        }
        public static string GetLogPath()
        {
            string folder = System.AppDomain.CurrentDomain.BaseDirectory + LogPath;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        public static string DocDataFolderPath
        {
            get
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DocData");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string DocRootViewDirName
        {
            get
            {
                return ConfigurationManager.AppSettings["DocRootViewDirName"];
            }
        }

        public static string DocViewNewSubFolderRelativePath
        {
            get
            {
                //string path = Path.Combine(DocRootViewDirName, Guid.NewGuid().ToString());
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}
                //return path;
                string path = Path.Combine(DocRootViewDirName, Guid.NewGuid().ToString());
                return path;
            }
        }

        public static string DocViewRootFolderFullPath
        {
            get
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DocRootViewDirName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string DocTempFolderName
        {
            get
            {
                return "DocTempData";
            }
        }

        public static string DocTempFolderPath
        {
            get
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DocTempFolderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string DocTempNewSubFolderFullPath
        {
            get
            {
                string path = Path.Combine(DocTempFolderPath, Guid.NewGuid().ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        private static string LogLevel = string.Empty;
        private static FileSystemWatcher FileWatcher = null;

        public static string GetLogLevel()
        {
            if (string.IsNullOrEmpty(LogLevel))
            {
                LogLevel = "Error";

                string fileName = "SystemConfig.xml";
                string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "SystemConfiguration");
                string filePath = Path.Combine(folderPath, fileName);
                if (File.Exists(filePath))
                {
                    if (FileWatcher != null)
                    {
                        FileWatcher.Changed -= Watcher_Changed;
                        FileWatcher.Deleted -= Watcher_Deleted;
                        FileWatcher.Dispose();
                        FileWatcher = null;
                    }

                    FileWatcher = new FileSystemWatcher(folderPath);
                    FileWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size;
                    FileWatcher.Filter = fileName;
                    FileWatcher.Changed += Watcher_Changed;
                    FileWatcher.Deleted += Watcher_Deleted;
                    FileWatcher.EnableRaisingEvents = true;
                    LoadLogLevel(filePath);
                }
            }
            return LogLevel;
        }

        private static void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            LogLevel = string.Empty;
            FileWatcher.Changed -= Watcher_Changed;
            FileWatcher.Deleted -= Watcher_Deleted;
            FileWatcher.Dispose();
            FileWatcher = null;
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string fileName = "SystemConfig.xml";
            string folderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "SystemConfiguration");
            string filePath = Path.Combine(folderPath, fileName);
            LoadLogLevel(filePath);
        }

        private static void LoadLogLevel(string filePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlElement configNode = xmlDoc.SelectSingleNode("//LogConfig") as XmlElement;
                LogLevel = configNode.GetAttribute("LogLevel");
            }
            catch
            {

            }
        }

        public static string ImageCacheFolderPath
        {
            get
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageCache");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
    }
}
