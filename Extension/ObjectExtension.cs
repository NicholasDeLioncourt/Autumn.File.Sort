namespace Autumn.File
{
    using System;

    //=============================================================================================
    /// <summary>System.Object extension methods.</summary>
    /// <created>LNDL</created>
    /// <copyright file="ObjectExtension.cs" company="De Lioncourt LLC">
    /// L. Nicholas de Lioncourt. All code herein is proprietary. It is OK to use 
    /// this as long as my name remains in this file.
    /// </copyright>
    //=============================================================================================
    public static class ObjectExtension
    {
        //=========================================================================================
        /// <summary>
        /// Returns the first value in the list of <paramref name="values"/> which is not <c>null</c>; 
        /// if all the <paramref name="values"/> are <c>null</c> or the list itself <c>null</c>, 
        /// returns default(TType).
        /// </summary>
        /// <param name="values">The values.</param>
        //=========================================================================================
        public static TType FirstNonNull<TType>(params TType[] values) where TType : class
        {
            if(null == values) { return(default(TType)); }
            // ReSharper disable LoopCanBeConvertedToQuery
            // ReSharper disable ForCanBeConvertedToForeach
            for(int __index = 0; __index < values.Length; __index++)
            {
                if(null == values[__index]) { continue; }
                return(values[__index]);
            }
            // ReSharper restore ForCanBeConvertedToForeach
            // ReSharper restore LoopCanBeConvertedToQuery
            return(default(TType));
        }

        //=========================================================================================
        /// <summary>
        /// Perform the provided <paramref name="action"/> if this value is  NOT <c>null</c>.
        /// </summary>
        //=========================================================================================
        public static void IfNotNull<TValue>(this TValue value, Action action) where TValue : class
        {
            if(null == value)  { return; }
            if(null == action) { return; }
            action();
        }

        //=========================================================================================
        /// <summary>
        /// Perform the provided <paramref name="action"/> if this value is <c>null</c>.
        /// </summary>
        //=========================================================================================
        public static void IfNull<TValue>(this TValue value, Action action) where TValue : class
        {
            if(null == value && null != action) { action(); }
        }

        //=========================================================================================
        /// <summary>
        /// Perform the provided <paramref name="action"/> if this value is <c>null</c>.
        /// </summary>
        //=========================================================================================
        public static void IfNull<TValue>(this TValue value, Func<TValue> action) where TValue : class
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if(null == value && null != action) { action(); }
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        //=========================================================================================
        /// <summary>
        /// Returns the <paramref name="replacement"/> if the provided <paramref name="source"/> 
        /// is (==) <c>null</c>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="replacement">The replacement if the source is <c>null</c>.</param>
        //=========================================================================================
        public static T IfNull<T>(this T source, T replacement) where T : class
        {
            return(source ?? replacement);
        }

        //=========================================================================================
        /// <summary>
        /// Returns a new, empty instance of type {TValue} if the <paramref name="value"/> is <c>null</c>.
        /// </summary>
        //=========================================================================================
        public static TValue NewIfNull<TValue>(this TValue value) where TValue : class, new()
        {
            return(value ?? new TValue());
        }

        //=========================================================================================
        /// <summary>
        /// Returns <c>true</c> of the provided <paramref name="source"/> is (==) <c>null</c>.
        /// </summary>
        /// <param name="source">The source.</param>
        //=========================================================================================
        public static bool IsNull<T>(this T source) where T : class
        {
            return(null == source);
        }
    }
}
