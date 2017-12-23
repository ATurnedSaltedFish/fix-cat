using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;

namespace CrawlerNpro.toolkit
{
    public class IOFileHelper
    {
        private class FileItem
        {
            private string[] strfileList;

            public string[] StrfileList
            {
                get
                {
                    return strfileList;
                }

                set
                {
                    strfileList = value;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <param name="isAppend">default is write </param>
        /// <returns></returns>
        public bool SaveJsonFile(string filePath, string fileName, string data, bool isAppend = false)
        {
            bool flag;
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var fileAllPath = filePath + fileName;
                if (isAppend == true)
                {
                    flag = WriteAppendTextFile(fileAllPath, data);
                }
                else
                {
                    flag = WriteTextFile(fileAllPath, data);
                }
                //  File.Move(fileAllPath,filePath);
                return flag;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        /// <summary>
        ///  read json file 
        /// </summary>
        /// <param name="fileParh"></param>
        /// <returns></returns>
        public string ReadJsonFileToString(string fileParh)
        {
          Uri uri= new Uri("pack://application:,,," + fileParh, UriKind.Absolute);
           StreamResourceInfo info = System.Windows.Application.GetContentStream(uri);
            if (info==null)
            {
                return null;
            }
            StreamReader stream =new StreamReader(info.Stream);
           var strJson=  stream.ReadToEnd();
            return strJson;
        }
  
        /// <summary>
        /// jsonfile to bytes
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public byte[] ReadJsonFileToByte(string filePath)
        {
            return null;
        }

        /// <summary>
        ///    traversing all file names 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string[] GetFileNames(string filePath)
        {
            List<string[]> ss = new List<string[]>();
            //check filePath  exists 
            if (Directory.Exists(filePath) == false)
            {
                Debug.WriteLine("--------filepPath is null--------");
                return null;
            }
            var strlist = Directory.GetDirectories(filePath);
            if (strlist == null)
            {
                return null;
            }
            return strlist;
        }

        /// <summary>
        /// write file to text 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool WriteTextFile(string filePath, string data)
        {
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(data);
                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteAppendTextFile(string filePath, string data)
        {
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(data);
                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// read txt File
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadTxtFile(string path)
        {
            int length = 1000;
            string str = File.ReadAllText(path, Encoding.Default);
            if (str.Length > length || str == null)
            {
                return "context to long or Error";
            }
            return str;
        }
    }
}
