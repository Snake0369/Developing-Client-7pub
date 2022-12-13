using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class CommentMap
    {
        public long Id { get; set; }
        public Guid ChannelId { get; set; }
        public Guid SubpositionId { get; set; }
        public Guid OrderItemId { get; set; }
    }
}
