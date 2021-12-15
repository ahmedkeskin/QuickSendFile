using QuickSendFile.Core;

namespace QuickSendFile.FileDownloader
{
    public static class Program
    {
        static void Main()
        {
            var settings = new Settings();
            var dm = new FileDownloadManager(settings);
            dm.DownloadFile();
        }
    }
}