using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FBBasicFacebookFeature
{
    public sealed class ExceptionsXml
    {
        private static ExceptionsXml s_Instance = null;
        private static object s_LockObj = new object();
        private string m_Message;

        private ExceptionsXml()
        {
        }

        public static ExceptionsXml GetInstance
        {
           get 
            {
                lock (s_LockObj)
                {
                    if (s_Instance == null)
                    {
                        s_Instance = new ExceptionsXml();
                    }

                    return s_Instance;
                }
            }
        }

        private void saveToXml(ExceptionsXml i_Exception)
        {
            using (FileStream file = new FileStream("Exceptions.xml", FileMode.Append))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExceptionsXml));
                serializer.Serialize(file, i_Exception);
            }
        }

        public void ExceptionOccurred(Exception i_Exception)
        {
            s_Instance.m_Message = i_Exception.Message;
            s_Instance.saveToXml(s_Instance);   
        }
    }
}