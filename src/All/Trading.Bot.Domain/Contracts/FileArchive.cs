using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class FileArchive
    {
        public Guid Id { get; set; }
        public DateTime Updated { get; set; }
        public virtual List<FileHistory> Files { get; set; } = new List<FileHistory>();
    }
}
