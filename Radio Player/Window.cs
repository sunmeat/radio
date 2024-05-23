using System;
using System.Text;

namespace Radio_Player
{
    class Window
    {
        private int m_Top;
        private int m_Left;
        private int m_Width;
        private int m_Height;

        public int Top
        {
            get { return m_Top; }
            set { m_Top = value; }
        }
        public int Left
        {
            get { return m_Left; }
            set { m_Left = value; }
        }
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        public Window()
        {
            m_Top = 0;
            m_Left = 0;
            m_Width = 2;
            m_Height = 2;
        }
        public Window(int top, int left, int width, int height)
        {
            m_Top = top;
            m_Left = left;
            m_Width = width;
            m_Height = height;
        }

        public bool WindowShow()
        {
            GlobalMutex.GetMutex.WaitOne();
            for (int i = 0; i < m_Height; i++)
		    {          
                Console.SetCursorPosition(m_Left, m_Top + i);
                for (int j = 0; j < m_Width; j++)
			    {
				    if (i == 0 && j == 0)
                        Console.Write((char)9484);
                    else if (i == 0 && j == m_Width - 1)
                        Console.Write((char)9488); 
                    else if (i == (m_Height - 1) && j == 0)
                        Console.Write((char)9492); 
                    else if (i == (m_Height - 1) && j == (m_Width - 1))
                        Console.Write((char)9496);
                    else if (i == 0 || i == (m_Height - 1))
                        Console.Write((char)9472); 
                    else if (j == 0 || j == (m_Width - 1))
                        Console.Write((char)9474);
				    else
					    Console.Write(' ');
				
			    }
			    Console.WriteLine();
		    }
            GlobalMutex.GetMutex.ReleaseMutex();
		    return true;
        }
    }
}
