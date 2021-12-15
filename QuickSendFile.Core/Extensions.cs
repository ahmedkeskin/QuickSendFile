using System.Text;

namespace QuickSendFile.Core
{
    public static class Extensions
    {
        public static string ToText(this byte[] bytes, int len)
        {
            return Encoding.ASCII.GetString(bytes, 0, len);
        }
        public static byte[] ToByte(this string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }

        public static byte[] ToByte(this int data)
        {
            return Encoding.ASCII.GetBytes(data.ToString());
        }
    }
}