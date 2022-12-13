using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;

namespace Trading.Bot.Service.Services
{
    public interface IDataLoaderService
    {
        Task LoadData(FileArchive fileArchive, Contract contract, DateTime date, TimeSpan timeframe, int depthLoading, decimal brick);
        Task<List<Contract>> LoadContracts();
        Task<OrderChannel?> LoadOrderChannel(OrderChannel channel);
        Task RemoveDataOfLastTradeDay(Contract contract, TimeSpan timeframe);
        Task RemoveDataOutOfCountTradeDays(Contract contract, TimeSpan timeframe, int depthLoading);
    }
}
