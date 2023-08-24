using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestProj
{

    public class MultiplatformFileWatcher
    {
        public event FileSystemEventHandler Created;

        public event FileSystemEventHandler Deleted;

        public event FileSystemEventHandler Changed;

        private readonly string _directoryToWatch;
        private string _searchPattern;

        private readonly ConcurrentDictionary<string, DateTime> _monitoredFiles = new ConcurrentDictionary<string, DateTime>();

        public MultiplatformFileWatcher(string directoryToWatch, string searchPattern)
        {
            _searchPattern = searchPattern ?? throw new ArgumentNullException(nameof(searchPattern));
            _directoryToWatch = directoryToWatch ?? throw new ArgumentNullException(nameof(directoryToWatch));
        }

        public async Task WatchForFileChangesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                List<string> allFiles = Directory.EnumerateFiles(_directoryToWatch, _searchPattern, SearchOption.AllDirectories).ToList();
                foreach (string file in allFiles)
                {
                    if (_monitoredFiles.TryGetValue(file, out DateTime existingTime))
                    {
                        var currentTime = File.GetLastWriteTime(file);
                        if (existingTime != currentTime)
                        {
                            Changed?.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Changed, _directoryToWatch, Path.GetFileName(file)));
                            _monitoredFiles.TryUpdate(file, File.GetLastWriteTime(file), existingTime);
                        }
                    }
                    else
                    {
                        if (File.Exists(file))
                        {
                            Created?.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Created, _directoryToWatch, Path.GetFileName(file)));
                            _monitoredFiles.TryAdd(file, File.GetLastWriteTime(file));
                        }
                    }
                }

                // Detection deleted files
                foreach (string monitoredFile in _monitoredFiles.Keys)
                {
                    if (!allFiles.Exists(item => item == monitoredFile))
                    {
                        Deleted?.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Deleted, _directoryToWatch, Path.GetFileName(monitoredFile)));
                        _monitoredFiles.TryRemove(monitoredFile, out _);
                    }
                }

                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
