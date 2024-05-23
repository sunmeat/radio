using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio_Player
{
    class Slider
    {
        private int m_Left;
        private int m_Top;       
        private int m_Length;
        private int m_Position;
        private bool m_Mute;
        private bool m_pressed = false;

        public int Left
        {
            get { return m_Left; }
            set { m_Left = value; }
        }
        public int Top 
        {
            get { return m_Top; }
            set { m_Top = value; }
        }
        public int Length
        {
            get { return m_Length; }
            set { m_Length = value; }
        }
        public int Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Slider(int left, int top)
        {
            m_Left = left;
            m_Top = top;
            m_Length = 10;
            m_Position = 10;
        }
        public Slider(int left, int top, int Length, int Position)
        {
            m_Left = left;
            m_Top = top;
            m_Length = Length;
            m_Position = Position;
        }

        public void Show()
        {
            Console.SetCursorPosition(m_Left, m_Top);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write((char)9500);
            for (int i = 0; i < m_Length; i++)
            {
                if(i != m_Position - 1)
                    Console.Write((char)9472);
                else
                    Console.Write((char)9608);
            }
            Console.Write((char)9508);
            ShowMute(true);
        }
        private void ShowMute(bool mode)
        {
            Console.SetCursorPosition(m_Left + m_Length + 2, m_Top);
            string muteOff = (char)9608 + "" + (char)9668 + "";
            string muteOn = "X" + (char)9668 + "";
            if (mode)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(muteOn);
            }
            else
                Console.WriteLine(muteOn);        
        }
        
        public bool Event(int x, int y, int click)
        {
            if (click == 3)
            {
                m_pressed = false;
            }
            if (x > m_Left && x <= m_Left + m_Length && y == m_Top)
            {
                if (click == 1 || m_pressed)
                {
                    m_pressed = true;
                    Console.MoveBufferArea(m_Left + m_Position, m_Top, 1, 1, x, m_Top, (char)9472, ConsoleColor.Blue, ConsoleColor.Black);
                    m_Position = x - m_Left;
                    return true;
                }
            }
            return false;
        }
    }
}
