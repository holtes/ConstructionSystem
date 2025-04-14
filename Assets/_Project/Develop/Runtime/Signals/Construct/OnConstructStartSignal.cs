using _Project.Develop.Runtime.Interfaces;

namespace _Project.Develop.Runtime.Signals
{
    public class OnConstructStartSignal
    {
        public IContructable ContructableItem;

        public OnConstructStartSignal(IContructable contructableItem) => ContructableItem = contructableItem;
    }
}

