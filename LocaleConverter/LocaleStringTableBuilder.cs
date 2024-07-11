namespace LocaleConverter
{
    public class LocaleStringTableBuilder
    {
        private Dictionary<string, List<LocaleString>> Entities { get; } = [];

        public List<LocaleString> this[string code]
        {
            get
            {
                if (Entities.TryGetValue(code, out var value))
                    return value;

                value = [];
                Entities.Add(code, value);
                return value;
            }
        }

        public IEnumerable<KeyValuePair<string, LocaleString[]>> All => Entities.Select(t => new KeyValuePair<string, LocaleString[]>(t.Key, [.. t.Value]));
    }
}
