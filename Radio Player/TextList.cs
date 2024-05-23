using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Enums;

namespace Radio_Player
{
    class TextList
    {
        private Button m_Next;
        private Button m_Back;
        public Song m_Song;      

        public TextList()
        {
            m_Song = new Song(@"http://o.tavrmedia.ua:9561/get/?k=kiss&callback=?");
            m_Back = new Button(0, 3, "Back");
            m_Next = new Button(72, 3, "Next");
        }

        public void Updata()
        {
            m_Back.Show();
            m_Next.Show();
            m_Song.m_activ = true;
            m_Song.Print();
        }

        public void StopShow()
        {
            m_Song.m_activ = false;
        }

        public bool Event(int x, int y, int click = 4)
        {
            if (m_Back.Event(x, y, click) && m_Song.Number > 0)
            {
                m_Song.Number--;
                m_Song.Print();
            }
            else if (m_Next.Event(x, y, click) && m_Song.Number < m_Song.m_counter - 1)
            {
                m_Song.Number++;
                m_Song.Print();
            }
               
            return true;
        }

    }
}
