using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.YouTube;

namespace FBBasicFacebookFeature
{
    public interface IYoutube
    {
        List<Video> FindVideos(string i_VideoToSearch, List<Video> i_VideoList);

        List<Video> NewVideos { get; }

        Video Video { get; set; }

        List<Video> VideoFeeds { get; }

        string GetSelectedVideo();

        bool AddToMyFavorites(Video i_Video);

        void RemoveVideoFromFavorites(int i_VideoToRemove);

        List<Video> MyFavoritesVideos { get; }     
    }
}
