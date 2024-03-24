namespace Pobytne.Shared.Struct
{
    public class LazyList(int startIndex = 0, int count = int.MaxValue, string subfix = "", bool active = false)
    {
        public int Count { get; set; } = count;
        public int StartIndex { get; set; } = startIndex;
        public string Subfix { get; set; } = subfix;
        public bool Active { get; set; } = active;
    }
}
