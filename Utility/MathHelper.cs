namespace ParlayRMS.Utility
{
    using System.Collections.Generic;

    public static class MathHelper
    {
        public static List<List<T>> GetCombinations<T>(List<T> values)
        {
            var collections = new List<List<T>>();

            for (var counter = 0; counter < 1 << values.Count; ++counter)
            {
                var combination = new List<T>();

                for (var index = 0; index < values.Count; ++index)
                {
                    if ((counter & (1 << index)) == 0)
                    {
                        combination.Add(values[index]);
                    }
                }

                collections.Add(combination);
            }

            return collections;
        }
    }
}
