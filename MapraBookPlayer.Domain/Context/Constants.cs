using System.Runtime.InteropServices;

namespace MapraBookPlayer.Domain.Context
{
    public static class Constants
    {
        public static string GetDbPath ()
        {
            string dataSource = "books.db";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                dataSource = @"..\..\..\books.db";
            }

            return dataSource;
        }
    }
}
