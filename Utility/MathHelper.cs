// --------------------------------------------------------------------------
// <copyright file="MathHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.1.24">Created</change>
// </history>

namespace ParlayRMS.Utility
{
    using System.Collections.Generic;

    /// <summary>
    /// Math helper extensions.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Gets all possible combinations of a list of values.
        /// </summary>
        /// <typeparam name="T">The type of data.</typeparam>
        /// <param name="values">The original values.</param>
        /// <returns>List of combinations.</returns>
        public static List<List<T>> GetCombinations<T>(List<T> values)
        {
            var collections = new List<List<T>>();

            for (int counter = 0; counter < (1 << values.Count); ++counter)
            {
                var combination = new List<T>();

                for (int index = 0; index < values.Count; ++index)
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