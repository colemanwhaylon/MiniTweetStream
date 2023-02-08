using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterLib.Interface
{
    public interface IDataSink
    {
        void RecieveTweet(string tweet);
    }
}
