using QuickSendFile.Core;

namespace QuickSendFile.FileSender
{
    public static class Program
    {
        static void Main()
        {
            var settings = new Settings();
            var listener = new FileSendManager(settings);
            listener.StartListening();
            Console.ReadLine();
        }
    }
}