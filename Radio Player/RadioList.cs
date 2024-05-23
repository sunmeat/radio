using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

namespace Radio_Player
{
    class RadioList
    {
        private string m_Path;
        private List<RadioInfo> m_Radio;        
        private int m_Counter = 0;
        public string Path
        {
            get { return m_Path; }
            set { m_Path = value; }
        }

        public RadioList(string path)
        {
            m_Path = path;
            FileRead();
        }

        public void Add(string url, string title)
        {
            RadioInfo temp = new RadioInfo(9 + 21 * (m_Counter % 3), 10 + 3 * (m_Counter / 3));
            temp.UrlStream = url;
            temp.WepPegeAddress = "Bass";
            int lastSymbol = url.LastIndexOf('/');
            temp.Title = url.Substring(lastSymbol+1);
            m_Radio.Add(temp);
            beautifulShow(m_Radio[m_Counter]);
            FileWrite();
            m_Counter++;
        }
        private void FileWrite()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"RadioList.xml");

            XmlNode element = doc.CreateElement("Radio");
            doc.DocumentElement.AppendChild(element);

            XmlNode subElement1 = doc.CreateElement("title");
            subElement1.InnerText = m_Radio[m_Counter].Title;
            element.AppendChild(subElement1);

            XmlNode subElement2 = doc.CreateElement("urlStream");
            subElement2.InnerText = m_Radio[m_Counter].UrlStream;
            element.AppendChild(subElement2);

            XmlNode subElement3 = doc.CreateElement("wepPageAddress");
            subElement3.InnerText = m_Radio[m_Counter].WepPegeAddress;
            element.AppendChild(subElement3);

            doc.Save("RadioList.xml");

            //XmlNode node = doc.SelectNodes("RadioStation")[0].L;
           // Console.WriteLine(doc.SelectNodes("RadioStation").Count);
        }

        public RadioInfo GetRadio(int index)
        {
            return m_Radio[index];
        }

        private void FileRead()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"RadioList.xml");
            m_Radio = new List<RadioInfo>();
            int index = 0;
            foreach (XmlNode node in doc.SelectNodes("RadioStation/Radio"))
            {
                m_Counter++;
                RadioInfo temp_Radio = new RadioInfo(9 + 21 * (index % 3), 10 + 3 * (index / 3));
                ArrayList temp = new ArrayList();
                foreach (XmlNode child in node.ChildNodes)
                {                
                    if (String.Compare(child.Name, "title") == 0)
                        temp_Radio.Title = child.InnerText;
                    else if (String.Compare(child.Name, "urlStream") == 0)
                    {
                        temp_Radio.UrlStream = child.InnerText;
                        temp_Radio.WepPegeAddress = child.InnerText;
                    }
                    else if (String.Compare(child.Name, "wepPageAddress") == 0)
                        temp_Radio.WepPegeAddress = child.InnerText;
                    else if (String.Compare(child.Name, "SearchTegs") == 0)
                    {
                        foreach (XmlNode teg in child.ChildNodes)
                            temp.Add(teg.InnerText);
                    }                      
                }
                temp_Radio.m_siporator = new string[temp.Count];
                for (int i = 0; i < temp.Count; i++)
                    temp_Radio.m_siporator[i] = (string)temp[i];
                m_Radio.Add(temp_Radio);
                index++;
            }
        }

        private void beautifulShow(RadioInfo ri)
        {
            int left = ri.Left;
            int top = ri.Top;
            ri.Left = 30;
            ri.Top = 45;
            ri.Show();
            while (left != ri.Left)
            {
                GlobalMutex.GetMutex.WaitOne();
                if (left < ri.Left)
                    Console.MoveBufferArea(ri.Left, ri.Top, ri.Width, ri.Height, --ri.Left, ri.Top);   
                else
                    Console.MoveBufferArea(ri.Left, ri.Top, ri.Width, ri.Height, ++ri.Left, ri.Top);
                GlobalMutex.GetMutex.ReleaseMutex();
                Thread.Sleep(15);
            }
            while (top != ri.Top)
            {
                GlobalMutex.GetMutex.WaitOne();
                if (top < ri.Top)
                    Console.MoveBufferArea(ri.Left, ri.Top, ri.Width, ri.Height, ri.Left, --ri.Top);
                else
                    Console.MoveBufferArea(ri.Left, ri.Top, ri.Width, ri.Height, ri.Left, ++ri.Top);
                GlobalMutex.GetMutex.ReleaseMutex();
                Thread.Sleep(15);
            }
        }

        public void Updata()
        {
            for (int i = 0; i < m_Radio.Count; i++)
                m_Radio[i].Status = m_Radio[i].Status;
        }

        private void DeleteAndMove(int index)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(m_Radio[m_Radio.Count - 1].Left, m_Radio[m_Radio.Count-1].Top + i);
                Console.Write("                      ");
            }
            m_Radio.RemoveAt(index);   
            for (int i = 0; i < m_Radio.Count; i++)
            {
                m_Radio[i].Left = 9 + 21 * (i % 3);
                m_Radio[i].Top = 10 + 3 * (i / 3);
                m_Radio[i].Show((ModeButton)m_Radio[i].Status);
            }
        }

        public int Event(int x, int y, int click = 4)
        {
            for (int i = 0; i < m_Counter; i++)
            {
                if (m_Radio[i].Status == 1 && (x == (m_Radio[i].Left + m_Radio[i].Width - 1)) && (y == m_Radio[i].Top) && click == 1)
                {
                    Remove(i);                                                    
                    m_Counter--;                                                  
                    return -1;
                }
                if (m_Radio[i].Event(x, y, click))
                {
                    for (int j = 0; j < m_Counter; j++)
                        if (m_Radio[j].Status == 2 && j != i)
                            m_Radio[j].Status = 0;
                    return i;
                }
            }
            return -1;
        }

        private void Remove(int index)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"RadioList.xml");
            doc.SelectNodes("RadioStation")[0].RemoveChild(doc.SelectNodes("RadioStation/Radio")[index]);
            doc.Save("RadioList.xml");
            DeleteAndMove(index);   
        }
    }
}
