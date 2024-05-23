using System;
using WMPLib;
using System.Threading;

namespace Radio_Player
{
    class ConsoleMediaPlayer: Window
    {
        private bool m_ShowAll;
        private string m_URL;
        private string m_song;
        private int m_Volume;
        public WindowsMediaPlayer m_WMP;
        private Button m_PlayOrStop;
        private Slider m_Slider;      
        private Thread m_Ticker;

        public string URL
        {
            get { return m_URL; }
            set { m_URL = value;
            m_WMP.URL = m_URL;
            }
        }
        public bool ShowAll
        {
            get { return m_ShowAll; }
            set { m_ShowAll = value; }
        }

        public ConsoleMediaPlayer(int left, int top)
        {
            Left = left;
            Top = top;
            Width = 62;
            Height = 6;
            m_song = "";
            m_ShowAll = true;

            m_WMP = new WindowsMediaPlayer();
            m_WMP.URL = @"http://online-kissfm.tavrmedia.ua/KissFM";
            m_WMP.settings.autoStart = true;

            m_PlayOrStop = new Button(Left + 2, Top + 1, (char)9608 + "");
            m_Slider = new Slider(Left + 16, Top + 4, 40, 30);
           
            m_Ticker = new Thread(Ticker);
            m_Ticker.Start();           
        }

        public void Show()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            WindowShow();
            m_PlayOrStop.Show();
            m_Slider.Show();
            ShowVolue();
            ShowRadioStation();
            ShowMusic(m_song);        
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ShowVolue()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(Left + 12, Top + 4);
            Console.Write("    ");
            Console.SetCursorPosition(Left + 1, Top + 4);
            m_Volume = Convert.ToInt32((double)(m_Slider.Position - 1)  / (m_Slider.Length - 1) * 100);
            Console.Write("Громкость : {0}", m_Volume);
        }
        private void ShowRadioStation()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(Left + 8, Top + 1);
            Console.Write("Радио станция : {0}", m_WMP.currentMedia.name);
        }
        public void ShowStatus()
        {
            while (true)
            {
                Thread.Sleep(500);
                if (ShowAll)
                {                  
                    GlobalMutex.GetMutex.WaitOne();
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.SetCursorPosition(Left + 8, Top + 3);
                        Console.Write("                                                 ");
                        Console.SetCursorPosition(Left + 8, Top + 3);
                        Console.Write(m_WMP.status);
                        if (m_WMP.status == "Буферизация")
                        {
                            m_WMP.controls.stop();
                            Thread.Sleep(500);
                            m_WMP.controls.play();
                        }
                    }
                    catch (Exception)
                    {
                        Console.SetCursorPosition(Left + 8, Top + 3);
                        Console.Write("Error");
                    }
                    GlobalMutex.GetMutex.ReleaseMutex();
                }
            }
        }

        public void ShowMusic(string music)
        {
                GlobalMutex.GetMutex.WaitOne();
                Thread.Sleep(100);
                if (ShowAll)
                {
                    Console.SetCursorPosition(Left + 8, Top + 2);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Музыка : ");
                }
                GlobalMutex.GetMutex.ReleaseMutex();
                m_song = music;
        }

        public void Ticker()
        {
            for (int i = 0; ; i++)
            {
                if (i > m_song.Length - 43)
                    {
                        i = 0;
                        Thread.Sleep(1000);
                    }
                if (m_ShowAll)
                {
                    
                    GlobalMutex.GetMutex.WaitOne();
                    Console.SetCursorPosition(Left + 17, Top + 2);
                    Console.Write("                                           ");
                    Console.SetCursorPosition(Left + 17, Top + 2);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (m_song.Length > 42 && m_ShowAll)
                        Console.WriteLine(m_song.Substring(i, 43));
                    else if (m_ShowAll)
                        Console.WriteLine(m_song);
                    GlobalMutex.GetMutex.ReleaseMutex();
                    if (i == 0)
                        Thread.Sleep(2500);
                    else
                        Thread.Sleep(500);
                }
            }
        }

        public void Event(int x, int y, int click = 4)
        {
            GlobalMutex.GetMutex.WaitOne();
            if (m_Slider.Event(x, y, click))
            {
                ShowVolue();
                m_WMP.settings.volume = m_Volume;
            }
            if (m_PlayOrStop.Event(x, y, click))
            {
                if (m_PlayOrStop.Caption == (char)9608 + "")
                {
                    m_PlayOrStop.Caption = (char)9658 + "";
                    m_WMP.controls.stop();
                   // m_WMP.controls.pause();
                }
                else
                {
                    m_PlayOrStop.Caption = (char)9608 + "";
                    m_WMP.controls.play();
                }
            }
            GlobalMutex.GetMutex.ReleaseMutex();
        }

    }
}
