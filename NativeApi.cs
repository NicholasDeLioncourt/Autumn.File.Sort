namespace Autumn.File
{
    using System.Runtime.InteropServices;

    internal struct NativeApi
    {
        internal struct Kernal32
        {
            // ReSharper disable InconsistentNaming
            //=========================================================================================
            /// <summary>
            /// In library Kernal32, Unicode method MoveFileW (WinBase.h Windows.h). If the function fails, 
            /// the return value is zero. To get extended error information, invoke GetLastError
            /// </summary>
            //=========================================================================================
            [DllImport("kernel32.dll", CharSet=CharSet.Unicode, SetLastError=true, ExactSpelling=true)]
            public static extern bool MoveFileW(string lpExistingFileName, string lpNewFileName);
            // ReSharper restore InconsistentNaming
        }

    }
}
