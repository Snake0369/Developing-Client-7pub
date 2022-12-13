using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class FileHistory
    {
        public Guid Id { get; set; }
        public Guid FileArchiveId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
        public bool IsWarning { get; set; }
        public int Timeframe { get; set; }
        public virtual FileArchive? FileArchive { get; set; }
    }
}
