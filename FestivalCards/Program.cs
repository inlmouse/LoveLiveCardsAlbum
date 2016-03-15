using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace FestivalCards
{
    class Program
    {
        static void Main(string[] args)
        {
            string Url = "http://schoolido.lu/cards/?page=";
            string strResult = null;
            try
            {
                for (int x = 1; x < 95; x++)
                {
                    string urlx = Url + x;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlx);
                    request.Timeout = 30000;
                    //out time setting
                    //request.Headers.Set("Pragma", "no-cache");
                    request.UserAgent = "Sync";//fake here
                    request.Accept = "*/*";
                    request.UseDefaultCredentials = true;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();//403 no access authorization, re-fake UserAgent.
                    Stream streamReceive = response.GetResponseStream();
                    Encoding encoding = Encoding.GetEncoding("GB2312");
                    StreamReader streamReader = new StreamReader(streamReceive, encoding);
                    strResult = streamReader.ReadToEnd();
                    string[] picres = MatchExpr(strResult, "img src=\"" + @"http://i.schoolido.lu/cards/\d*\w*\.png");
                    string[] filenames = new string[picres.Length];
                    string[] names = new string[picres.Length];
                    for (int i = 0; i < picres.Length; i++)
                    {
                        picres[i] = picres[i].Substring(9);
                        filenames[i] = picres[i].Substring(picres[i].LastIndexOf("/") + 1);
                        if (filenames[i].IndexOf("lized") >= 0)
                        {
                            names[i] = filenames[i].Substring(filenames[i].IndexOf("lized") + 5,
                                filenames[i].LastIndexOf(".") - filenames[i].IndexOf("lized") - 5);

                        }
                        else if (filenames[i].IndexOf("Round") >= 0)
                        {
                            names[i] = filenames[i].Substring(filenames[i].IndexOf("Round") + 5,
                                filenames[i].LastIndexOf(".") - filenames[i].IndexOf("Round") - 5);
                        }
                        else
                        {
                            names[i] = filenames[i].Substring(isLetter(filenames[i]),
                                filenames[i].LastIndexOf(".") - isLetter(filenames[i]));
                        }
                        //Console.WriteLine(picres[i]);
                        //Console.WriteLine(filenames[i]);
                        //Console.WriteLine(names[i]);
                        DownloadCards(picres[i], names[i], filenames[i]);
                    }
                }
                
                Console.WriteLine("\nAll Cards Download Complete!\n");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                //Console.WriteLine("Access Denied");
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private static string[] MatchExpr(string text, string expr)
        {
            MatchCollection mc = Regex.Matches(text, expr);
            string[] res = new string[mc.Count];
            int i = 0;
            foreach (Match m in mc)
            {
                res[i] = m.ToString();
                i++;
            }
            return res;
        }

        private static void DownloadCards(string URL, string name, string FileName)
        {
            try
            {
                WebClient myWebClient = new WebClient();
                myWebClient.UseDefaultCredentials = true;
                myWebClient.Headers.Add("User-Agent: Other");
                string cpath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\FestivalCards\"+name;
                if (!Directory.Exists(cpath))
                {
                    Directory.CreateDirectory(cpath);
                }
                if (!File.Exists(cpath + @"\" + FileName))
                {
                    myWebClient.DownloadFile(URL, cpath + @"\" + FileName);
                    Console.WriteLine(FileName + " Downloaded");
                }
                Console.WriteLine(FileName + " is already exist");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        public static int isLetter(string validString)
        {
            byte[] tempbyte = System.Text.Encoding.Default.GetBytes(validString);
            int pos = 0;
            for (int i = 0; i < validString.Length; i++)
            {
                byte by = tempbyte[i];
                if ((by >= 65) && (by <= 90) || ((by >= 97) && (by <= 122)))
                {
                    break;
                }
                pos++;
            }
            return pos;
        }
    }
}
