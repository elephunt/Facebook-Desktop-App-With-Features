using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Google.YouTube;
using Google.GData.Client;

namespace FBBasicFacebookFeature
{
    public class YoutubeProxy : IYoutube
    {
       private Youtube m_YoutubeProxy;

       public List<Video> MyFavoritesVideos
        { 
            get
            {
                return m_YoutubeProxy.MyFavoritesVideos;
            }
        }

       public Video Video
        {
            get
            {
                return m_YoutubeProxy.Video;
            }

            set { m_YoutubeProxy.Video = value; }
        }

       public List<Video> NewVideos 
        {
            get
            {
                return m_YoutubeProxy.NewVideos;
            }
        }

       public List<Video> VideoFeeds 
        {
            get
            {
                return m_YoutubeProxy.VideoFeeds;
            }
        }

       public YoutubeProxy()
        {
            m_YoutubeProxy = new Youtube();
            getFavoritesFromFile();
        }

       public List<Video> FindVideos(string i_VideoToSearch, List<Video> i_VideoList)
        {
            return m_YoutubeProxy.FindVideos(i_VideoToSearch, i_VideoList);
        }

        private void getFavoritesFromFile()
        {
            m_YoutubeProxy.MyFavoritesVideos = new List<Video>();
            StreamReader fileToRead = null;

            try
            {
                string readLine = string.Empty;
                fileToRead = new StreamReader("MyFavorites.txt");
                while ((readLine = fileToRead.ReadLine()) != null)
                {
                    Uri uri = new Uri("http://gdata.youtube.com/feeds/api/videos/" + readLine +
                        "?AI39si4cTAJSx5HF1qHrhfD_ws7kUEnk0Tr02WcFPiMf96nTxczLMT8a_lJqGhlbKRsY0YZE5BYhO-gu2y7rXsQesC3Jf2-jGA");
                    Feed<Google.YouTube.Video> videoFeeds = m_YoutubeProxy.Request.Get<Google.YouTube.Video>(uri);

                    if (videoFeeds.Entries != null)
                    {
                        foreach (Video currentVideo in videoFeeds.Entries)
                        {
                            m_YoutubeProxy.MyFavoritesVideos.Add(currentVideo);
                        }
                    }
                }
            }
            catch (Exception exceptionReadFromFavorites)
            {
                m_YoutubeProxy.m_XmlExp.ExceptionOccurred(exceptionReadFromFavorites);
            }
            finally
            {
                if (fileToRead != null)
                {
                    fileToRead.Close();
                }
            }
        }

        public string GetSelectedVideo()
        {
           return m_YoutubeProxy.GetSelectedVideo();
        }

        private void updateMyFavoritesFile()
        {
            try
            {
                using (StreamWriter file = new StreamWriter("MyFavorites.txt", false))
                {
                    foreach (Video currentVideo in m_YoutubeProxy.MyFavoritesVideos)
                    {
                        file.WriteLine(currentVideo.VideoId);
                    }
                }
            }
            catch (Exception streamException)
            {
                m_YoutubeProxy.m_XmlExp.ExceptionOccurred(streamException);
            }
        }

        public void RemoveVideoFromFavorites(int i_VideoToRemove)
        {
            m_YoutubeProxy.RemoveVideoFromFavorites(i_VideoToRemove);
            updateMyFavoritesFile();
        }

        public bool AddToMyFavorites(Video i_Video)
        {
            bool exsitInFavoriteList = m_YoutubeProxy.AddToMyFavorites(i_Video); 

            if (!exsitInFavoriteList)
            {
                updateMyFavoritesFile();
            }

            return exsitInFavoriteList;
        }
    }
}
