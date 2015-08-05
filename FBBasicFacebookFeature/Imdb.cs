using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Xml.Serialization;

namespace FBBasicFacebookFeature
{
    /// <summary>
    /// !!!!This class Was took from internet becasue we had an error with datasource and dll from outside,
    /// so we added a class himself from internet
    /// </summary>
    [Serializable]
    public class IMDb
    {
        private ExceptionsXml m_XmlException = ExceptionsXml.GetInstance;

        private Image image;

        [XmlIgnore]
        public Image Picture
        {
            get
            {
                loadImage();
                return image;
            }
        }

        [XmlIgnore]
        public bool status { get; set; }

        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Title")]
        public string Title { get; set; }

        [XmlElement("OriginalTitle")]
        public string OriginalTitle { get; set; }

        [XmlElement("Year")]
        public string Year { get; set; }

        [XmlElement]
        public string Rating { get; set; }
        [XmlIgnore]
        public ArrayList Genres { get; set; }
        [XmlIgnore]
        public ArrayList Directors { get; set; }
        [XmlIgnore]
        public ArrayList Writers { get; set; }
        [XmlIgnore]
        public ArrayList Cast { get; set; }
        [XmlIgnore]
        public ArrayList Producers { get; set; }
        [XmlIgnore]
        public ArrayList Musicians { get; set; }
        [XmlIgnore]
        public ArrayList Cinematographers { get; set; }
        [XmlIgnore]
        public ArrayList Editors { get; set; }

        [XmlElement("MpaaRating")]
        public string MpaaRating { get; set; }

        [XmlElement("ReleaseDate")]
        public string ReleaseDate { get; set; }

        [XmlIgnore]
        public string Plot { get; set; }

        [XmlIgnore]
        public ArrayList PlotKeywords { get; set; }

        [XmlIgnore]
        public string Poster { get; set; }

        [XmlIgnore]
        public string PosterLarge { get; set; }

       [XmlElement("PostreFull")]
        public string PosterFull { get; set; }

        [XmlElement("Runtime")]
        public string Runtime { get; set; }

        [XmlElement("Top250")]
        public string Top250 { get; set; }

        [XmlElement("Oscars")]
        public string Oscars { get; set; }

        [XmlElement("Awards")]
        public string Awards { get; set; }

        [XmlElement("Nominations")]
        public string Nominations { get; set; }

        [XmlElement("Storyline")]
        public string Storyline { get; set; }

        [XmlElement("Tagline")]
        public string Tagline { get; set; }

        [XmlElement("Votes")]
        public string Votes { get; set; }

        [XmlIgnore]
        public ArrayList Languages { get; set; }

        [XmlIgnore]
        public ArrayList Countries { get; set; }

        [XmlIgnore]
        public ArrayList ReleaseDates { get; set; }

        [XmlIgnore]
        public ArrayList MediaImages { get; set; }

        [XmlIgnore]
        public ArrayList RecommendedTitles { get; set; }

        [XmlElement("ImdbURL")]
        public string ImdbURL { get; set; }

        private string GoogleSearch = "http://www.google.com/search?q=imdb+";
        private string BingSearch = "http://www.bing.com/search?q=imdb+";
        private string AskSearch = "http://www.ask.com/web?q=imdb+";

        public IMDb()
        {
        }

        public IMDb(string MovieName, bool GetExtraInfo = true)
        {
            string imdbUrl = getIMDbUrl(System.Uri.EscapeUriString(MovieName));
            status = false;
            if (!string.IsNullOrEmpty(imdbUrl))
            {
                parseIMDbPage(imdbUrl, GetExtraInfo);
            }

            loadImage();
        }

       private static string stripHTML(string inputString)
        {
            return Regex.Replace(inputString, @"<.*?>", string.Empty);
        }

        public string getIMDbUrl(string MovieName, string searchEngine = "google")
        {
            string returnValue = string.Empty;
            string url = GoogleSearch + MovieName;
            if (searchEngine.ToLower().Equals("bing"))
            {
                url = BingSearch + MovieName;
            }

            if (searchEngine.ToLower().Equals("ask"))
            {
                url = AskSearch + MovieName;
            }

            string html = getUrlData(url);
            ArrayList imdbUrls = matchAll(@"<a href=""(http://www.imdb.com/title/tt\d{7}/)"".*?>.*?</a>", html);
            if (imdbUrls.Count > 0)
            {
                return (string)imdbUrls[0];
            }
            else
            {
                if (searchEngine.ToLower().Equals("google"))
                {
                    returnValue = getIMDbUrl(MovieName, "bing");
                }
                else if (searchEngine.ToLower().Equals("bing"))
                {
                    returnValue = getIMDbUrl(MovieName, "ask");
                }
                else
                {
                    returnValue = string.Empty;
                }
            }

            return returnValue;
        }

        private void parseIMDbPage(string imdbUrl, bool GetExtraInfo)
        {
            string html = getUrlData(imdbUrl + "combined");
            Id = match(@"<link rel=""canonical"" href=""http://www.imdb.com/title/(tt\d{7})/combined"" />", html);
            if (!string.IsNullOrEmpty(Id))
            {
                status = true;
                Title = match(@"<title>(IMDb \- )*(.*?) \(.*?</title>", html, 2);
                OriginalTitle = match(@"title-extra"">(.*?)<", html);
                Year = match(@"<title>.*?\(.*?(\d{4}).*?\).*?</title>", html);
                Rating = match(@"<b>(\d.\d)/10</b>", html);
                Genres = matchAll(@"<a.*?>(.*?)</a>", match(@"Genre.?:(.*?)(</div>|See more)", html));
                Directors = matchAll(@"<td valign=""top""><a.*?href=""/name/.*?/"">(.*?)</a>", match(@"Directed by</a></h5>(.*?)</table>", html));
                Writers = matchAll(@"<td valign=""top""><a.*?href=""/name/.*?/"">(.*?)</a>", match(@"Writing credits</a></h5>(.*?)</table>", html));
                Producers = matchAll(@"<td valign=""top""><a.*?href=""/name/.*?/"">(.*?)</a>", match(@"Produced by</a></h5>(.*?)</table>", html));
                Musicians = matchAll(@"<td valign=""top""><a.*?href=""/name/.*?/"">(.*?)</a>", match(@"Original Music by</a></h5>(.*?)</table>", html));
                Cinematographers = matchAll(@"<td valign=""top""><a.*?href=""/name/.*?/"">(.*?)</a>", match(@"Cinematography by</a></h5>(.*?)</table>", html));
                Editors = matchAll(@"<td valign=""top""><a.*?href=""/name/.*?/"">(.*?)</a>", match(@"Film Editing by</a></h5>(.*?)</table>", html));
                Cast = matchAll(@"<td class=""nm""><a.*?href=""/name/.*?/"".*?>(.*?)</a>", match(@"<h3>Cast</h3>(.*?)</table>", html));
                Plot = match(@"Plot:</h5>.*?<div class=""info-content"">(.*?)(<a|</div)", html);
                PlotKeywords = matchAll(@"<a.*?>(.*?)</a>", match(@"Plot Keywords:</h5>.*?<div class=""info-content"">(.*?)</div", html));
                ReleaseDate = match(@"Release Date:</h5>.*?<div class=""info-content"">.*?(\d{1,2} (January|February|March|April|May|June|July|August|September|October|November|December) (19|20)\d{2})", html);
                Runtime = match(@"Runtime:</h5><div class=""info-content"">(\d{1,4}) min[\s]*.*?</div>", html);
                Top250 = match(@"Top 250: #(\d{1,3})<", html);
                Oscars = match(@"Won (\d+) Oscars?\.", html);
                if (string.IsNullOrEmpty(Oscars) && "Won Oscar.".Equals(match(@"(Won Oscar\.)", html)))
                {
                    Oscars = "1";
                }

                Awards = match(@"(\d{1,4}) wins", html);
                Nominations = match(@"(\d{1,4}) nominations", html);
                Tagline = match(@"Tagline:</h5>.*?<div class=""info-content"">(.*?)(<a|</div)", html);
                MpaaRating = match(@"MPAA</a>:</h5><div class=""info-content"">Rated (G|PG|PG-13|PG-14|R|NC-17|X) ", html);
                Votes = match(@">(\d+,?\d*) votes<", html);
                Languages = matchAll(@"<a.*?>(.*?)</a>", match(@"Language.?:(.*?)(</div>|>.?and )", html));
                Countries = matchAll(@"<a.*?>(.*?)</a>", match(@"Country:(.*?)(</div>|>.?and )", html));
                Poster = match(@"<div class=""photo"">.*?<a name=""poster"".*?><img.*?src=""(.*?)"".*?</div>", html);
                if (!string.IsNullOrEmpty(Poster) && Poster.IndexOf("media-imdb.com") > 0)
                {
                    Poster = Regex.Replace(Poster, @"_V1.*?.jpg", "_V1._SY200.jpg");
                    PosterLarge = Regex.Replace(Poster, @"_V1.*?.jpg", "_V1._SY500.jpg");
                    PosterFull = Regex.Replace(Poster, @"_V1.*?.jpg", "_V1._SY0.jpg");
                }
                else
                {
                    Poster = string.Empty;
                    PosterLarge = string.Empty;
                    PosterFull = string.Empty;
                }

                ImdbURL = "http://www.imdb.com/title/" + Id + "/";
                if (GetExtraInfo)
                {
                    string plotHtml = getUrlData(imdbUrl + "plotsummary");
                    Storyline = match(@"<p class=""plotpar"">(.*?)(<i>|</p>)", plotHtml);
                    ReleaseDates = getReleaseDates();
                    MediaImages = getMediaImages();
                    RecommendedTitles = getRecommendedTitles();
                }
            }
        }

        private ArrayList getReleaseDates()
        {
            ArrayList list = new ArrayList();
            string releasehtml = getUrlData("http://www.imdb.com/title/" + Id + "/releaseinfo");
            foreach (string r in matchAll(@"<tr>(.*?)</tr>", match(@"Date</th></tr>\n*?(.*?)</table>", releasehtml)))
            {
                Match rd = new Regex(@"<td>(.*?)</td>\n*?.*?<td align=""right"">(.*?)</td>", RegexOptions.Multiline).Match(r);
                list.Add(stripHTML(rd.Groups[1].Value.Trim()) + " = " + stripHTML(rd.Groups[2].Value.Trim()));
            }

            return list;
        }

        private ArrayList getMediaImages()
        {
            ArrayList list = new ArrayList();
            string mediaurl = "http://www.imdb.com/title/" + Id + "/mediaindex";
            string mediahtml = getUrlData(mediaurl);
            int pagecount = matchAll(@"<a href=""\?page=(.*?)"">", match(@"<span style=""padding: 0 1em;"">(.*?)</span>", mediahtml)).Count;
            for (int p = 1; p <= pagecount + 1; p++)
            {
                mediahtml = getUrlData(mediaurl + "?page=" + p);
                foreach (Match m in new Regex(@"src=""(.*?)""", RegexOptions.Multiline).Matches(match(@"<div class=""thumb_list"" style=""font-size: 0px;"">(.*?)</div>", mediahtml)))
                {
                    string image = m.Groups[1].Value;
                    list.Add(Regex.Replace(image, @"_V1\..*?.jpg", "_V1._SY0.jpg"));
                }
            }

            return list;
        }

        private ArrayList getRecommendedTitles()
        {
            ArrayList list = new ArrayList();
            string recUrl = "http://www.imdb.com/widget/recommendations/_ajax/get_more_recs?specs=p13nsims%3A" + Id;
            string json = getUrlData(recUrl);
            list = matchAll(@"title=\\""(.*?)\\""", json);
            HashSet<string> set = new HashSet<string>();
            foreach (string rec in list)
            {
                set.Add(rec);
            }

            return new ArrayList(set.ToList());
        }

        private string match(string regex, string html, int i = 1)
        {
            return new Regex(regex, RegexOptions.Multiline).Match(html).Groups[i].Value.Trim();
        }

        private ArrayList matchAll(string regex, string html, int i = 1)
        {
            ArrayList list = new ArrayList();
            foreach (Match m in new Regex(regex, RegexOptions.Multiline).Matches(html))
            {
                list.Add(m.Groups[i].Value.Trim());
            }

            return list;
        }

        private string getUrlData(string url)
        {
            WebClient client = new WebClient();
            Random r = new Random();
            client.Headers["X-Forwarded-For"] = r.Next(0, 255) + "." + r.Next(0, 255) + "." + r.Next(0, 255) + "." + r.Next(0, 255);
            client.Headers["User-Agent"] = "Mozilla/" + r.Next(3, 5) + ".0 (Windows NT " + r.Next(3, 5) + "." + r.Next(0, 2) + "; rv:2.0.1) Gecko/20100101 Firefox/" + r.Next(3, 5) + "." + r.Next(0, 5) + "." + r.Next(0, 5);
            Stream datastream = client.OpenRead(url);
            StreamReader reader = new StreamReader(datastream);
            StringBuilder sb = new StringBuilder();
            while (!reader.EndOfStream)
            {
                sb.Append(reader.ReadLine());
            }

            return sb.ToString();
        }

        private void loadImage()
        {
            Stream stream = null;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(PosterFull);
                HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebReponse.GetResponseStream();
                if(stream != null)
                {
                    image = Image.FromStream(stream);
                }
            }
            catch (Exception exceptionGettingPicture)
            {
                m_XmlException.ExceptionOccurred(exceptionGettingPicture);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
    }
}
