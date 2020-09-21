using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMPlatform.ToolKits.Helper
{
    public class FileUtil
    {
        public static string File2Bytes(string path)
        {
            string rPath = "";

            if (!System.IO.File.Exists(path))
            {
                return "";
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            rPath = Convert.ToBase64String(buff);
            return rPath;
        }

        /// <summary>   
        /// 将 Stream 写入文件   
        /// </summary>   
        public static void StreamToFile(Stream stream, string filePath)
        {
            // 把 Stream 转换成 byte[]   
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始   
            stream.Seek(0, SeekOrigin.Begin);

            // 把 byte[] 写入文件   
            FileStream fs = new FileStream(filePath, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        public static void DeleteFiles(IList<string> filePaths)
        {
            if (filePaths == null)
                return;

            foreach (string path in filePaths)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }  
                catch (Exception ex)
                {
                    //Logger.Log(Level.Error, $"删除文件夹失败，路径：{path}", ex);
                }
            }
        }
    }
}
