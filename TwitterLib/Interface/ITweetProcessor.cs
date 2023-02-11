using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterLib.Interface
{
    public interface ITweetProcessor
    {
        void ProcessTweet(string tweet);
    }
}
