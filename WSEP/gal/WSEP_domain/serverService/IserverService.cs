using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Script.Serialization;
using System.Threading;
using System.Collections;
using System.IO;
using System.Net.NetworkInformation;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.userManagement;
using System.Runtime.CompilerServices;
using Observer;

namespace serverService
{
    public interface IserverService
    {


        string parseMessage(string str, IObserverable co);

         Tuple<string, string, List<Tuple<string, string>>> parseMessage2(string str, IObserverable co);
        
    }
}
