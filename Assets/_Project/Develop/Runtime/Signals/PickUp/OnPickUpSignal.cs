using _Project.Develop.Runtime.Interfaces;

namespace _Project.Develop.Runtime.Signals
{
    public class OnPickUpSignal
    {
        public IPickable PickableItem;

        public OnPickUpSignal(IPickable pickableItem) => PickableItem = pickableItem;
    }
}
