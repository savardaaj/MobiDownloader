using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net.Mail;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System;

namespace MobiDownloader
{
    public partial class Form1 : Form
    {
        private static readonly log4net.ILog log
            = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        HtmlAgilityPack.HtmlDocument htmlDoc;
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
        string fileType;
        string fileName;
        string uriFileName = "";
        string filePath;

        string desiredString = "";
        string dlPageLink = "";

        public Form1()
        {
            InitializeComponent();
            textBoxDownloadLoc.Text = @"C:\Users\" + Environment.UserName + @"\Downloads\";
            this.AcceptButton = buttonDownload;            
        }       

        private void Initialize()
        {
            try
            {
                progressBar.Value = 0;
                labelProgress.Text = "Downloading";
                found = false;
                fileName = textboxMain.Text;
                uriFileName = Regex.Replace(fileName, @"\s+", "+");
                websiteURI = new Uri("http://libgen.io/search.php?req=" + fileName); //declare page to search
                downloadFolder = textBoxDownloadLoc.Text;
                htmlDoc = new HtmlAgilityPack.HtmlDocument();
            }
            catch (Exception e)
            {
                log.Info(e.ToString());
            }
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            Initialize();

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(websiteURI);
                request.UserAgent = "A .NET Web Crawler";

                //Get the HTML text / Load it into parser

                using (response = request.GetResponse())
                {
                    stream = response.GetResponseStream();
                    reader = new StreamReader(stream);
                    
                    StreamReader readerOriginal = reader;

                    
                    using (reader)
                    {
                        //Check for mobi first
                        while (!found)
                        {
                            desiredString = reader.ReadLine();
                            if (desiredString.Contains("mobi</td>"))
                            {
                                desiredString = reader.ReadLine(); //line with link I want            
                                found = true;
                                fileType = ".mobi";
                            }
                        }
                    }

                    //Check for pdf second                   
                    using (readerOriginal)
                    {
                        while (!found)
                        {
                            if (desiredString.Contains("pdf</td>"))
                            {
                                desiredString = readerOriginal.ReadLine(); //found pdf but is there a mobi?        
                                found = true;
                                fileType = ".pdf";
                            }
                        }
                    }
                }
            }
            catch (Exception es)
            {
                log.Info(es.ToString());
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
                //wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                //wc.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadCompleted);
                wc.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 8.0)");
                wc.DownloadProgressChanged += (sende, ex) =>
                    {
                        progressBar.Value = ex.ProgressPercentage;
                        labelProgress.Text = "Downloaded " + ex.BytesReceived + " of " + ex.TotalBytesToReceive;
                    };
                wc.DownloadFileCompleted += (sendera, exa) =>
                    {
                        MessageBox.Show(fileName + " download completed");                        
                    };
                wc.DownloadFileAsync(new Uri(dlPageLink), filePath);
                
            }
            else
            {
                MessageBox.Show("Error occurred: second link not found");
            }
        }

        private string ExtractAllHrefTags(HtmlAgilityPack.HtmlDocument htmlSnippet)
        {
            List<string> hrefTags = new List<string>();

            foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
            {

                HtmlAttribute att = link.Attributes["href"];
                
                if(att.Value.ToString().Contains("http://libgen.io") || att.Value.ToString().Contains("http://download.libgen.io"))
                {
                    return att.Value.ToString();
                }               
            }
            return "";
        }

        private void buttonEmail_Click(object sender, EventArgs e)
        {
            try
            {
                mail = new MailMessage(textBoxFromEmail.Text, textBoxToEmail.Text);
                client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.google.com";
                if (fileType.Equals(".mobi"))
                {
                    mail.Subject = "";
                }
                else if (fileType.Equals(".pdf"))
                {
                    mail.Subject = "convert";
                }

                mail.Body = "";
                client.Send(mail);
                MessageBox.Show("Email Sent!");
            }
            catch (Exception ek)
            {
                log.Info(ek.ToString());
            }

        }
    }
}
