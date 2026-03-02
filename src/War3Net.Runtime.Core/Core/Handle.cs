namespace War3Net.Runtime.Core
{
    public abstract class Handle
    {
        public int GetHandleId()
        {
            return GetHashCode();
        }
    }
}