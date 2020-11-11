namespace BIMPlatform.Application.Contracts.DocumentDataInfo.Util
{
    public class FileUploader
    {
        //    public static DocumentFileDataInfo GetUploadedFileInfo(IBIMResourceManager resourceManager, int projectID, int userID,DocumentUploadParams uploadParams)
        //    {
        //        if(uploadParams.Files == null || uploadParams.Files.Count == 0)
        //        {
        //            // 未上传文件
        //            return null;
        //        }

        //        DocumentFileDataInfo fileInfo = null;
        //        string batchUploadID = uploadParams.BatchUploadID;
        //        string guid = uploadParams.Guid;
        //        string fileTempRelativeFolder = guid;
        //        if(!string.IsNullOrEmpty(batchUploadID))
        //        {
        //            fileTempRelativeFolder = Path.Combine(batchUploadID, guid);
        //        }
        //        var file = uploadParams.Files[0];

        //        #region Todo
        //        //if (file.ContentLength != Convert.ToInt32(request.Form["size"]))
        //        //{
        //        //    throw new Exception(resourceManager.GetString("Document_MissPackageError"));
        //        //}
        //        #endregion

        //        string fileTempFullFolder = Path.Combine(CommonDefine.DocTempFolderPath, fileTempRelativeFolder);

        //        if (!Directory.Exists(fileTempFullFolder))
        //        {
        //            Directory.CreateDirectory(fileTempFullFolder);
        //        }

        //        // Save party file
        //        string filePartName = uploadParams.FileName + uploadParams.Part;
        //        string filePartFullPath = Path.Combine(fileTempFullFolder, filePartName);
        //        file.(filePartFullPath);
        //        //string md5=  GetFileMD5.GetMD5HashFromFile(file.InputStream);
        //        // if (md5 != GetFileMD5.GetMD5HashFromFile(file.InputStream))
        //        // {
        //        //     return ResultJson.BuildJsonResponse(new { success = false });
        //        // }
        //        if (request.Form["isLast"] == "true")
        //        {
        //            string tempFilePath = Path.Combine(fileTempFullFolder, request.Form["fileName"]);

        //            if (System.IO.File.Exists(tempFilePath))
        //            {
        //                System.IO.File.Delete(tempFilePath);
        //            }

        //            string[] allPartyFiles = null;
        //            DateTime datetime = DateTime.Now;
        //            bool isall = true;
        //            while (isall)
        //            {
        //                allPartyFiles = Directory.GetFiles(fileTempFullFolder, request.Form["fileName"] + "*");
        //                if (allPartyFiles.Count() == Convert.ToInt32(request.Form["allfilesCount"]) || datetime.AddMinutes(1) < DateTime.Now)
        //                {
        //                    isall = false;
        //                }
        //            }

        //            //创建空的文件流
        //            using (FileStream tempFile = new FileStream(tempFilePath, FileMode.CreateNew, FileAccess.ReadWrite))
        //            {
        //                Thread.Sleep(1000);

        //                allPartyFiles = allPartyFiles != null ? allPartyFiles.OrderBy(s => int.Parse(Regex.Match(s, @"\d+$").Value)).ToArray() : new string[] { };

        //                using (BinaryWriter bw = new BinaryWriter(tempFile))
        //                {
        //                    for (int i = 0; i < allPartyFiles.Length; i++)
        //                    {
        //                        using (BinaryReader reader = new BinaryReader(File.OpenRead(allPartyFiles[i])))
        //                        {
        //                            byte[] data = new byte[4194304]; //流读取,缓存空间
        //                            int readLen = 0; //每次实际读取的字节大小
        //                            readLen = reader.Read(data, 0, data.Length);
        //                            bw.Write(data, 0, readLen);
        //                        }
        //                    }
        //                }
        //            }

        //            List<int> ids = new List<int>();
        //            if (request.Form["notificationUserId"] != null)
        //            {
        //                ids = new List<int>(request.Form["notificationUserId"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select<string, int>(q => Convert.ToInt32(q)));
        //            }

        //            fileInfo = new DocumentFileDataInfo()
        //            {
        //                BatchUploadID= batchUploadID,
        //                UploadID = guid,
        //                Name = request.Form["fileName"],
        //                Suffix = Path.GetExtension(request.Form["fileName"]),
        //                UploadTempPath = tempFilePath,
        //                CreationUserID = userID,
        //                RequireTransform = true,
        //                SupportDocNumber = false,
        //                NotificationUserId = ids,
        //                Tags = request.Form["tags"] == null ? null : request.Form["tags"].ToString(),
        //                Status = request.Form["isVerified"] == null ? null : (request.Form["isVerified"] == "true" ? "OnVerified" : null),
        //                ClientRelativePath = request.Form["ClientRelativePath"] == null ? string.Empty : request.Form["ClientRelativePath"].ToString(),
        //                ProjectID = projectID
        //            };
        //        }
        //        return fileInfo;
        //    }

        //    public static IEnumerable<DocumentFileDataInfo> GetBacthUploadedTempFileInfos(IBIMResourceManager resourceManager, int projectID, int userID, string batchUploadID,bool getFileStream, bool requireTransform)
        //    {
        //        // lower file name to file info, one batch, one same file
        //        Dictionary<string, DocumentFileDataInfo> fileLowerNameToFiles = new Dictionary<string, DocumentFileDataInfo>();

        //        string tempRelativeParentFolder = Path.Combine(CommonDefine.DocTempFolderName, batchUploadID);
        //        string tempFullParentFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, tempRelativeParentFolder);

        //        if (!Directory.Exists(tempFullParentFolder))
        //        {
        //            return fileLowerNameToFiles.Values;
        //        }

        //        DirectoryInfo parentDirInfo = new DirectoryInfo(tempFullParentFolder);
        //        // Each dir contains a temp uploaded file and dir is an original file upload ID

        //        Regex searchPartyPattern = new Regex(@"part\d+$", RegexOptions.IgnoreCase);
        //        foreach (DirectoryInfo subDir in parentDirInfo.GetDirectories())
        //        {
        //            string guid = subDir.Name;

        //            // Search file which is last created but not party
        //            // Current, only one file in the directory
        //            FileInfo uploadedFile = subDir.GetFiles("*", SearchOption.TopDirectoryOnly).Where(f => !searchPartyPattern.IsMatch(f.Name))
        //                            .OrderBy(f => f.CreationTime).LastOrDefault();

        //            if (uploadedFile != null)
        //            {
        //                string directParentFolderName = uploadedFile.Directory.Name;
        //                DocumentFileDataInfo fileInfo = new DocumentFileDataInfo()
        //                {
        //                    BatchUploadID = batchUploadID,
        //                    UploadID = guid,
        //                    UploadedTime = uploadedFile.CreationTime,
        //                    UploadTempRelativePath = "/"+Path.Combine(tempRelativeParentFolder, Path.Combine(directParentFolderName, uploadedFile.Name)).Replace("\\","/"),
        //                    Name = uploadedFile.Name,
        //                    Suffix = Path.GetExtension(uploadedFile.Name),
        //                    UploadTempPath = uploadedFile.FullName,
        //                    CreationUserID = userID,
        //                    RequireTransform = requireTransform,
        //                    SupportDocNumber = false,
        //                    ProjectID = projectID,
        //                    SizeDisplay = GetSizeDisplayAs(resourceManager, uploadedFile.Length),
        //                    Stream = !getFileStream ? null : File.OpenRead(uploadedFile.FullName)
        //                };

        //                string lowerFileName = uploadedFile.Name.ToLower();
        //                if(fileLowerNameToFiles.ContainsKey(lowerFileName))
        //                {
        //                    DocumentFileDataInfo addedFileInfo = fileLowerNameToFiles[lowerFileName];
        //                    if (fileInfo.UploadedTime.CompareTo(addedFileInfo.UploadedTime) > 0)
        //                    {
        //                        fileLowerNameToFiles[lowerFileName] = fileInfo;
        //                    }
        //                    else
        //                    {
        //                        // Ignore previous uploaded same file
        //                        continue;
        //                    }
        //                }
        //                else
        //                {
        //                    fileLowerNameToFiles[lowerFileName] = fileInfo;
        //                }
        //            }
        //        }

        //        return fileLowerNameToFiles.Values;
        //    }

        //    public static void RemoveTempUploadedFileOfBatch(string batchUploadID, string fileName)
        //    {
        //        // lower file name to file info, one batch, one same file
        //        Dictionary<string, DocumentFileDataInfo> fileLowerNameToFiles = new Dictionary<string, DocumentFileDataInfo>();

        //        string tempFullParentFolder = Path.Combine(CommonDefine.DocTempFolderPath, batchUploadID);

        //        if (!Directory.Exists(tempFullParentFolder))
        //        {
        //            return;
        //        }

        //        DirectoryInfo parentDirInfo = new DirectoryInfo(tempFullParentFolder);
        //        FileInfo[] files = parentDirInfo.GetFiles(fileName, SearchOption.AllDirectories);
        //        // Remove the file's directory totally
        //        foreach(FileInfo file in files)
        //        {
        //            try
        //            {
        //                if (file.Directory.Exists)
        //                {
        //                    file.Directory.Delete(true);
        //                }
        //            }
        //            catch(Exception ex)
        //            {
        //                // ServerLogger.Error(string.Format("Failed to delete temp file {0}", file.Name), ex);
        //            }
        //        }
        //    }

        //    public static string GetSizeDisplayAs(IBIMResourceManager ResourceManager, long fileLength)
        //    {
        //        double size = 0;
        //        if (fileLength < 1024) // Byte
        //        {
        //           return GetSizeDisplay(size, ResourceManager.GetString("String_FileSizeByte"));
        //        }
        //        else
        //        {
        //            size = (float)(fileLength) / 1024;
        //            if (size < 1024) // KB
        //            {
        //                return GetSizeDisplay(size, ResourceManager.GetString("String_FileSizeKB"));
        //            }
        //            else
        //            {
        //                size = size / 1024;
        //                if (size < 1024) // MB
        //                {
        //                    return GetSizeDisplay(size, ResourceManager.GetString("String_FileSizeMB"));
        //                }
        //                else
        //                {
        //                    size = size / 1024;
        //                    if (size < 1024) // GB
        //                    {
        //                        return GetSizeDisplay(size, ResourceManager.GetString("String_FileSizeGB"));
        //                    }
        //                    else
        //                    {
        //                        size = size / 1024; //TB
        //                        return GetSizeDisplay(size, ResourceManager.GetString("String_FileSizeTB"));
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    private static string GetSizeDisplay(double size, string format)
        //    {
        //        int integer = (int)(Math.Floor(size));
        //        if (integer == size)
        //        {
        //            return string.Format(format, integer);
        //        }
        //        else
        //        {
        //            return string.Format(format, Math.Round(size, 2).ToString());
        //        }
        //    }
    }
}
