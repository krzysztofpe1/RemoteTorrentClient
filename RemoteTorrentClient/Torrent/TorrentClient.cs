using MonoTorrent.Client;

namespace RemoteTorrentClient.Torrent;
internal class TorrentClient
{
    public TorrentClient()
    {
        
    }
    public async void Download()
    {
        CancellationTokenSource cancellation = new CancellationTokenSource();
        var settingBuilder = new EngineSettingsBuilder()
        {
            AllowPortForwarding = true,
            AutoSaveLoadDhtCache = true,
            AutoSaveLoadFastResume = true,
            AutoSaveLoadMagnetLinkMetadata = true
        };
        using var engine = new ClientEngine(settingBuilder.ToSettings());
        var task = new TorrentDownloader(engine).DownloadAsync("Z:\\C#\\RemoteTorrentClient\\RemoteTorrentClient\\bin\\Debug\\net8.0\\Torrents\\Rick and Morty (2023) (S07E10) (480p).torrent", "Z:\\C#\\RemoteTorrentClient\\RemoteTorrentClient\\bin\\Debug\\net8.0\\Downloads");
        await task;
        Task.Delay(10000);
        while(engine.IsRunning)
        {
            Console.WriteLine($"{engine.TotalDownloadSpeed / 1024.0:0.00}kB/sec ↓ / {engine.TotalUploadSpeed / 1024.0:0.00}kB/sec ↑\"");
        }
    }
}
