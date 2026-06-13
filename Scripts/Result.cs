using Bakery.Core;

namespace Bakery.Quests
{
    public abstract class Result : ContentTag
    {
        public abstract void Execute();
    }
}