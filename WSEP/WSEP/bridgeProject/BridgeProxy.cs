using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.bridgeProject
{
    class BridgeProxy : BridgeProject
    {

        private BridgeProject real;

        public BridgeProxy() {
		    this.real=null;
	    }

	    public void setRealBridge(BridgeProject real) {
		    this.real = real;
	    }
    }
}
