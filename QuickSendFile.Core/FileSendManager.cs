using System.Net;
using System.Net.Sockets;

namespace QuickSendFile.Core
{
    public class FileSendManager
    {
        private readonly Settings _settings;

        public FileSendManager(Settings settings)
        {
            _settings = settings;
        }

        // Incoming data from the client.  
        public string data = null;

        public void StartListening()
        {

            var ipAddress = _settings.HostIpAddress;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, _settings.HostPort);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);
                Socket handler = listener.Accept();

                bool isActive = true;
                while (isActive)
                {
                    Console.WriteLine("Baslatmak icin OK Bekleniyor");
                    var readByte = new Byte[2];
                    int bytesRec = handler.Receive(readByte);
                    if (readByte.ToText(bytesRec) != "OK")
                    {
                        Console.WriteLine("Patladi"); continue;
                    }

                    Console.WriteLine("Dosya okunuyor");
                    var filePath = @"C:\Users\ahmet.keskin\source\repos\QuickSendFile\QuickSendFile.FileDownloader\bin\Release\net6.0\publish.rar";
                    var byteFile = File.ReadAllBytes(filePath);
                                        
                    var byteCount = byteFile.Length.ToByte();

                    Console.WriteLine("Dosya byte bilgisi gonderiliyor");
                    handler.Send(byteCount);
                    
                    Console.WriteLine("Dosya gondermek icin OK Bekleniyor");
                    bytesRec = handler.Receive(readByte);

                    Console.WriteLine("Dosya gonderiliyor");
                    handler.Send(byteFile);

                    readByte = new Byte[2];
                    Console.WriteLine("Dosya indirildi icin OK Bekleniyor");
                    bytesRec = handler.Receive(readByte);

                    Console.WriteLine("Dosya adi gonderiliyor");
                    var fileNameByte = Path.GetFileName(filePath).ToByte();
                    handler.Send(fileNameByte);

                    readByte = new Byte[2];
                    Console.WriteLine("Dosya adi okundu icin OK Bekleniyor");
                    bytesRec = handler.Receive(readByte);
                    isActive = false;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nSunucu kapandi");
        }
    }
}
