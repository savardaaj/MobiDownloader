using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net.Mail;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System;
using System.Diagnostics;

/**
 * For referecing HTML Nodes - "//a[@href]" get a href 
 *                             "//img/@src" get img src
 *  
 */

namespace MobiDownloader
{
    public partial class Form1 : Form
    {
        private static readonly log4net.ILog log
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        HtmlAgilityPack.HtmlDocument htmlDoc;
        NetworkCredential credentials;
        OpenFileDialog fileDialog;
        WebClient wc;
        HttpWebRequest request;
        MailMessage mail;
        SmtpClient client;
        Uri websiteURI;
        WebResponse response;
        Stream stream;
        StreamReader reader;
        string downloadFolder;
        Boolean found;
        StreamReader readerCopy;
        string fileType;
        string fileName;
        string uriFileName = "";
        string filePath;
        string coverImage = "";

        string desiredString = "";
        string dlPageLink = "";

        public Form1()
        {
            InitializeComponent();
            textBoxDownloadLoc.Text = @"C:\Users\" + Environment.UserName + @"\Downloads\";
            textBoxToEmail.Text = "savarda91_01@kindle.com";
            textBoxToEmail.AcceptsReturn = true;
            textBoxFileName.AcceptsReturn = true;
            textBoxFileName.KeyDown += new KeyEventHandler(tb_FileName);
            textBoxToEmail.KeyDown += new KeyEventHandler(tb_ToEmail);
        }       

        private void Initialize()
        {
            try
            {
                listBoxLog.Items.Add("Initializing");
                //    progressBar.Value = 0; //reset prog bar
                labelProgress.Text = "Downloading";
                found = false;
                fileName = textBoxFileName.Text;
                uriFileName = Regex.Replace(fileName, @"\s+", "+");
                websiteURI = new Uri("http://libgen.io/search.php?req=" + fileName); //declare page to search
                downloadFolder = textBoxDownloadLoc.Text;
                htmlDoc = new HtmlAgilityPack.HtmlDocument();
            }
            catch (Exception e)
            {
                log.Info(e.ToString());
                listBoxLog.Items.Add(e.ToString());
            }
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            try
            {
                Initialize();

                try
                {
                    listBoxLog.Items.Add("Creating web request");
                    request = (HttpWebRequest)HttpWebRequest.Create(websiteURI);
                    request.UserAgent = "A .NET Web Crawler";

                    //Get the HTML text / Load it into parser

                    using (response = request.GetResponse())
                    {
                        stream = response.GetResponseStream();
                        Stream streamCopy = new MemoryStream();
                        stream.CopyTo(streamCopy);

                        reader = new StreamReader(streamCopy);
                        streamCopy.Position = 0;
                        readerCopy = new StreamReader(streamCopy);
                        listBoxLog.Items.Add("Reading response");

                        using (reader)
                        {
                            //Check for mobi first
                            while (!found)
                            {
                                desiredString = reader.ReadLine();
                                if (!(desiredString == null))
                                {
                                    if(desiredString.Contains("English</td>"))
                                    {
                                        desiredString = reader.ReadLine();
                                        desiredString = reader.ReadLine();
                                        if (desiredString.Contains("mobi</td>"))
                                        {
                                            desiredString = reader.ReadLine(); //line with link I want            
                                            found = true;
                                            fileType = ".mobi";
                                            listBoxLog.Items.Add(".mobi found");
                                        }
                                    }
                                }
                                else
                                {
                                    listBoxLog.Items.Add(".mobi not found");
                                    break;
                                }
                            }

                            streamCopy.Position = 0;
                            readerCopy = new StreamReader(streamCopy);

                            while (!found)
                            {
                                desiredString = readerCopy.ReadLine();
                                if (!(desiredString == null))
                                {
                                    if (desiredString.Contains("English</td>"))
                                    {
                                        desiredString = readerCopy.ReadLine();
                                        desiredString = readerCopy.ReadLine();
                                        if (desiredString.Contains("pdf</td>"))
                                        {
                                            desiredString = readerCopy.ReadLine();
                                            found = true;
                                            fileType = ".pdf";
                                            listBoxLog.Items.Add(".pdf found");
                                            coverImage = GetCoverImage(fileName);
                                        }
                                    }
                                }
                                else
                                {
                                    listBoxLog.Items.Add(".pdf not found");
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception es)
                {
                    log.Info(es.ToString());
                    listBoxLog.Items.Add(es.ToString());
                }

                labelProgress.Text = "Downloading " + fileType;

                if (desiredString == null)
                {
                    MessageBox.Show("Book not found. Please check spelling and try again");
                    return;
                }

                htmlDoc.LoadHtml(desiredString);
                dlPageLink = ExtractAllHrefTags(htmlDoc); //extract all href from my td to find link

                if (!dlPageLink.Equals(""))
                {
                    try
                    {
                        request = (HttpWebRequest)HttpWebRequest.Create(dlPageLink);
                        request.UserAgent = "A .NET Web Crawler";

                        //Get the HTML text / Load it into parser

                        using (response = request.GetResponse())
                        {
                            stream = response.GetResponseStream();
                            reader = new StreamReader(stream);

                            found = false;
                            while (!found)
                            {
                                desiredString = reader.ReadLine();
                                if (desiredString.Contains("download.libgen.io"))
                                {
                                    htmlDoc.LoadHtml(desiredString);
                                    dlPageLink = ExtractAllHrefTags(htmlDoc);
                                    found = true;
                                }
                            }

                        }
                    }
                    catch (Exception ea)
                    {
                        log.Info(ea.ToString());
                        listBoxLog.Items.Add(ea.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Error occurred: first link not found");
                }

                if (!dlPageLink.Equals(""))
                {

                    if (!textBoxFileSave.Text.Equals(""))
                    {
                        filePath = downloadFolder + textBoxFileSave.Text + fileType; //C:\users\Alex\Downloads\Outliers Malcolm Gladwell.mobi
                    }
                    else
                    {
                        filePath = downloadFolder + fileName + fileType; //C:\users\Alex\Downloads\Outliers Malcolm Gladwell.mobi
                    }
                    wc = new WebClient();
                    wc.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0)");
                    wc.DownloadProgressChanged += (sende, ex) =>
                        {
                            progressBar.Value = ex.ProgressPercentage;
                            labelProgress.Text = "Downloaded " + ex.BytesReceived + " of " + ex.TotalBytesToReceive + " " + fileType;
                        };
                    wc.DownloadFileCompleted += (sendera, exa) =>
                        {
                            if (exa.Cancelled)
                            {
                                progressBar.Value = 0;
                                labelProgress.Text = "Cancelled";
                                return;
                            }
                            MessageBox.Show(fileName + " download completed");
                        };
                    wc.DownloadFileAsync(new Uri(dlPageLink), filePath);
                    coverImage = GetCoverImage(fileName);

                }
                else
                {
                    MessageBox.Show("Error occurred: second link not found");
                }
            }
            catch(Exception s)
            {
                log.Info(s.ToString());
            }
        }

        private string ExtractAllHrefTags(HtmlAgilityPack.HtmlDocument htmlSnippet)
        {
            try
            {
                List<string> hrefTags = new List<string>();

                foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute att = link.Attributes["href"];

                    if (att.Value.ToString().Contains("http://libgen.io") || att.Value.ToString().Contains("http://download.libgen.io"))
                    {
                        return att.Value.ToString();
                    }
                }
                
            }
            catch(Exception s)
            {
                log.Info(s.ToString());
                
            }

            return "";
        }

        private void buttonEmail_Click(object sender, EventArgs e)
        {
            try
            {
                SendEmail(sender, e, filePath);
            }
            catch (Exception ek)
            {
                log.Info(ek.ToString());
                listBoxLog.Items.Add(ek.ToString());
            }
        }

        private string ConvertToMobi(string fileName, string coverImage)
        {
            try
            {
                string newFile = fileName.Split('.')[0] + ".mobi";
                string oldFile = fileName;
                Process converter = new Process();
                converter.StartInfo.FileName = (@"C:\Program Files\Calibre2\ebook-convert.exe");
                converter.StartInfo.Arguments = "\"" + oldFile + "\"" + " \"" + newFile + "\"" + " --cover " + "\"" + textBoxDownloadLoc.Text + coverImage + "\"";
                //converter.StartInfo.CreateNoWindow = true;
                converter.Start();
                return newFile;
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
                return "";
            }
        }

        private void SendEmail(object sender, EventArgs e, string attachment)
        {
            try
            {
                credentials = new NetworkCredential("dummymailsail@gmail.com", "1.13198824");

                if (textBoxToEmail.Text == "")
                {
                    textBoxToEmail.BackColor = System.Drawing.Color.PaleVioletRed;
                }
                else
                {
                    textBoxToEmail.BackColor = System.Drawing.Color.White;

                    mail = new MailMessage(new MailAddress("dummymailsail@gmail.com"), new MailAddress(textBoxToEmail.Text));
                    mail.Body = "Doesn't matter";
                    if (!(attachment.Split('.')[1].Equals("mobi"))) //If the attchment file is not a mobi, convert it
                    {
                        attachment = ConvertToMobi(attachment, coverImage);
                        if (!File.Exists(attachment))
                        {
                            listBoxLog.Items.Add("Attachment did not convert successfully");
                            return;
                        }
                    }
                    mail.Attachments.Add(new Attachment(attachment));
                    //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;

                    client = new SmtpClient();
                    client.EnableSsl = true;
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = "smtp.gmail.com";

                    client.Credentials = credentials;

                    client.Send(mail);
                    MessageBox.Show("Email Sent!");
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
                return;
            }
        }

        private void buttonEmailBulk_Click(object sender, EventArgs e)
        {
            try
            {
                fileDialog = new OpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.InitialDirectory = textBoxDownloadLoc.Text;
                DialogResult dr = fileDialog.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    // Read the files
                    foreach (string file in fileDialog.FileNames)
                    {
                        SendEmail(sender, e, file);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }
        }

        private void buttonCancelDownload_Click(object sender, EventArgs e)
        {
            try
            {
                client.SendAsyncCancel();
                client.Dispose();
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }
        }

        private void tb_ToEmail(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendEmail(sender, e, fileName);
            }
        }

        private void tb_FileName(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonDownload_Click(sender, e);
            }
        }

        private string GetCoverImage(string fileName)
        {
            //Search URL: https://www.goodreads.com/search?q=
            //Search Result URL: /book/show/77566.Hyperion

            string coverImage = "";

            ProcessWebRequest(fileName);


            return coverImage;
        }

        private void ProcessWebRequest(string fileName)
        {

            websiteURI = new Uri("https://www.goodreads.com/search?q=" + fileName); //declare page to search
            request = (HttpWebRequest)HttpWebRequest.Create(websiteURI);
            using (response = request.GetResponse())
            {
                stream = response.GetResponseStream();
                Stream streamCopy = new MemoryStream();
                stream.CopyTo(streamCopy);

                reader = new StreamReader(streamCopy);
                streamCopy.Position = 0;
                readerCopy = new StreamReader(streamCopy);

                string bookTitle = fileName.Split(' ')[0];
                Regex reg = new Regex(@"(\/book\/show\/[0-9]*." + bookTitle + "?)");
                Match match;

                using (reader)
                {
                    //Check for  first
                    found = false;
                    while (!found)
                    {
                        desiredString = reader.ReadLine();
                        match = reg.Match(desiredString);
                        if (match.Success)
                        {
                            htmlDoc.LoadHtml(desiredString);
                            dlPageLink = ExtractFromHtml(htmlDoc, "//a[@href]", "href");
                            found = true;
                        }
                    }
                }
            }

            if (!dlPageLink.Equals(""))
            {
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create("https://www.goodreads.com" + dlPageLink);
                    request.UserAgent = "A .NET Web Crawler";

                    //Get the HTML text / Load it into parser

                    using (response = request.GetResponse())
                    {
                        stream = response.GetResponseStream();
                        reader = new StreamReader(stream);

                        found = false;
                        while (!found)
                        {
                            desiredString = reader.ReadLine();
                            if (desiredString.Contains("<div class=\"editionCover\""))
                            {
                                desiredString = reader.ReadLine();
                                htmlDoc.LoadHtml(desiredString);
                                dlPageLink = ExtractFromHtml(htmlDoc, "//img/@src", "src");

                                found = true;
                                wc = new WebClient();
                                wc.DownloadFileAsync(new Uri(dlPageLink), textBoxDownloadLoc.Text + fileName + ".png");
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    log.Info(ex);
                }
            }
        }

        private string ExtractFromHtml(HtmlAgilityPack.HtmlDocument htmlSnippet, string attribute, string attr)
        {
            try
            {
                foreach (HtmlNode node in htmlSnippet.DocumentNode.SelectNodes(attribute))
                {
                    HtmlAttribute att = node.Attributes[attr];

                    return att.Value.ToString();
                }
            }
            catch (Exception s)
            {
                log.Info(s.ToString());
            }

            return "";
        }
    }
}
