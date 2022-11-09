namespace Autumn.File
{
    //=============================================================================================
    /// <summary>Represents an empty array</summary>
    /// <created>LNDL</created>
    /// <copyright file="EmptyArray.cs" company="De Lioncourt LLC">
    /// L. Nicholas de Lioncourt. All code herein is proprietary.
    /// </copyright>
    //=============================================================================================
    #if PORTABLEVERSION
    using System.Runtime.Serialization;

    [DataContract(IsReference = true)]
    #endif

    public static class EmptyArray<T>
    {  
        #if PORTABLEVERSION
        [DataMember]
        #endif
        public static readonly T [] Instance;

        //=========================================================================================
        /// <summary>Initializes the <see cref="EmptyArray{T}"/> class.</summary>
        //=========================================================================================
        static EmptyArray() { EmptyArray<T>.Instance = new T[0]; }
    }
}
