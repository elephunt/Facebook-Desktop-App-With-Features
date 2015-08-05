using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FBBasicFacebookFeature
{
    public class SearchInImdb : ISearch
    {
        private const string c_FileName = "ChacheMemoryOfMovies.xml";
        private List<IMDb> searchResualt = null;

        private ExceptionsXml m_XmlException = ExceptionsXml.GetInstance;

        public bool Search(string i_NameOfTheMovie, out IMDb io_TheMovie)
        {
            io_TheMovie = null;
            bool movieExist = false;
            IMDb searchMovie = new IMDb(i_NameOfTheMovie);
            io_TheMovie = searchMovie;
            saveToFileCache(searchMovie);

            if(searchMovie != null)
            {
                movieExist = true;
            }

            return movieExist;
        }

        private void saveToFileCache(IMDb i_Movie)
        {
            ReadFromFile();
            if (searchResualt == null)
            {
                searchResualt = new List<IMDb>();
            }

            searchResualt.Add(i_Movie);
            SaveAndReadXmlFile.SaveToXml(c_FileName, searchResualt, FileMode.Create);
        }

        private void ReadFromFile()
        {
            SaveAndReadXmlFile.ReadFromXml(c_FileName, typeof(List<IMDb>), ref searchResualt);
        }
    }
}
