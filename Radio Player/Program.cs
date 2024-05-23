using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;

namespace Radio_Player
{
    class Program
    {
        //[DllImport(@"MouseInConsole.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]

        //static extern int ReCord(ref int X, ref int Y, StringBuilder str, ref int len);
        public delegate bool MethodContainer(int x, int y, int click = 4);
        static event MethodContainer MouseEvent;
        static void Main(string[] args)
        {
            //Console.SetWindowSize(80, 50);
            GlobalMutex mutex = GlobalMutex.CreateMutex();
            Console.CursorVisible = false;

            Tab RadioTab = new Tab("Радио", 0);
            Tab TextTab = new Tab("Текст", 8);
            Tab ListTab = new Tab("Скачанные песни", 16);
            MouseEvent += RadioTab.Event;
            MouseEvent += TextTab.Event;
            MouseEvent += ListTab.Event;

            Tab Download = new Tab("Скачать", 71);
            MouseEvent += Download.Event; ;

            Tab DAD = new Tab("DragAndDrop", 40);

            RadioMusic music = new RadioMusic();
            TextList textlist = new TextList();
            Radio radio = new Radio();
            textlist.m_Song.NewSong += radio.m_CMP.ShowMusic;
            radio.NewAddress += textlist.m_Song.SetAddress;
            textlist.m_Song.downloadTab = Download;

           
            int status = -1;
            int X = 0;
            int Y = 0;
            int click = 4;
            int len = 0;
            StringBuilder sb = new StringBuilder(4092);
            bool firstRun = true;
 
            while (true)
            {
                MouseEvent(X, Y, click);
                GlobalMutex.GetMutex.WaitOne();
                if ((RadioTab.Status == 2 && status != 0) || firstRun)
                {
                    if (firstRun)
                        RadioTab.Status = 2;

                    textlist.StopShow();
                    Console.Clear();

                    status = 0;
                    TextTab.Status = 0;
                    ListTab.Status = 0;

                    radio.StartShow();
                    MouseEvent += radio.Event;
                    MouseEvent += DAD.Event;
                    MouseEvent -= textlist.Event;
                    

                    TextTab.Update((ModeButton)TextTab.Status);
                    RadioTab.Update((ModeButton)RadioTab.Status);
                    ListTab.Update((ModeButton)ListTab.Status);
                    Download.Update((ModeButton)Download.Status);
                    DAD.Update((ModeButton)DAD.Status);

                    if (firstRun)
                    {
                        firstRun = false;
                        MouseEvent(11, 11, 1);
                    }

                }
                else if (TextTab.Status == 2 && status != 1)
                {
                    radio.StopShow();
                    Console.Clear();

                    status = 1;
                    RadioTab.Status = 0;
                    ListTab.Status = 0;

                    textlist.Updata();
                    MouseEvent += textlist.Event;
                    MouseEvent -= radio.Event;
                    MouseEvent -= DAD.Event;

                    TextTab.Update((ModeButton)TextTab.Status);
                    RadioTab.Update((ModeButton)RadioTab.Status);
                    ListTab.Update((ModeButton)ListTab.Status);
                    Download.Update((ModeButton)Download.Status);
                    DAD.Update((ModeButton)(DAD.Status = 1));
                }
                else if (ListTab.Status == 2 && status != 2)
                {
                    radio.StopShow();
                    textlist.StopShow();
                    Console.Clear();

                    status = 2;
                    TextTab.Status = 0;
                    RadioTab.Status = 0;

                    music.CreateList();

                    MouseEvent -= radio.Event;
                    MouseEvent -= textlist.Event;
                    MouseEvent -= DAD.Event;

                    TextTab.Update((ModeButton)TextTab.Status);
                    RadioTab.Update((ModeButton)RadioTab.Status);
                    ListTab.Update((ModeButton)ListTab.Status);
                    Download.Update((ModeButton)Download.Status);
                    DAD.Update((ModeButton)(DAD.Status = 1));
                }
                if (Download.Status == 2)
                {
                    if (textlist.m_Song.m_counter > 0)
                        textlist.m_Song.DownloadSong();
                    else
                    {
                        Download.Status = 0;
                        Download.Update(0);
                    }
                }
                if (DAD.Status == 2)
                {
                    string newUrl = DragAndDrop();
                    if (String.Compare(newUrl, "null") == 0)
                    {
                        DAD.Status = 0;
                        DAD.Update(0);
                    }
                    else
                    {
                        radio.Add(newUrl);
                    }
                    DAD.Status = 0;
                    DAD.Update(0);
                }
                GlobalMutex.GetMutex.ReleaseMutex();
                //click = ReCord(ref X, ref Y, sb, ref len);
              //  Console.Write("Size: {0}, str: {1}",sb.Length, sb.ToString());                           
            }
        }

        public static string FileRead(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] readBytes = new byte[fs.Length];
            fs.Read(readBytes, 0, readBytes.Length);
            string text = Encoding.Default.GetString(readBytes);
            fs.Close();
            return text;
        }

        public static string DragAndDrop()
        {
            string path = "";
            Regex regex = new Regex(@"^([A-Z]:(\\(\W|\w)*)*\.m3u)$");
            while (true)
            {
                char ch = Console.ReadKey(true).KeyChar;
                if(ch == (char)ConsoleKey.Escape)
                    return "null";
                path += ch;
                if (!Console.KeyAvailable)
                {
                    Match match = regex.Match(path);
                    if (match.Success)
                    {
                        path = match.Value;
                        break;
                    }
                    path = "";
                }
            }
            string text = FileRead(path);
            Regex regexURL = new Regex(@"http://.+");
            Match mat = regexURL.Match(text);
            if (mat.Success)
                return mat.Value;
            return "null";
        }

     
    }
}
