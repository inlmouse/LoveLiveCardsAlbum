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
            try
            {
                string MainURL = "http://www.project-imas.com/wiki/THE_iDOLM@STER:_Cinderella_Girls";
                string strResult = Common.GetWebContent(MainURL);
                string[] picres = Common.MatchExpr(strResult, "<a href=\"" + @"/wiki/\w*\" + "\"");
                string[] names = new string[picres.Length];
                for (int i = 1; i < picres.Length-1; i++)
                {
                    names[i] = picres[i].Substring(15, picres[i].Length - 16);
                    //Console.WriteLine(names[i]);
                    picres[i] = "http://www.project-imas.com"+picres[i].Substring(9, picres[i].Length - 10);
                    
                    //Console.WriteLine(picres[i]);
                    string ChildURL = picres[i];
                    string singleidol = Common.GetWebContent(ChildURL);
                    string[] idolpics = Common.MatchExpr(singleidol, "<a href=\"" + @"/wiki/File:\S*\.jpg" + "\"");
                    string[] filenames=new string[idolpics.Length];
                    
                    for (int j = 0; j < idolpics.Length; j++)
                    {
                        filenames[j] = idolpics[j].Substring(20,idolpics[j].Length-21);
                        //Console.WriteLine(filenames[j]);
                        idolpics[j] = "http://www.project-imas.com" + idolpics[j].Substring(9, idolpics[j].Length - 10);
                        
                        string realpic = Common.GetWebContent(idolpics[j]);
                        string[] realpics = Common.MatchExpr(realpic, @"/w/images/\S/\S\S/\S*.jpg");
                        if (realpics.Length>0)
                        {
                            string xmlurl = "http://www.project-imas.com" + realpics[0];
                            Console.WriteLine("Download from: " + xmlurl);
                            if (names[i].Length+filenames[j].Length<200)
                            {
                                Common.DownloadCards(xmlurl, names[i], filenames[j]);
                            }
                        }
                        
                    }
                    
                    //Console.ReadLine();
                }

                Console.WriteLine(picres.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                throw;
            }
           
        }


        private void LoveLive()
        {
            string Url = "http://schoolido.lu/cards/?page=";
            string strResult = null;
            try
            {
                for (int x = 1; x < 95; x++)
                {
                    string urlx = Url + x;
                    strResult = Common.GetWebContent(urlx);
                    string[] picres = Common.MatchExpr(strResult, "img src=\"" + @"http://i.schoolido.lu/cards/\d*\w*\.png");
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
                            names[i] = filenames[i].Substring(Common.isLetter(filenames[i]),
                                filenames[i].LastIndexOf(".") - Common.isLetter(filenames[i]));
                        }
                        //Console.WriteLine(picres[i]);
                        //Console.WriteLine(filenames[i]);
                        //Console.WriteLine(names[i]);
                        Common.DownloadCards(picres[i], names[i], filenames[i]);
                    }
                }

                Console.WriteLine("\nAll Cards Download Complete!\n");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Access Denied");
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
        
    }
}
