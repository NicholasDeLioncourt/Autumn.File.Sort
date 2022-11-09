namespace Autumn.File
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    //=============================================================================================
    /// <summary>XObject extension methods.</summary>
    /// <created>L. Nicholas de Lioncourt.</created>
    /// <copyright file="XObjectExtension.cs" company="De Lioncourt LLC">
    /// L. Nicholas de Lioncourt. All code herein is proprietary. It is OK to use 
    /// this as long as my name remains in this file. See Notes.txt
    /// </copyright>
    //=====================================================================================LNDL====
    public static class XObjectExtension
    {
        #region XML REMOVE NAMESPACES
        //=========================================================================================
        /// <summary>
        /// Removes namespace declarations from an XML document to simplify the usage of 
        /// <see cref="XName"/> references.
        /// </summary>
        /// <param name="source">The root <see cref="XElement"/>.</param>
        //=========================================================================================
        public static XElement RemoveNameSpaces(this XElement source)
        {
            if(null == source) { return(null); }
            XElement __result = new XElement(source.Name.LocalName, source.HasElements ?
                                             source.Elements().Select(RemoveNameSpaces) : (object)source.Value); 
                     __result.ReplaceAttributes(source.Attributes().Where(attribute => (!attribute.IsNamespaceDeclaration)));
            return(__result);
        } 
        #endregion

        #region FIND
        //=========================================================================================
        /// <summary>
        /// Recursively searches for an element by name; since the layout of the XML might change, 
        /// this is better than a formatted forward-only read.
        /// </summary>
        //=========================================================================================
        public static XElement FindElement(this XElement element, string name)
        {
            if(null == element) { return(null); }
            if(element.Name == name) { return(element); }

            if(!element.HasElements) { return(null); }

            foreach(XElement __current in element.Elements())
            {
                XElement __result = FindElement(__current, name);
                if(null == __result) { continue; } return(__result);
            }

            return(null);
        }
        #endregion

        #region AS BOOLEAN
        //===> XElement and XAttribute descend from XObject but do not share a common Value() method.
        //===> I have no intention of using reflection to avoid writing additional methods.
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to a <see cref="Boolean"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        //=========================================================================================
        public static bool ValueAsBool(this XElement element)
        {
            if(null == element) { return(false); }

            bool __result;
            return(bool.TryParse(element.Value, out __result) && __result);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to a <see cref="Boolean"/>; or false on failure.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        //=========================================================================================
        public static bool ValueAsBool(this XAttribute attribute)
        {
            if (null == attribute) { return(false); }

            bool __result;
            return(bool.TryParse(attribute.Value, out __result) && __result);
        }
        
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to a <see cref="Boolean"/>; or @default on failure.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The value to assign if the conversion fails.</param>
        //=========================================================================================
        public static bool ValueAsBool(this XAttribute attribute, bool @default)
        {
            if(null == attribute) { return(@default); }

            bool __result;
            return(bool.TryParse(attribute.Value, out __result) ? __result : @default);
        }
        #endregion

        #region AS BYTE
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to an <see cref="byte"/>; or (byte)0 or @default.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The optional default value, which is (byte)0.</param>
        //=========================================================================================
        public static byte ValueAsByte(this XAttribute attribute, byte @default = (byte)0)
        {
            if(null == attribute) { return(@default); }

            byte __result;
            return(byte.TryParse(attribute.Value, out __result) ? __result : @default);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to an <see cref="byte"/>; or (byte)0 or @default.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        /// <param name="default">The optional default value, which is (byte)0.</param>
        //=========================================================================================
        public static byte ValueAsByte(this XElement element, byte @default = (byte)0)
        {
            if(null == element) { return(@default); }

            byte __result;
            return(byte.TryParse(element.Value, out __result) ? __result : @default);
        }
        #endregion

        #region AS DATETIME NULLABLE
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to a nullable <see cref="DateTime"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        //=========================================================================================
        public static DateTime ? ValueAsDateTime(this XElement element)
        {
            if(null == element) { return(default(DateTime?)); }

            DateTime __result;
            return(DateTime.TryParse(element.Value, out __result) ? __result : default(DateTime?));
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to a nullable <see cref="DateTime"/>.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        //=========================================================================================
        public static DateTime ? ValueAsDateTime(this XAttribute attribute)
        {
            if(null == attribute) { return(default(DateTime?)); }

            DateTime __result;
            return(DateTime.TryParse(attribute.Value, out __result) ? __result : default(DateTime?));
        }
        #endregion

        #region AS DATETIMEOFFSET
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to a nullable <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        //=========================================================================================
        public static DateTimeOffset ? ValueAsDateTimeOffset(this XElement element)
        {
            if(null == element) { return(default(DateTimeOffset?)); }

            DateTimeOffset __result;
            return(DateTimeOffset.TryParse(element.Value, out __result) ? __result : default(DateTimeOffset?));
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to a nullable <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        //=========================================================================================
        public static DateTimeOffset? ValueAsDateTimeOffset(this XAttribute attribute)
        {
            if(null == attribute) { return (default(DateTimeOffset?)); }

            DateTimeOffset __result;
            return(DateTimeOffset.TryParse(attribute.Value, out __result) ? __result : default(DateTimeOffset?));
        }
        #endregion

        #region AS DECIMAL
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to an <see cref="decimal"/>; or 0M or @default.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The optional default value, which is 0M.</param>
        //=========================================================================================
        public static decimal ValueAsDecimal(this XAttribute attribute, decimal @default = 0M)
        {
            if(null == attribute) { return(@default); }

            decimal __result;
            return(decimal.TryParse(attribute.Value, out __result) ? __result : @default);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to an <see cref="decimal"/>; or 0M or @default.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        /// <param name="default">The optional default value, which is 0M.</param>
        //=========================================================================================
        public static decimal ValueAsDecimal(this XElement element, decimal @default = 0M)
        {
            if(null == element) { return(@default); }

            decimal __result;
            return(decimal.TryParse(element.Value, out __result) ? __result : @default);
        }
        #endregion

        #region AS DOUBLE
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to an <see cref="double"/>; or 0D or @default.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The optional default value, which is 0D.</param>
        //=========================================================================================
        public static double ValueAsDouble(this XAttribute attribute, double @default = 0D)
        {
            if(null == attribute) { return(@default); }

            double __result;
            return(double.TryParse(attribute.Value, out __result) ? __result : @default);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to an <see cref="double"/>; or 0D or @default.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        /// <param name="default">The optional default value, which is 0D.</param>
        //=========================================================================================
        public static double ValueAsDouble(this XElement element, double @default = 0D)
        {
            if(null == element) { return(@default); }

            double __result;
            return(double.TryParse(element.Value, out __result) ? __result : @default);
        }
        #endregion

        #region AS FLOAT
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to an <see cref="float"/>; or 0F or @default.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The optional default value, which is 0F.</param>
        //=========================================================================================
        public static float ValueAsFloat(this XAttribute attribute, float @default = 0F)
        {
            if(null == attribute) { return(@default); }

            float __result;
            return(float.TryParse(attribute.Value, out __result) ? __result : @default);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to an <see cref="float"/>; or 0F or @default.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        /// <param name="default">The optional default value, which is 0F.</param>
        //=========================================================================================
        public static float ValueAsFloat(this XElement element, float @default = 0F)
        {
            if(null == element) { return(@default); }

            float __result;
            return(float.TryParse(element.Value, out __result) ? __result : @default);
        }
        #endregion

        #region AS INT16
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to an <see cref="short"/>; or (short)0 or @default.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The optional default value, which is (short)0.</param>
        //=========================================================================================
        public static short ValueAsInt16(this XAttribute attribute, short @default = (short)0)
        {
            if(null == attribute) { return(@default); }

            short __result;
            return(short.TryParse(attribute.Value, out __result) ? __result : @default);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to an <see cref="short"/>; or (short)0 or @default.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        /// <param name="default">The optional default value, which is (short)0.</param>
        //=========================================================================================
        public static short ValueAsInt16(this XElement element, short @default = (short)0)
        {
            if(null == element) { return(@default); }

            short __result;
            return(short.TryParse(element.Value, out __result) ? __result : @default);
        }
        #endregion

        #region AS INT32
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to an <see cref="Int32"/>; or 0 or @default.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The optional default value, which is 0.</param>
        //=========================================================================================
        public static int ValueAsInt32(this XAttribute attribute, int @default = 0)
        {
            if(null == attribute) { return(@default); }

            int __result;
            return(int.TryParse(attribute.Value, out __result) ? __result : @default);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to an <see cref="Int32"/>; or 0.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        /// <param name="default">The optional default value, which is 0.</param>
        //=========================================================================================
        public static int ValueAsInt32(this XElement element, int @default = 0)
        {
            if(null == element) { return(@default); }

            int __result;
            return(int.TryParse(element.Value, out __result) ? __result : @default);
        }
        #endregion

        #region AS INT64
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to an <see cref="long"/>; or 0L or @default.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The optional default value, which is 0L.</param>
        //=========================================================================================
        public static long ValueAsInt64(this XAttribute attribute, long @default = 0L)
        {
            if(null == attribute) { return(@default); }

            long __result;
            return(long.TryParse(attribute.Value, out __result) ? __result : @default);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to an <see cref="long"/>; or 0L or @default.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        /// <param name="default">The optional default value, which is 0L.</param>
        //=========================================================================================
        public static long ValueAsInt64(this XElement element, long @default = 0L)
        {
            if(null == element) { return(@default); }

            long __result;
            return(long.TryParse(element.Value, out __result) ? __result : @default);
        }
        #endregion

        #region AS STRING
        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to a <see cref="string"/>.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        //=========================================================================================
        public static string ValueAsString(this XAttribute attribute)
        {
            return(null == attribute ? null : attribute.Value);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XAttribute"/> value to a <see cref="string"/>.
        /// </summary>
        /// <param name="attribute">The <see cref="XAttribute"/>.</param>
        /// <param name="default">The default value if the <see cref="XAttribute.Value"/> is <c>null</c> 
        /// or empty.</param>
        //=========================================================================================
        public static string ValueAsString(this XAttribute attribute, Func<string> @default)
        {
            if(null == attribute) { return(null == @default ? null : @default()); }
            string __value = attribute.Value;

            return(__value.IsNullOrEmpty() ? (null == @default ? __value : @default()) : __value);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to a <see cref="string"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        //=========================================================================================
        public static string ValueAsString(this XElement element)
        {
            return(null == element ? null : element.Value);
        }

        //=========================================================================================
        /// <summary>
        /// Converts an <see cref="XElement"/> value to a <see cref="string"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/>.</param>
        /// <param name="default">The default value if the <see cref="XElement.Value"/> is <c>null</c> 
        /// or empty.</param>
        //=========================================================================================
        public static string ValueAsString(this XElement element, Func<string> @default)
        {
            if(null == element) { return(null == @default ? null : @default()); }
            string __value = element.Value;

            return(__value.IsNullOrEmpty() ? (null == @default ? __value : @default()) : __value);
        }
        #endregion
    }
}
