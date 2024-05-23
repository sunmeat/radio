using System;
using System.IO;

namespace Radio_Player
{
    class RadioMusic
    {
        private FileInfo[] m_MusicList;
        private int m_counter;

        public RadioMusic()
        {
            m_counter = 0;
        }

        public void CreateList()
        {
            Console.SetCursorPosition(0, 2);
            DirectoryInfo root = new DirectoryInfo(@"Music\");
            m_MusicList = root.GetFiles("*.mp3");
            int i = 1;
            foreach (FileInfo name in m_MusicList)
            {
                Console.WriteLine("{0}.{1}", i++, name.Name);
            }
        }
    }
}
