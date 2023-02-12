using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterLib.Interface
{
    public interface ISubject
    {
        void RegisterObserver(IObserver observer);
        void UnRegisterObserver(IObserver observer);
        void NotifyObservers(string newTweet);
    }
}
