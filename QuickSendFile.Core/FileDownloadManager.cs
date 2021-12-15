using System.Net;
using System.Net.Sockets;

namespace QuickSendFile.Core
{
    public class FileDownloadManager
    {
        private readonly Settings _settings;
        private readonly Socket _sender;
        private List<byte> _fileBytes = new();
        public FileDownloadManager(Settings settings)
        {
            _settings = settings;
            IPAddress ipAddress = _settings.HostIpAddress;
            _sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        public void DownloadFile()
        {
            Console.WriteLine("Sunucudan dosya indirme islemi basliyor");

            try
            {
                IPEndPoint remoteEP = new IPEndPoint(_settings.HostIpAddress, _settings.HostPort);

                // soket baglantisi ac.
                _sender.Connect(remoteEP);

                // islemi baslatmak icin ok gonder
                _sender.Send(_settings.OK);

                // toplam gelecek byte sayisi alinir.
                var bytes = new byte[_settings.DataLen];
                int bytesRec = _sender.Receive(bytes);
                var fileByteLen = Convert.ToInt64(bytes.ToText(bytesRec));

                // devam icin OK gonderilir.
                _sender.Send(_settings.OK);

                // inen toplam byte sayisini burada saklar.
                long totalByteCount = 0;

                while (totalByteCount != fileByteLen)
                {
                    if (totalByteCount > fileByteLen)
                    {
                        Console.WriteLine("Hesap disi islem yapildi.");
                        return;
                    }

                    // dosya indirilmek uzere dinlenir.
                    bytesRec = _sender.Receive(bytes);
                    foreach (var item in bytes.Take(bytesRec))
                    {
                        _fileBytes.Add(item);
                    }
                    totalByteCount += bytesRec;
                    Console.WriteLine(totalByteCount + "/" + fileByteLen);
                }
                // dosya indirildi demek icin OK gonderilir.
                _sender.Send(_settings.OK);

                // dosya adi dinlenir
                bytesRec = _sender.Receive(bytes);
                var fileName = bytes.ToText(bytesRec);
                
                // tamamlandi demek icin OK gonderilir.
                _sender.Send(_settings.OK);

                var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                File.WriteAllBytes(filePath, _fileBytes.ToArray());
                Console.WriteLine(filePath);

                _sender.Shutdown(SocketShutdown.Both);
                _sender.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
    }
}
