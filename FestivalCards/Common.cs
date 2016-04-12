using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace FestivalCards
{
    class Common
    {
        static Random m_rnd = new Random();
        public static char getRandomChar()
        {
            int ret = m_rnd.Next(122);
            while (ret < 48 || (ret > 57 && ret < 65) || (ret > 90 && ret < 97))
            {
                ret = m_rnd.Next(122);
            }
            return (char)ret;
        }
        public static string getRandomString(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(getRandomChar());
            }
            return sb.ToString();
        }     

        public static int isLetter(string validString)
        {
            byte[] tempbyte = Encoding.Default.GetBytes(validString);
            int pos = 0;
            for (int i = 0; i < validString.Length; i++)
            {
                byte by = tempbyte[i];
                if ((@by >= 65) && (@by <= 90) || ((@by >= 97) && (@by <= 122)))
                {
                    break;
                }
                pos++;
            }
            return pos;
        }

        public static void DownloadCards(string URL, string name, string FileName)
        {
            try
            {
                WebClient myWebClient = new WebClient();
                myWebClient.UseDefaultCredentials = true;
                myWebClient.Headers.Add("User-Agent: Other");
                string cpath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\IdolMasters\" + name;
                if (!Directory.Exists(cpath))
                {
                    Directory.CreateDirectory(cpath);
                }
                if (!File.Exists(cpath + @"\" + FileName))
                {
                    myWebClient.DownloadFile(URL, cpath + @"\" + FileName);
                    Console.WriteLine(FileName + " Downloaded");
                }
                else
                {
                    Console.WriteLine(FileName + " is already exist");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //throw;
                //continue;
            }
        }

        public static string GetWebContent(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Timeout = 30000;
            //out time setting
            //request.Headers.Set("Pragma", "no-cache");
            request.UserAgent = getRandomString(7);//"Sync";//fake here
            request.Accept = "*/*";
            request.UseDefaultCredentials = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//403 no access authorization, re-fake UserAgent.
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.GetEncoding("GB2312");
            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            return strResult;
        }

        public static string[] MatchExpr(string text, string expr)
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
    }
}
