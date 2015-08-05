using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Google.YouTube;

namespace FBBasicFacebookFeature
{
    internal partial class Form1 : Form
    {
        private string m_ErrorMesage;
        private FacebookManager m_FacebookManager;
        private Paint m_Paint;
        private IYoutube m_Youtube;
        private ExceptionsXml m_XmlExc = ExceptionsXml.GetInstance;
        private IMDb m_MovieImdbSearch;
        private FinanceDecorator m_FinanceDecorator;

        public Form1()
        {
            InitializeComponent();
            m_FacebookManager = new FacebookManager();
            m_Paint = new Paint();
            m_Youtube = new YoutubeProxy();
            axShockwaveFlashYoutube.Visible = false;
            initializePanel(panelPaint.Width, panelPaint.Height);
            listBoxFavoritesVideos.DataSource = m_Youtube.MyFavoritesVideos;
            loadFeauter4();    
        }

        private void loadFeauter4()
        {
            m_FinanceDecorator = new FinanceDecorator();
            m_FinanceDecorator.Add(observerTextBoxTotalExpend);
            List<FinanceTrack> list = new List<FinanceTrack>();
            m_FinanceDecorator.LoadFinanceDocument(ref list);
            foreach (FinanceTrack track in list)
            {
                listBoxFinanceTrack.Items.Add(track);
            }
        }

        private void buttonLoginToFacebook_Click(object sender, EventArgs e)
        {
            m_ErrorMesage = m_FacebookManager.LoginInFacebook();
            if (!string.IsNullOrEmpty(m_ErrorMesage))
            {
                MessageBox.Show(m_ErrorMesage);
            }
            else
            {
                fetchUserInfo(m_ErrorMesage);
            }
        }

        private void fetchUserInfo(string loginResultErrorMessage)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {
                picture_smallPictureBox.LoadAsync(m_FacebookManager.LoggedInUser.PictureNormalURL);
                if (m_FacebookManager.LoggedInUser.Statuses.Count > 0)
                {
                    textBoxBasicFeatureStatus.Text = m_FacebookManager.LoggedInUser.Statuses[0].Message;
                }
            }
        }

        private void linkFriends_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Thread(threadFacebookFetchFriends).Start();
        }

        private void threadFacebookFetchFriends()
        {
            listBoxEvents.Invoke(new Action(() =>
            {
                listBoxFriends.DisplayMember = "name";
                listBoxFriends.DataSource = m_FacebookManager.UserFriends;
            }));
        }

        private void displayFriends(ListBox i_ListBox)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {
                i_ListBox.DisplayMember = "Name";
                i_ListBox.DataSource = m_FacebookManager.UserFriends;
            }
        }

        private void labelEvents_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {
                new Thread(threadEventFacebook).Start();
            }
        }

        private void threadEventFacebook()
        {
            listBoxEvents.Invoke(new Action(() =>
            {
                listBoxEvents.DataSource = m_FacebookManager.FacebookEvents;
            }));
        }

        private void linkCheckins_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {
                new Thread(threadChekinFacebook).Start();
            }
        }

        private void threadChekinFacebook()
        {
            listBoxCheckins.Invoke(new Action(() =>
            {
                listBoxCheckins.DataSource = m_FacebookManager.FacebookCheckins;
            }));
        }

        private void listBoxFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            displaySelectedFriend();
        }

        private void displaySelectedFriend()
        {
            if (listBoxFriends.SelectedItems.Count == 1)
            {
                FacebookWrapper.ObjectModel.User selectedFriend = listBoxFriends.SelectedItem as FacebookWrapper.ObjectModel.User;
                if (selectedFriend.PictureNormalURL != null)
                {
                    pictureBoxFriend.LoadAsync(selectedFriend.PictureNormalURL);
                }
                else
                {
                    picture_smallPictureBox.Image = picture_smallPictureBox.ErrorImage;
                }
            }
        }

        private void linkNewsFeed_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (m_FacebookManager.LoggedInUser != null)
            {
                new Thread(threadNewsFacebook).Start();
            }
        }

        private void threadNewsFacebook()
        {
            listBoxEvents.Invoke(new Action(() =>
            {
                listBoxNewsFeed.DataSource = m_FacebookManager.News;
            }));
        }

        private void showConnectedMessage()
        {
            MessageBox.Show("Sorry, must be connected");
        }

        private void buttonSetStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_FacebookManager.LoggedInUser != null && !string.IsNullOrEmpty(textBoxBasicFeatureStatus.Text))
                {
                    m_FacebookManager.LoggedInUser.PostStatus(textBoxBasicFeatureStatus.Text);
                }
                else
                {
                    showConnectedMessage();
                }
            }
            catch(Exception exceptionFacebook)
            {
                m_XmlExc.ExceptionOccurred(exceptionFacebook);
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            m_Paint.GraphicsPanel.Clear(panelPaint.BackColor);
        }

        private void panelPaint_MouseDown(object sender, MouseEventArgs e)
        {
            m_Paint.Paints = true;
        }

        private void panelPaint_MouseUp(object sender, MouseEventArgs e)
        {
            m_Paint.Paints = false;
        }

        private void panelPaint_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && m_Paint.Paints && (m_Paint.Pencil || m_Paint.Eraser || m_Paint.Brush))
                {
                    m_Paint.GraphicsPanel.FillEllipse(m_Paint.SolidBrush, e.X, e.Y, m_Paint.WidthDraw, m_Paint.HighDraw);
                    m_Paint.GraphicsBmp.FillEllipse(m_Paint.SolidBrush, e.X, e.Y, m_Paint.WidthDraw, m_Paint.HighDraw);
                }
            }
            catch(ArgumentNullException argumentNullException)
            {
                m_XmlExc.ExceptionOccurred(argumentNullException);
            }
        }

        private void toolStripButtonPencil_Click(object sender, EventArgs e)
        {
            m_Paint.ChangeTools(true, false, false);
            m_Paint.SolidBrush.Color = m_Paint.LastColor;
            m_Paint.WidthDraw = 4;
            m_Paint.HighDraw = 4;
            this.Cursor = new Cursor(m_Paint.r_Path + @"\Images\Pencil.cur");
        }

        private void toolStripButtonBrush_Click(object sender, EventArgs e)
        {
            m_Paint.ChangeTools(false, true, false);
            m_Paint.SolidBrush.Color = m_Paint.LastColor;
            m_Paint.WidthDraw = trackBarBrushSize.Value;
            m_Paint.HighDraw = trackBarBrushSize.Value;
            this.Cursor = new Cursor(m_Paint.r_Path + @"\Images\Brush.cur");
        }

        private void toolStripButtonEraser_Click(object sender, EventArgs e)
        {
            m_Paint.LastColor = m_Paint.SolidBrush.Color;
            m_Paint.ChangeTools(false, false, true);
            this.Cursor = Cursors.Cross;
            m_Paint.SolidBrush.Color = Color.White;
        }

        private void toolStripButtonChangeColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                m_Paint.SolidBrush.Color = colorDialog.Color;
                m_Paint.LastColor = m_Paint.SolidBrush.Color;
            }
        }

        private void toolStripButtonMousePointer_Click(object sender, EventArgs e)
        {
            m_Paint.WidthDraw = 0;
            m_Paint.HighDraw = 0;
            this.Cursor = Cursors.Default;
        }

        private void trackBarBrushSize_Scroll(object sender, EventArgs e)
        {
            if (m_Paint.Brush || m_Paint.Eraser)
            {
                labelShowBrushSize.Text = trackBarBrushSize.Value.ToString();
                m_Paint.WidthDraw = trackBarBrushSize.Value;
                m_Paint.HighDraw = trackBarBrushSize.Value;
            }
        }

        private void initializePanel(int i_Width, int i_Height)
        {
            m_Paint.Bmp = new Bitmap(i_Width, i_Height);
            m_Paint.GraphicsBmp = Graphics.FromImage(m_Paint.Bmp);
            m_Paint.GraphicsPanel = panelPaint.CreateGraphics();
        }

        private void loadBackgroungImgae(Image i_ImageToLoad)
        {
            panelPaint.BackgroundImage = i_ImageToLoad;
            m_Paint.GraphicsBmp.DrawImage(i_ImageToLoad, 0, 0, panelPaint.Size.Width, panelPaint.Size.Height);
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Image backGroundImage = Image.FromStream(openFileDialog1.OpenFile());
                    loadBackgroungImgae(backGroundImage);
                }
            }
            catch(ArgumentNullException argumentNullException)
            {
                m_XmlExc.ExceptionOccurred(argumentNullException);
            }
            catch(ArgumentException argumentException)
            {
                m_XmlExc.ExceptionOccurred(argumentException);
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog.OverwritePrompt = false;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog.FileName.EndsWith(".jpg"))
                    {
                        m_Paint.Bmp.Save(saveFileDialog.FileName);
                    }
                    else
                    {
                        m_Paint.Bmp.Save(saveFileDialog.FileName + ".jpg");
                    }

                    MessageBox.Show("The picture has been saved");
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                m_XmlExc.ExceptionOccurred(argumentNullException);
            }
            catch (ExternalException externalException)
            {
                m_XmlExc.ExceptionOccurred(externalException);
            }
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_FacebookManager.LoggedInUser != null)
                {
                    byte[] imageData = m_Paint.ConvertBitMapToByteArray();
                    m_FacebookManager.LoggedInUser.PostPhoto(imageData, textBoxPaintFeatureStatus.Text);
                }
                else
                {
                    showConnectedMessage();
                }
            }
            catch(Exception facebookException)
            {
                m_XmlExc.ExceptionOccurred(facebookException);
            }
        }

        private void TabControlFacebookMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void listBoxOfSearchAndWhatNewResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            showVideoInForm(sender);
        }

        private void showVideoInForm(object i_Sender)
        {
            ListBox listBox = i_Sender as ListBox;
            if (listBox != null)
            {
                axShockwaveFlashYoutube.Visible = true;
                if (listBox.SelectedItems.Count == 1)
                {
                    m_Youtube.Video = listBox.SelectedItem as Video;
                    if (m_Youtube.Video != null)
                    {
                        axShockwaveFlashYoutube.Movie = m_Youtube.GetSelectedVideo();
                        textBoxVideoDescription.Text = m_Youtube.Video.Media.Description.Value;
                    }
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            new Thread(threadYoutubeSearch).Start();
        }

        private void threadYoutubeSearch()
        {
            listBoxVideos.Invoke(new Action(() =>
            {
                listBoxVideos.DataSource = null;
                listBoxVideos.Items.Clear();

                if (!string.IsNullOrEmpty(textBoxSearch.Text))
                {
                    listBoxVideos.DataSource = m_Youtube.FindVideos(textBoxSearch.Text, m_Youtube.VideoFeeds);
                    axShockwaveFlashYoutube.Visible = true;
                }
                else
                {
                    MessageBox.Show("must enter value");
                }    
            }));     
        }

        private void buttonFavorites_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxVideos.SelectedItems.Count == 1)
                {
                    if (!m_Youtube.AddToMyFavorites(m_Youtube.Video))
                    {
                        MessageBox.Show("video was added");
                        updateFormData(m_Youtube.MyFavoritesVideos, listBoxFavoritesVideos);
                    }
                    else
                    {
                        MessageBox.Show("This video already exist");
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, must choose video");
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                MessageBox.Show(argumentNullException.Message);
                m_XmlExc.ExceptionOccurred(argumentNullException);
            }
        }

        private void SearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateFormData(m_Youtube.VideoFeeds, listBoxVideos);        
        }

        private void buttonRemoveFromFavorites_Click(object sender, EventArgs e)
        {
            if(listBoxFavoritesVideos.SelectedItems.Count == 1)
            {
                m_Youtube.RemoveVideoFromFavorites(listBoxFavoritesVideos.SelectedIndex);
                updateFormData(m_Youtube.MyFavoritesVideos, listBoxFavoritesVideos);
            }
            else
            {
                MessageBox.Show("Sorry, there is nothing to remove");
            }
        }

        private void updateFormData(List<Video> i_CurrentList, ListBox i_ListBoxName)
        {
            i_ListBoxName.DataSource = null;
            i_ListBoxName.Items.Clear();
            i_ListBoxName.DataSource = i_CurrentList;
            if (listBoxFavoritesVideos.Items.Count == 0)
            {
                textBoxVideoDescription.Text = string.Empty;
            }
        }

        private void whatsNewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            updateFormData(m_Youtube.FindVideos(string.Empty, m_Youtube.NewVideos), listBoxVideos);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            displayFriends(listBoxPaintFreinds);
        }

        private void listBoxFethcFriendsAndPicToDraw_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPaintFreinds.SelectedItems.Count == 1)
            {
                FacebookWrapper.ObjectModel.User selectedFriend = listBoxPaintFreinds.SelectedItem as FacebookWrapper.ObjectModel.User;
                if (selectedFriend.PictureNormalURL != null)
                {
                    Image loadUserPicture = selectedFriend.ImageLarge;
                    loadBackgroungImgae(loadUserPicture);
                }
            }
        }

        private void buttonPostYoutubeVideo_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_FacebookManager.LoggedInUser != null)
                {
                    if (listBoxFavoritesVideos.SelectedItems.Count == 1)
                    {
                        byte[] imageData = m_Paint.ConvertBitMapToByteArray();
                        Video selectedVideo = listBoxFavoritesVideos.SelectedItem as Video;

                        if (selectedVideo != null)
                        {
                            m_FacebookManager.LoggedInUser.PostLink("https://www.youtube.com/watch?v=" + selectedVideo.VideoId, textBoxYoutubeFeatureStatus.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sorry, have to choose video from favorites");
                    }
                }
                else
                {
                    showConnectedMessage();
                }
            }
            catch (Exception facebookException)
            {
                m_XmlExc.ExceptionOccurred(facebookException);
            }
        }

        private void listBoxFavoritesVideos_SelectedIndexChanged(object sender, EventArgs e)
        {
            showVideoInForm(sender);   
        }

        private void buttonSearchImdb_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBoxImdbSearch.Text))
            {
                   new Thread(fetchMovies).Start();  
            }
        }
      
        private void fetchMovies()
        {
            try
            {
                ISearch searchFromMovie = new ChacheMemorySearch();
                if (!searchFromMovie.Search(textBoxImdbSearch.Text, out m_MovieImdbSearch))
                {
                    searchFromMovie = new SearchInImdb();
                    searchFromMovie.Search(textBoxImdbSearch.Text, out m_MovieImdbSearch);
                }

                if (!buttonSearchImdb.InvokeRequired)
                {
                    iMDbBindingSource.DataSource = m_MovieImdbSearch;
                }
                else
                {
                    buttonSearchImdb.Invoke(new Action(() => iMDbBindingSource.DataSource = m_MovieImdbSearch));
                }
            }
            catch(Exception exceptionFindingMovie)
            {
                m_XmlExc.ExceptionOccurred(exceptionFindingMovie);
            }
        }

        private void buttonAddTrack_Click(object sender, EventArgs e)
        {
            double amountSpent = 0;
            if (double.TryParse(textBoxAmount.Text, out amountSpent))
            {
                if (amountSpent < 0)
                {
                    MessageBox.Show("Please Enter Positive Numbers");
                }
                else
                {
                    IFinance financeDecorator = new FinanceDecorator(new FinanceTrack(amountSpent, textBoxReason.Text, DateTime.Now));
                    (financeDecorator as FinanceDecorator).AddNewTrack();
                    listBoxFinanceTrack.Items.Add((financeDecorator as FinanceDecorator).Finance);
                }
            }
            else
            {
                MessageBox.Show("Please Enter Amount Only With Numbers");
            }
        }

        private void buttonRemoveFromList_Click(object sender, EventArgs e)
        {
            if (!(listBoxFinanceTrack.SelectedIndex == -1))
            {
                listBoxFinanceTrack.Items.Remove(listBoxFinanceTrack.SelectedItem);
                finCalcWithChanges();
            }
            else
            {
                MessageBox.Show("Choose A Transaction To Remove");
            }
        }

        private void EditItem_Click(object sender, EventArgs e)
        {
                if (!(listBoxFinanceTrack.SelectedIndex == -1))
                {
                    int indexListBox = listBoxFinanceTrack.SelectedIndex;
                    FinanceTrack financeTrackFromListbox = (FinanceTrack)listBoxFinanceTrack.SelectedItem;
                    EditFormForFinance formDialogForEdit = new EditFormForFinance(financeTrackFromListbox.Amount, financeTrackFromListbox.Reason);
                    formDialogForEdit.ShowDialog();
                    if(formDialogForEdit.DialogResult == DialogResult.OK)
                    {
                        financeTrackFromListbox.Amount = formDialogForEdit.Amount;
                        financeTrackFromListbox.Reason = formDialogForEdit.Comment;
                        listBoxFinanceTrack.Items[indexListBox] = financeTrackFromListbox;
                        finCalcWithChanges();
                    }        
                }
                else
                {
                    MessageBox.Show("Choose A Transaction To Edit");
                }
        }

        private void finCalcWithChanges()
        {
            List<FinanceTrack> listFinanceTrack = listBoxFinanceTrack.Items.Cast<FinanceTrack>().ToList();
            m_FinanceDecorator.SaveFinanceTrack(listFinanceTrack);
        }
    }
}