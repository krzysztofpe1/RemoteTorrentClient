using MonoTorrent;
using MonoTorrent.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteTorrentClient.Torrent;

internal class TorrentDownloader
{
    ClientEngine _engine { get; }
    public TorrentDownloader(ClientEngine engine)
    {
        _engine = engine;
    }
    public async Task DownloadAsync(string torrentFullPath, string downloadsPath)
    {
        try
        {
            var settingsBuilder = new TorrentSettingsBuilder
            {
                MaximumConnections = 60,
            };
            var manager = await _engine.AddAsync(torrentFullPath, downloadsPath, settingsBuilder.ToSettings());
        }catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        if (_engine.Torrents.Count == 0)
        {
            Console.WriteLine($"'{torrentFullPath}' is not a torrent file");
            Console.WriteLine("Exiting...");
            return;
        }
        foreach (TorrentManager manager in _engine.Torrents)
        {
            manager.PeerConnected += (o, e) => {
                    Console.WriteLine($"Connection succeeded: {e.Peer.Uri}");
            };
            manager.ConnectionAttemptFailed += (o, e) => {
                    Console.WriteLine(
                        $"Connection failed: {e.Peer.ConnectionUri} - {e.Reason}");
            };
            // Every time a piece is hashed, this is fired.
            manager.PieceHashed += delegate (object o, PieceHashedEventArgs e) {
                if(e!=null)
                    Console.WriteLine($"Piece Hashed: {e.PieceIndex} - {(e.HashPassed ? "Pass" : "Fail")}");
            };

            // Every time the state changes (Stopped -> Seeding -> Downloading -> Hashing) this is fired
            manager.TorrentStateChanged += delegate (object o, TorrentStateChangedEventArgs e) {
                if (e != null)
                    Console.WriteLine($"OldState: {e.OldState} NewState: {e.NewState}");
            };

            // Every time the tracker's state changes, this is fired
            manager.TrackerManager.AnnounceComplete += (sender, e) => {
                Console.WriteLine($"{e.Successful}: {e.Tracker}");
            };

            // Start the torrentmanager. The file will then hash (if required) and begin downloading/seeding.
            // As EngineSettings.AutoSaveLoadDhtCache is enabled, any cached data will be loaded into the
            // Dht engine when the first torrent is started, enabling it to bootstrap more rapidly.
            await manager.StartAsync();
            while(!manager.Complete)
            {

            }
            throw new Exception();
        }
    }
}
