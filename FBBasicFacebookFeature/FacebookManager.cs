// -----------------------------------------------------------------------
// <copyright file="FacebookManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace FBBasicFacebookFeature
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Facebook;
    using FacebookWrapper.ObjectModel;
    using FacebookWrapper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FacebookManager
    {
        public User LoggedInUser { get; set; }

        private List<User> m_UserFriends;
        private List<Event> m_FacebookEvents;
        private List<Checkin> m_FacebookCheckin;
        private List<Post> m_Posts;

        public List<Post> News
        {
            get 
            {
                fetchNewsFeed();
                return m_Posts;
            }
        }

        public List<User> UserFriends
        {
            get
            {
                fetchFriends();
                return m_UserFriends;
            }
        }

        public List<Event> FacebookEvents
        {
            get
            {
                fetchEvents();
                return m_FacebookEvents;
            }
        }

        public List<Checkin> FacebookCheckins
        {
            get
            {
                fetchCheckins();
                return m_FacebookCheckin;
            }
        }

        public string LoginInFacebook()
        {
            string resultErrorMessage = string.Empty;
            LoginResult result = new LoginResult();  
            result = FacebookService.Login("522385301179083", "user_about_me", "user_friends", "friends_about_me", "publish_stream", "user_events", "read_stream", "user_status");
            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                LoggedInUser = result.LoggedInUser;
            }
            else
            {
                resultErrorMessage = result.ErrorMessage;
            }

            return resultErrorMessage;
        }

        private void fetchNewsFeed()
        {
            m_Posts = new List<Post>();

            foreach (Post postnews in LoggedInUser.NewsFeed)
            {
                m_Posts.Add(postnews);
            }
        }

        private void fetchFriends()
        {
            m_UserFriends = new List<User>();

            foreach (User friend in LoggedInUser.Friends)
            {
                m_UserFriends.Add(friend);
            }
        }

        private void fetchEvents()
        {
            m_FacebookEvents = new List<Event>();
            foreach (Event fbEvent in LoggedInUser.Events)
            {
                m_FacebookEvents.Add(fbEvent);
            }
        }

        private void fetchCheckins()
        {
            foreach (Checkin checkin in LoggedInUser.Checkins)
            {
                m_FacebookCheckin = new List<Checkin>();
                m_FacebookCheckin.Add(checkin);
            }
        }
    }
}
