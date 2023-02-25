using System;
 
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace wheelDetection
{
    class SaveDataTool//进行序列号和反序列化
    {
        public static void Save2File<T>(T obj,string FileName)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
                fs.Flush();
            }
            catch  (Exception ex)
            {

            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public static T FormBetyFile<T>(string path)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (T)formatter.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
      
    }
}
