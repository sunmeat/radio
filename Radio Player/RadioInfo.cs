using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radio_Player
{
    class RadioInfo : Window
    {
        private string m_title;
        private string m_urlStream;
        private string m_wepPageAddress;
        private int m_status = 0; // 0 - normal, 1 - move, 2 - click
        public string[] m_siporator;
        private Thread t;
        private Random r;

        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public string UrlStream
        {
            get { return m_urlStream; }
            set { m_urlStream = value; }
        }

        public string WepPegeAddress
        {
            get { return m_wepPageAddress; }
            set { m_wepPageAddress = value; }
        }

        public int Status
        {
            get { return m_status; }
            set { m_status = value; Show((ModeButton)value); }
        }

        public RadioInfo()
        {
            m_title = "title";
            Left = 0;
            Top = 0;
            Width = 16;
            Height = 3;
        }
        public RadioInfo(int left, int top)
        {
            m_title = "title";
            Left = left;
            Top = top;
            Width = 20;
            Height = 3;
            t = new Thread(Raduga);
            r = new Random();
          //  t.Start();
        }

        public void Show(ModeButton Mode = ModeButton.Normal)
        {
            GlobalMutex.GetMutex.WaitOne();
            if (Mode == ModeButton.Normal)
            {
                Console.ForegroundColor = ConsoleColor.White;
                m_status = 0;
            }
            else if (Mode == ModeButton.Move)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                m_status = 1;
            }
            else if (Mode == ModeButton.Click)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                m_status = 2;
            }
            WindowShow();
            if (Mode == ModeButton.Move)
            {
                Console.SetCursorPosition(Left + Width - 1, Top);
                Console.Write('x');
            }
            int space = (20 - m_title.Length) / 2;
            Console.SetCursorPosition(Left + 1 + space, Top + 1);
            Console.WriteLine(m_title);

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            GlobalMutex.GetMutex.ReleaseMutex();
        }
        public void Raduga()
        {
            Thread.Sleep(500);
            for (int i = r.Next(100); ; i++)
            { 
                if (i > 10000)
                    i = 1;
                if (m_status != 0)
                    continue;            
                Thread.Sleep(250);
                GlobalMutex.GetMutex.WaitOne();
                Console.ForegroundColor = (ConsoleColor)(i % 15 + 1);
                WindowShow();
                int space = (20 - m_title.Length) / 2;
                Console.SetCursorPosition(Left + 1 + space, Top + 1);
                Console.WriteLine(m_title);

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                GlobalMutex.GetMutex.ReleaseMutex();
            }
        }
        public bool Event(int x, int y, int click = 4)
        {
            if (x >= Left && x <= Left + Width && y >= Top && y < Top + Height)
            {
                if (click == 1 && m_status != 2)
                {
                    Show(ModeButton.Click);
                    return true;
                }
                else if (m_status == 0)
                {
                    Show(ModeButton.Move);
                }
            }
            else if (m_status == 1)
            {
                
                Show(ModeButton.Normal);
            }
            return false;
        }
    }

}
