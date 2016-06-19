using Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class CommObserverable : IObserverable
    {
        private Socket _s;

        public CommObserverable(Socket s)
        {
            _s = s;
        }

        public void send(string msg)
        {
            _s.Send(getBytes(msg));
        }

        private byte[] getBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
