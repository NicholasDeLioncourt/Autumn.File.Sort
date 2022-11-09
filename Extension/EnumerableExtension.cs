namespace Autumn.File
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autumn.File.Properties;
    //=============================================================================================
    /// <summary>IEnumerable extension methods.</summary>
    /// <created>L. Nicholas de Lioncourt</created>
    /// <copyright file="EnumerableExtension.cs" company="De Lioncourt LLC">
    /// L. Nicholas de Lioncourt. Open usage is fine as long as my name remains herein.
    /// </copyright>
    //=============================================================================================
    public static class EnumerableExtension
    {
        #region ITERATING OVER THE IENUMERABLE
        //=========================================================================================
        /// <summary>
        /// An iteration over the source with the supplied <paramref name="action"/> invoked against 
        /// each instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The <see cref="Action{TType}"/>.</param>
        //=========================================================================================
        [System.Diagnostics.DebuggerStepThrough]
        public static void Each<TType>(this IEnumerable<TType> source, Action<TType> action)
        {
            if(null == source)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Source)); }
            if(null == action)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Action)); }
            foreach(TType __item in source) { action(__item); }
        }

        //=========================================================================================
        /// <summary>
        /// An indexed iteration over the source with the supplied <paramref name="action"/> invoked 
        /// against each instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The <see cref="Action{TSource,Index}"/>.</param>
        //=========================================================================================
        [System.Diagnostics.DebuggerStepThrough]
        public static void Each<TType>(this IEnumerable<TType> source, Action<TType, int> action)
        {
            if(null == source)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Source)); }
            if(null == action)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Action)); }

            IList<TType> __instance = source as IList<TType>;

            if(null == __instance) { int __current = 0; foreach(TType __item in source) { action(__item, __current++); } return; }

            int __limit = __instance.Count;
            for(int __index = 0; __index < __limit; __index++) { action(__instance[__index], __index); }
        }

        //=========================================================================================
        /// <summary>
        /// An iteration over the source with the supplied <paramref name="action"/> invoked against 
        /// each instance if the <paramref name="condition"/> is fulfilled (returns <c>true</c>).
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The <see cref="System.Action{TType}"/> delegate to perform on each 
        /// element; if the action is <c>null</c>, no action is taken.</param>
        /// <param name="condition">The <seealso cref="Predicate{T}"/> delegate which defines the 
        /// conditions to check against each item.</param>
        //=========================================================================================
        [System.Diagnostics.DebuggerStepThrough]
        public static void EachIf<TType>(this IEnumerable<TType> source, Action<TType> action, Predicate<TType> condition)
        {
            if(null == source)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Source)); }
            if(null == action)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Action)); }
            if(null == condition) { source.Each(action); return; }

            foreach(TType __item in source.Where(item => condition(item))) { action(__item); }
        }

        //=========================================================================================
        /// <summary>
        /// Returns the items starting and index 0 and up to the <paramref name="count"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="count">The total count of items to retrieve.</param>
        //=========================================================================================
        public static IEnumerable<TType> EachLimit<TType>(this IEnumerable<TType> source, int count)
        {
            if(null == source) { yield break; }

            TType [] __array = source.ToArray();
            int __limit	  = Math.Min(__array.Length, count) - 1;

            if(0 >= __limit) { yield break; }

            for(int __current = 0; __current < __limit; __current++) { yield return(__array[__current]); }
        }

        //=========================================================================================
        /// <summary>
        /// Iterates over the <paramref name="source"/> and determines whether each element is <c>null</c>; 
        /// if not <c>null</c>, performs the <paramref name="action"/> otherwise that element is 
        /// ignored.
        /// <paramref name="source"/> is not <c>null</c>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The action to perform if the element is not <c>null</c>.</param>
        //=========================================================================================
        public static void EachNotNull<TType>(this IEnumerable<TType> source, Action<TType> action) where TType : class
        {
            if(null == source)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Source)); }
            if(null == action)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Action)); }

            foreach(TType __item in source) { if(null == __item) { continue; } action(__item); }
        }
        #endregion

        #region FILTERING TO ONE ELEMENT OR MANY
        //=========================================================================================
        /// <summary>
        /// Returns the first item; or if not found, the result of the <paramref name="defaultvalue"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IEnumerable{TType}"/>.</param>
        /// <param name="defaultvalue">The default value if not found.</param>
        //=========================================================================================
        public static TType FirstOrDefault<TType>(this IEnumerable<TType> source, Func<TType> defaultvalue) where TType : class
        {
            if(null == source) return(defaultvalue());

            TType __item = source.FirstOrDefault();
            return(__item ?? (null == defaultvalue ? default(TType) : defaultvalue()));
        }

        //=========================================================================================
        /// <summary>
        /// Returns the first item matching the conditions of the <paramref name="predicate"/>; or 
        /// if not found, the result of the <paramref name="defaultvalue"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IEnumerable{TType}"/>.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="defaultvalue">The default value if not found.</param>
        //=========================================================================================
        public static TType FirstOrDefault<TType>(this IEnumerable<TType> source, Func<TType, bool> predicate, Func<TType> defaultvalue = null)
        {
            if(null == predicate) { return(source.FirstOrDefault()); }

            if(null == source)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Source)); }
            if(null == predicate)
              { throw new ArgumentException(Resources.ExceptionFirstOrDefaultNullParam.FormatCulture(StringsParamNames.Predicate)); }

            foreach(TType __item in source.Where(predicate)) { return(__item); }

            return(null == defaultvalue ? default(TType) : defaultvalue());
        }

        //=========================================================================================
        /// <summary>
        /// Returns the first item in the <paramref name="source"/> which matches the 
        /// <paramref name="condition"/>; if not found, returns the result of the 
        /// <paramref name="default"/> or default(TType) if default is <c>null</c>.
        /// </summary>
        /// <param name="source">The source <see cref="IEnumerable{T}"/>.</param>
        /// <param name="condition">
        /// The predicate; if <c>null</c>, throws <see cref="ArgumentException"/>.
        /// </param>
        /// <param name="default">
        /// The default value if not found; if <c>null</c>, throws <see cref="ArgumentException"/>.
        /// </param>
        //=========================================================================================
        public static TType FirstWhere<TType>(this IEnumerable<TType> source, Func<TType, bool> condition, Func<TType> @default = null)
        {
            if(null == condition)
              { throw new ArgumentException(Resources.ExceptionFirstWhereOrDefault.FormatCulture(StringsParamNames.Condition)); }

            if(null == source) { return(null == @default ? default(TType) : @default()); }
            foreach(TType __item in source.Where(condition)) { return(__item); }
            return(null == @default ? default(TType) : @default());
        }
        #endregion

        #region CONVERSIONS
        //=========================================================================================
        /// <summary>
        /// Converts each instance in the <see cref="IEnumerable{TType}"/> provided <paramref name="source"/> 
        /// to type <c>TResult</c> and returns all non-null conversions.
        /// </summary>
        /// <param name="source">The source <see cref="IEnumerable{TType}"/>.</param>
        //=========================================================================================
        public static IEnumerable<TResult> As<TType, TResult> (this IEnumerable<TType> source) where TType: class where TResult: class
        {
            if(null == source) { return(EmptyArray<TResult>.Instance); }

            List<TResult> __result = new List<TResult>(0x000010);
                          __result.AddRange(source.OfType<TResult>());

            return(__result);
        } 

        //=========================================================================================
        /// <summary>
        /// Creates a new <see cref="ICollection{TType}"/> which is populated with the elements in 
        /// the <paramref name="source"/>; if <paramref name="beforeadd"/> is not <c>null</c> the 
        /// current item will be submitted to that delegate before addition into the collection.
        /// </summary>
        /// <param name="source">The source <see cref="IEnumerable{TType}"/>.</param>
        /// <param name="beforeadd">The delegate performed on each item.</param>
        //=========================================================================================
        public static ICollection<TType> ToCollection<TType>(this IEnumerable<TType> source, Func<TType, TType> beforeadd = null)
        {
            if(null == source) { return(null); }

            ICollection<TType> __result = new System.Collections.ObjectModel.Collection<TType>();

            foreach(TType __item in source.Select(current => (null == beforeadd) ? current : beforeadd(current)))
                   { __result.Add(__item); }
            return(__result);
        }

        //=========================================================================================
        /// <summary>
        /// Converts the provided TType list to a delimited string by invoking ToString on each 
        /// instance. Any <c>null</c> reference will be treated as an empty string.
        /// </summary>
        /// <param name="source">The <see cref="IEnumerable{TType}"/> of strings.</param>
        /// <param name="delimiter">The delimiter; if not provided, defaults to a comma.</param>
        //=========================================================================================
        public static string ToDelimitedString<TType>(this IEnumerable<TType> source, char delimiter = Chars.CharComma)
        {
            // ReSharper disable ForCanBeConvertedToForeach
            if(null == source) { return(String.Empty); }

            TType [] __list = source.ToArray();

            System.Text.StringBuilder __builder = new System.Text.StringBuilder(__list.Length * 0x000020);

            for(int __index = 0; __index < __list.Length; __index++)
            {
                if(0 < __builder.Length) { __builder.Append(delimiter); }
                __builder.Append(ReferenceEquals(__list[__index], null) ? String.Empty : __list[__index].ToString());
            }

            return(__builder.ToString());
            // ReSharper restore ForCanBeConvertedToForeach
        }

        //=========================================================================================
        /// <summary>
        /// Creates a new <see cref="Queue{TType}"/> which is populated with the elements in the 
        /// <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IEnumerable{TType}"/>.</param>
        //=========================================================================================
        public static Queue<TType> ToQueue<TType>(this IEnumerable<TType> source)
        {
            if(null == source) { return(null); }

            Queue<TType> __result = new Queue<TType>(source);
            return(__result);
        }

        //=========================================================================================
        /// <summary>
        /// Creates a new <see cref="Stack{TType}"/> which is populated with the elements in the 
        /// <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source <see cref="IEnumerable{TType}"/>.</param>
        //=========================================================================================
        public static Stack<TType> ToStack<TType>(this IEnumerable<TType> source)
        {
            if(null == source) { return(null); }

            Stack<TType> __result = new Stack<TType>(source);
            return(__result);
        }
        #endregion

    }
}
