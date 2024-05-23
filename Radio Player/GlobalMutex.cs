using System;
using System.Threading;

namespace Radio_Player
{
    class GlobalMutex
    {
        private static Mutex m_Mutex;
        static GlobalMutex m_GlobalMutex;

        public static Mutex GetMutex
        {
            get { return m_Mutex; }
        }
        private GlobalMutex()
        {
            if (m_Mutex == null)
                m_Mutex = new Mutex();
        }

        public static GlobalMutex CreateMutex()
        {
            m_GlobalMutex = new GlobalMutex();
            return m_GlobalMutex;
        }
    }
}
