using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using AddToOKAreport.Models;

namespace AddToOKAreport
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        static void Main(string[] args)
        {
            #region <ПРЯЧЕМ КОНСОЛЬНОЕ ОКНО>
            try
            {
                IntPtr hWnd = FindWindow(null, Console.Title);
                if (hWnd == IntPtr.Zero)
                {
                    throw new Exception("Окно не найдено");
                }
                else
                {
                    //Console.ReadKey();
                    //0-скрыть консольное окно; 1-показывать окно
                    ShowWindow( hWnd, 0);
                }
            } catch (Exception e)
            {
                AlertMsg(e);
                throw;
            }
            #endregion
       }
   }
}
