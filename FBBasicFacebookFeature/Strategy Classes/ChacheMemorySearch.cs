using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FBBasicFacebookFeature
{
   public class ChacheMemorySearch : ISearch
    {
        private const string c_FileName = "ChacheMemoryOfMovies.xml";
        private ExceptionsXml m_XmlException = ExceptionsXml.GetInstance;

        public List<IMDb> List = new List<IMDb>();

        public bool Search(string i_NameToSearch, out IMDb io_TheMovie)
        {
            bool movieExistAlready = false;
            ReadFromFile();
            io_TheMovie = getImdb(i_NameToSearch);

            if(io_TheMovie != null)
            {
                movieExistAlready = true;
            }

            return movieExistAlready;
        }

        private IMDb getImdb(string i_NameToSearch)
        {
            IMDb theMovie = new IMDb();
            IMDb theMovie2 = null;
            try
            {
                string imdbUrl = theMovie.getIMDbUrl(System.Uri.EscapeUriString(i_NameToSearch));
                int indexOfId = imdbUrl.LastIndexOf("tt");
                string IdNameOfMovie = imdbUrl.Substring(indexOfId, imdbUrl.Length - indexOfId - 1);

                foreach (IMDb currentImdb in List)
                {
                    if (currentImdb.Id == IdNameOfMovie)
                    {
                        theMovie2 = currentImdb;
                        break;
                    }
                }
            }
            catch(Exception exceptionFindingTheMovie)
            {
                m_XmlException.ExceptionOccurred(exceptionFindingTheMovie);
                theMovie = null;
            }

            return theMovie2;
        }

        private void ReadFromFile()
        {
            SaveAndReadXmlFile.ReadFromXml(c_FileName, typeof(List<IMDb>), ref List);
        }
    }
}
