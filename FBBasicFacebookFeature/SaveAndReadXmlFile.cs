using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FBBasicFacebookFeature
{
    public static class SaveAndReadXmlFile
    {
        public static void SaveToXml<T>(string i_XmlFileName, T i_ObjectToSerialize, FileMode i_FileMode)
        {
            FileStream file = null;
            try
            {
                using (file = new FileStream(i_XmlFileName, i_FileMode))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(file, i_ObjectToSerialize);
                }
            }
            catch (Exception writeToXmlFileException)
            {
                ExceptionsXml.GetInstance.ExceptionOccurred(writeToXmlFileException);
            }
        }

        public static void ReadFromXml<T1, T2>(string i_NameOfFile, T1 i_ReflectionObject, ref T2 i_ReturnFileRead)
        {
            StreamReader file = null;
            if (File.Exists(i_NameOfFile))
            {
                try
                {
                    XmlSerializer reader = new XmlSerializer(typeof(T2));
                    file = new StreamReader(i_NameOfFile);
                    i_ReturnFileRead = (T2)reader.Deserialize(file);
                }
                catch (Exception exceptionSerializetion)
                {
                    ExceptionsXml.GetInstance.ExceptionOccurred(exceptionSerializetion);
                }
                finally
                {
                    file.Close();
                }
            }
        }
    }
}
