using System;

namespace SOLocalisation
{
    public enum Language
    {
        EnUs,
        RuRu,
    }

    public static class LanguageUtil
    {
        /// <summary>
        /// Get language string value based on its index
        /// </summary>
        /// <param name="index">index of the language enum</param>
        /// <returns>language string</returns>
        public static string StringAt(int index)
        {
            if (Enum.IsDefined(typeof(Language), index))
                return ((Language)index).ToString();

            return "Invalid Value";
        }

        /// <summary>
        /// Get language string value based on its index
        /// </summary>
        /// <param name="index">index of the language enum</param>
        /// <returns>language enum</returns>
        public static Language LanguageAt(int index)
        {
            if (Enum.IsDefined(typeof(Language), index))
                return (Language)index;

            return default;
        }
    }
    
}