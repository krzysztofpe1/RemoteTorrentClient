using MonoTorrent.Client;
using RemoteTorrentClient.Torrent;

namespace RemoteTorrentClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var temp = new TorrentClient();
            temp.Download();
            while(true)
            {
                
            }
        }
    }
}
