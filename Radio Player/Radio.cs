using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radio_Player
{
    class Radio
    {
        public delegate void SetAddress(string Address, string urlStream, string[] sep);
        public event SetAddress NewAddress;

        public ConsoleMediaPlayer m_CMP;
        private RadioList m_list;
        private Thread m_Status;
        private int m_index = 0;
        public Radio()
        {
            m_CMP = new ConsoleMediaPlayer(9, 2);
            m_list = new RadioList("RadioList.xml");
            m_Status = new Thread(m_CMP.ShowStatus);
            m_CMP.URL = m_list.GetRadio(0).UrlStream;
            m_CMP.Show();
            m_Status.Start();
        }
        public void StartShow()
        {
            m_CMP.ShowAll = true;  
            m_CMP.Show();
            m_list.Updata();                   
        }

        public void Add(string url, string title = "station")
        {
            m_list.Add(url, title);
        }

        public void StopShow()
        {
            m_CMP.ShowAll = false; 
        }

        public bool Event(int x, int y, int click = 4)
        {
            m_CMP.Event(x, y, click);
            m_index = m_list.Event(x, y, click);
            if (m_index != -1)
            {               
                m_CMP.m_WMP.controls.stop();
                m_CMP.URL = m_list.GetRadio(m_index).UrlStream;
                NewAddress(m_list.GetRadio(m_index).WepPegeAddress, m_list.GetRadio(m_index).UrlStream, m_list.GetRadio(m_index).m_siporator);
                m_CMP.Show();
            }
            return false;
        }

    }
}
