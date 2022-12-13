using StockSharp.BusinessEntities;
using StockSharp.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.UI.Services
{
    public class CustomSecurityIdGenerator : SecurityIdGenerator
    {
        public override string GenerateId(string secCode, string board)
        {
            // генерация идентификатора вида CODE
            return secCode;
        }
    }
}
