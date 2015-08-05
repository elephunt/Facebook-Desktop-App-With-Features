using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.YouTube;

namespace FBBasicFacebookFeature
{
    public class Youtube : IYoutube
    {
        public YouTubeRequest Request { get; set; }

        public List<Video> VideoFeeds { get; set; }

        public List<Video> NewVideos { get; set; }

        public ExceptionsXml m_XmlExp = ExceptionsXml.GetInstance;

        public Video Video { get; set; }

        public List<Video> MyFavoritesVideos { get; set; }
  
        public Youtube()
        {
            YouTubeRequestSettings settings = new YouTubeRequestSettings("DemoFacebookFeature", "AI39si4cTAJSx5HF1qHrhfD_ws7kUEnk0Tr02WcFPiMf96nTxczLMT8a_lJqGhlbKRsY0YZE5BYhO-gu2y7rXsQesC3Jf2-jGA");
            Request = new YouTubeRequest(settings);
            VideoFeeds = new List<Video>();
            NewVideos = new List<Video>();
        }

        public List<Video> FindVideos(string i_VideoToSearch, List<Video> i_VideoList)
        {
            try
            {
                i_VideoList.Clear();
                YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);
                
                if (query != null)
                {
                    query.OrderBy = "viewCount";

                    if (i_VideoToSearch == string.Empty)
                    {
                        query.Time = YouTubeQuery.UploadTime.ThisWeek;
                    }
                    else
                    {
                        query.Query = i_VideoToSearch;
                    }

                    query.SafeSearch = YouTubeQuery.SafeSearchValues.None;
                    Feed<Video> videoFeeds = Request.Get<Video>(query);

                    if (videoFeeds != null && i_VideoList != null)
                    {
                        getVideos(videoFeeds, i_VideoList);
                    }
                    else
                    {
                        throw new ArgumentNullException("videoFeeds or i_VideoList is null");
                    }
                }
                else
                {
                    throw new ArgumentNullException("query is null");
                }
            }
            catch(ArgumentNullException argumentNullException)
            {
                m_XmlExp.ExceptionOccurred(argumentNullException);
            }

            return i_VideoList;
        }

        private void getVideos(Feed<Video> i_VideoFeeds, List<Video> i_VideoList)
        {
            foreach (Video currentVideo in i_VideoFeeds.Entries)
            {
                if (currentVideo.Status == null && currentVideo.YouTubeEntry.State == null && currentVideo.Media.Restriction == null)
                {
                    i_VideoList.Add(currentVideo);
                }
            }
        }

        public string GetSelectedVideo()
        {
            return "http://www.youtube.com/v/" + Video.VideoId + "?autohide=1&version=3";
        }

        public bool AddToMyFavorites(Video i_Video)
        {
            bool exsit = false;

            if (i_Video != null)
            {
                if (MyFavoritesVideos.Count > 0)
                {
                    foreach (Video currentVideo in MyFavoritesVideos)
                    {
                        if (currentVideo.VideoId == i_Video.VideoId)
                        {
                            exsit = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("You must choose video!");
            }

            if (!exsit)
            {
                MyFavoritesVideos.Add(i_Video);
            }

            return exsit;
        }

        public void RemoveVideoFromFavorites(int i_VideoToRemove)
        {
            try
            {
                MyFavoritesVideos.RemoveAt(i_VideoToRemove);
            }
            catch (ArgumentOutOfRangeException outOfRangeException)
            {
                m_XmlExp.ExceptionOccurred(outOfRangeException);
            }
        }
    }
}
