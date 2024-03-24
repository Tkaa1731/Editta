namespace Pobytne.Shared.Struct
{
    public class LazyList(Type type, int startIndex = 0, int count = int.MaxValue, string subfix = "", bool active = false)
    {
        public readonly Type Type = type;
        public int Count { get; set; } = count;
        public int StartIndex { get; set; } = startIndex;
        public string Subfix { get; set; } = subfix;
        public bool Active { get; set; } = active;
    }
}
