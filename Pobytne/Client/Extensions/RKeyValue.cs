namespace Pobytne.Client.Extensions
{
	internal record RKeyValue // dataType for KeyValued components (Radio,Select,Autosuggest)
	{
		public required string Name { get; set; }
		public int? NId { get; set; }
        public int Id { get => NId ?? -1; set { NId = value; } }
	}
}
