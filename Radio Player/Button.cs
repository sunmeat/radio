using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radio_Player
{
    class Button : Window
    {
        private string m_Caption;
        private bool m_callNewEvent = false; // m_callNewEvent == true - press click
        private int m_status = 0; // 0 - normal, 1 - move, 2 - click
        public string Caption
        {
            get { return m_Caption; }
            set { m_Caption = value; }
        }

        public Button()
        {
            m_Caption = "Caption";
            Left = 0;
            Top = 0;
            Width = m_Caption.Length + 4;
            Height = 3;
        }
        public Button(int left, int top, string caption)
        {
            m_Caption = caption;
            Left = left;
            Top = top;
            Width = m_Caption.Length + 4;
            Height = 3;          
        }

        public void Show(ModeButton Mode = ModeButton.Normal)
        {
            GlobalMutex.GetMutex.WaitOne();
            if (Mode == ModeButton.Normal)
                Console.ForegroundColor = ConsoleColor.White;
            else if (Mode == ModeButton.Move)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (Mode == ModeButton.Click)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            WindowShow();
            Console.SetCursorPosition(Left + 2, Top + 1);
            Console.WriteLine(m_Caption);

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            GlobalMutex.GetMutex.ReleaseMutex();
        }
        public bool Event(int x, int y, int click = 4)
        {
            if (click == 3 && m_callNewEvent)
            {
                m_callNewEvent = false;
                return true;
            }
            if (x >= Left && x <= Left + Width && y >= Top && y < Top + Height)
            {
                if (click == 1)
                {
                    Show(ModeButton.Click);
                    m_callNewEvent = true;
                    m_status = 2;
                }
                else if (click == 4 && m_status != 1 && !m_callNewEvent)
                {
                    Show(ModeButton.Move);
                    m_status = 1;
                }

            }
            else if (m_status > 0 && !m_callNewEvent)
            {
                Show(ModeButton.Normal);
                m_status = 0;
            }
            return false;
        }
    }

    enum ModeButton
    {
        Normal,
        Move,
        Click,
    }
}
