namespace OverpassNet.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool TryGetValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys, out IEnumerable<TValue> values)
        {
            ArgumentNullException.ThrowIfNull(dictionary);

            var result = new List<TValue>();

            foreach (var key in keys)
            {
                if (!dictionary.TryGetValue(key, out TValue? value) || value is null)
                {
                    values = [];
                    return false;
                }
                result.Add(value);
            }
            values = result;
            return true;
        }
    }
}
