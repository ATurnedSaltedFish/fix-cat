using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerNpro.toolkit
{
  public  class IOFileHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <param name="isAppend">default is write </param>
        /// <returns></returns>
        public bool SaveJsonFile(string filePath,string fileName,string data, bool isAppend=false )
        {
            bool flag;
            try
            {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var fileAllPath = filePath + fileName;
                if (isAppend==true)
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
            catch 
            {
            }
            return false;
        }

        /// <summary>
        /// write file to text 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool WriteTextFile(string filePath,string data)
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
            string str = File.ReadAllText(path,Encoding.Default);
            if (str.Length>length||str==null)
            {
                return "context to long or Error";
            }
            return str;
        }
    }
}
