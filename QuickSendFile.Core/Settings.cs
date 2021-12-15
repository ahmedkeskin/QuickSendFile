using System.Net;

namespace QuickSendFile.Core
{
    public class Settings
    {
        private string _okMessage = "OK";
        private string _hostIpAddress = "192.168.1.49";
        public int DataLen { get { return 5242880; } }
        public byte[] OK { get { return _okMessage.ToByte(); } }
        public int OkLen { get { return _okMessage.ToByte().Length; } }
        public IPAddress HostIpAddress { get { return IPAddress.Parse(_hostIpAddress); } }
        public int HostPort = 11000;
    }
}
