using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio_Player
{
    class Tab
    {
        private string m_titles;
        private int m_positionX;
        private int m_status = 0;

        public Tab()
        {
            m_titles = "example";
            m_positionX = 0;
            Update();
        }
        public Tab(string title, int posX = 0)
        {
            m_titles = title;
            m_positionX = posX;
            Update();
        }

        public int Status
        {
            get { return m_status; }
            set { m_status = value; Update(ModeButton.Normal); }
        }

        public void Update(ModeButton mode = ModeButton.Normal)
        {
            GlobalMutex.GetMutex.WaitOne();
            Console.ForegroundColor = ConsoleColor.Black;         
            if (mode == ModeButton.Normal)
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            else if (mode == ModeButton.Move)
                Console.BackgroundColor = ConsoleColor.DarkGray;
            else
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(m_positionX, 0);
            Console.Write(" " + m_titles + " ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.BackgroundColor = ConsoleColor.Black;
            GlobalMutex.GetMutex.ReleaseMutex();
        }

        public bool Event(int x, int y, int click = 4)
        {
            if (x >= m_positionX && x <= m_positionX + m_titles.Length + 1 && y < 1)
            {
                if (click == 1 && m_status != 2)
                {
                    Update(ModeButton.Click);
                    m_status = 2;
                    return true;
                }
                else if (m_status == 0)
                {
                    Update(ModeButton.Move);
                    m_status = 1;
                }
            }
            else if (m_status == 1)
            {
                Update(ModeButton.Normal);
                m_status = 0;
            }
            return false;
        }
    }
}
