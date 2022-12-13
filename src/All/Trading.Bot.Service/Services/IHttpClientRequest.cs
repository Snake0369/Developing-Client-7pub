using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Service.Services
{
    public interface IHttpClientRequest
    {
        Task<string> GetTradingFile(string market, string code, string fileCode, string fileName, DateTime date);
    }
}
