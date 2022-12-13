using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Core.Extensions;
using Trading.Bot.DataAccess.Repositories;
using Trading.Bot.Domain.Contracts;
using Trading.Bot.Service.Cache;
using Trading.Bot.Service.Model;

namespace Trading.Bot.Service.Services
{
    public class DataLoaderService : IDataLoaderService
    {
        private readonly ILogger _logger = Log.ForContext<DataLoaderService>();
        private readonly ITradingFutRepository _tradingFutHistoryRepository;
        private readonly IHttpClientRequest _httpClientRequest;
        private readonly string _directory;
        private readonly IEnumerable<ContractDto> _contractDtos;
        private readonly IConfiguration _configuration;

        public DataLoaderService(ITradingFutRepository tradingFutHistoryRepository,
            IHttpClientRequest httpClientRequest,
            string directory,
            IEnumerable<ContractDto> contractDtos,
            IConfigurationRoot configuration)
        {
            _tradingFutHistoryRepository = tradingFutHistoryRepository;
            _httpClientRequest = httpClientRequest;
            _directory = directory;
            _contractDtos = contractDtos;
            _configuration = configuration;
        }

        public async Task LoadData(FileArchive fileArchive, Contract contract, DateTime date, TimeSpan timeframe, int depthLoading, decimal brick)
        {
            var archive = await _tradingFutHistoryRepository.GetFileArchiveAsync(fileArchive.Id);
            if (archive == null)
            {
                await _tradingFutHistoryRepository.AddFileArchiveAsync(fileArchive);
            }
            _logger.Information($"Проверка актуальности данных по контракту {contract.Ticker}" +
                $" (таймфрейм {(int)timeframe.TotalMinutes} минут) и загрузка, если данные не актуальны");
            depthLoading = depthLoading + (1 + depthLoading / 5) * 2;
            var contractDto = _contractDtos.FirstOrDefault(c => c.Ticker == contract.Ticker);
            if (contractDto == null)
            {
                throw new Exception($"В настройках не найден контракт {contract.Ticker}");
            }
            var bars_ = await _tradingFutHistoryRepository.GetBarsAsync(contract, (int)timeframe.TotalMinutes);
            contract.BarHistories.AddRange(bars_);
            var bars = contract.BarHistories
                .Where(x => x.Timeframe == (int)timeframe.TotalMinutes)
                .OrderBy(x => x.Timestamp)
                .ToList();
            BarHistory? bar = null;
            if (bars.Any())
            {
                bar = bars.Last();
            }
            var dateOfLastBar = DateTime.MinValue;
            if (bar == null)
            {
                _logger.Information($"Данные по контракту {contract.Ticker}" +
                    $" (таймфрейм {(int)timeframe.TotalMinutes} минут) в базе данных не найдены");
                dateOfLastBar = date.AddDays(-depthLoading);
            }
            else
            {
                dateOfLastBar = bar.Timestamp;
                _logger.Information($"Последние данные по контракту {contract.Ticker}" +
                    $" (таймфрейм {(int)timeframe.TotalMinutes} минут) :=> {dateOfLastBar:yyyy/MM/dd HH:mm}");
            }

            dateOfLastBar = dateOfLastBar.EndOfEveningSession(timeframe) != null ?
                dateOfLastBar.EndOfEveningSession(timeframe).Value.Add(-timeframe) <
                dateOfLastBar ? dateOfLastBar.AddDays(1) : dateOfLastBar : dateOfLastBar.EndOfBaseSession(timeframe).Add(-timeframe) <
                dateOfLastBar ? dateOfLastBar.AddDays(1) : dateOfLastBar;
            if (dateOfLastBar.Date < date.Date)
            {
                var rnd = new Random(DateTime.Now.Millisecond);
                while (dateOfLastBar.Date < date.Date)
                {
                    _logger.Information($"Загружаем данные по контракту {contract.Ticker}" +
                        $" (таймфрейм {(int)timeframe.TotalMinutes} минут) за {dateOfLastBar:yyyy/MM/dd}");
                    bool isFileGet = false;
                    try
                    {
                        var fileCode = $"SPFB.{contractDto.BaseTicker}-{contractDto.Expired:MM.yy}";
                        var fileName = $"{fileCode}_{dateOfLastBar:yyMMdd}_{dateOfLastBar:yyMMdd}";
                        var fileNameOut = $"{_directory}\\" +
                                $"{contract.Ticker.Trim().Substring(0, 2)}\\{fileName}.txt";
                        _logger.Information($"Проверяем {fileNameOut} файл");
                        if (!File.Exists(fileNameOut))
                        {
                            // Если файл не существует, скачиваем файл
                            _logger.Information($"Скачиваем {fileCode} ..");
                            isFileGet = true;
                            var sFile = await _httpClientRequest.GetTradingFile(contractDto.Market, contractDto.Code, fileCode, fileName, dateOfLastBar);
                            if (!string.IsNullOrEmpty(sFile))
                            {
                                // Если файл не пустой, сохраняем
                                await using var stream = new StreamWriter(new FileStream(fileNameOut,
                                    FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
                                {
                                    stream.AutoFlush = true;
                                    await stream.WriteAsync(sFile);
                                    stream.Close();
                                }
                            }
                            else
                            {
                                dateOfLastBar = dateOfLastBar.AddDays(1);
                                continue;
                            }
                        }
                        var file = Path.GetFileNameWithoutExtension(fileNameOut);
                        if (archive != null)
                        {
                            var trades = new List<TradeHistory>();
                            var fileHistory = archive.Files.FirstOrDefault(x => x.FileName == file);
                            if (fileHistory == null || fileHistory.IsWarning)
                            {
                                // Если файл мы не обрабатывали, либо обрабатывали с ошибкой
                                if (fileHistory != null)
                                {
                                    // Если файл обработан с ошибкой, удаляем все сделки за текущую дату
                                    await _tradingFutHistoryRepository.DeleteTradesAsync(contract, dateOfLastBar);
                                }
                                using var streamR = new StreamReader(File.OpenRead(fileNameOut));
                                var fileStrings = streamR.ReadToEnd().Split("\r\n");
                                long count = 0;
                                foreach (var line in fileStrings)
                                {
                                    var lineString = line.Trim().Split(",");
                                    if (lineString.Length < 6)
                                    {
                                        continue;
                                    }
                                    var dto = DateTimeOffset.ParseExact(lineString[2] + lineString[3], "yyyyMMddHHmmss", null);
                                    trades.Add(new TradeHistory
                                    {
                                        Id = Guid.NewGuid(),
                                        TradeId = contract.Ticker + (dto.ToUnixTimeMilliseconds() + ++count).ToString(),
                                        Timestamp = dto.DateTime,
                                        ContractId = contract.Id,
                                        Contract = contract,
                                        Price = decimal.Parse(lineString[4]),
                                        Volume = decimal.Parse(lineString[5]),
                                        Updated = DateTime.Now
                                    });
                                }
                                await _tradingFutHistoryRepository.AddTradesAsync(trades);
                                if (fileHistory == null)
                                {
                                    fileHistory = new FileHistory
                                    {
                                        Id = Guid.NewGuid(),
                                        FileArchiveId = archive.Id,
                                        FileArchive = archive,
                                        FileName = file,
                                        IsWarning = false,
                                        LastModified = DateTime.Now,
                                        Timeframe = (int)timeframe.TotalMinutes
                                    };
                                    archive.Files.Add(fileHistory);
                                    await _tradingFutHistoryRepository.AddFileHistoryAsync(fileHistory);
                                }
                                else
                                {
                                    fileHistory.IsWarning = false;
                                    await _tradingFutHistoryRepository.UpdateFileHistoryAsync(fileHistory);
                                }
                            }
                            // делаем бары
                            if (!trades.Any())
                            {
                                trades.AddRange(await _tradingFutHistoryRepository.GetTradesAsync(contract, dateOfLastBar));
                            }
                            _logger.Information($"Генерируем бары по контракту {contract.Ticker}" +
                                $" (таймфрейм {(int)timeframe.TotalMinutes} минут) за {dateOfLastBar:yyyy/MM/dd}");
                            bars = GenerateBarsByDate(contract, trades, dateOfLastBar, timeframe, bar);
                            if (bars.Any())
                            {
                                contract.BarHistories.AddRange(bars);
                                await _tradingFutHistoryRepository.AddBarsAsync(bars);
                                bar = bars.Last();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "load data");
                        throw;
                    }
                    finally
                    {
                        if (isFileGet)
                        {
                            await Task.Delay(TimeSpan.FromMilliseconds(rnd.Next(5000, 15000)));
                        }
                    }
                    dateOfLastBar = dateOfLastBar.AddDays(1);
                }
            }
            
            _logger.Information($"Актуализируем ренко по контракту {contract.Ticker}" +
                $" (таймфрейм {(int)timeframe.TotalMinutes} минут)");
            var renkos = contract.Renkos.Where(x => x.Timeframe == (int)timeframe.TotalMinutes);
            if (!renkos.Any())
            {
                if (contract.BarHistories.Where(b => b.Timeframe == (int)timeframe.TotalMinutes).Any())
                {
                    var highBorder = Math.Ceiling(contract.BarHistories.Where(b => b.Timeframe == (int)timeframe.TotalMinutes).First().Close / brick) * brick;
                    var lowBorder = highBorder - brick;
                    foreach (var b in contract.BarHistories.Where(b => b.Timeframe == (int)timeframe.TotalMinutes))
                    {
                        if (b.Close <= lowBorder - brick)
                        {
                            while (b.Close <= lowBorder - brick)
                            {
                                lowBorder -= brick;
                                highBorder = lowBorder + brick;
                            }
                        }
                        else if (b.Close >= highBorder + brick)
                        {
                            while (b.Close >= highBorder + brick)
                            {
                                highBorder += brick;
                                lowBorder = highBorder - brick;
                            }
                        }
                        var renko = new Renko
                        {
                            Id = Guid.NewGuid(),
                            Timestamp = b.Timestamp,
                            Timeframe = b.Timeframe,
                            ContractId = contract.Id,
                            Contract = contract,
                            HighBorder = highBorder,
                            LowBorder = lowBorder
                        };
                        contract.Renkos.Add(renko);
                        await _tradingFutHistoryRepository.AddRenkoAsync(renko);
                    }
                }
            }
            else
            {
                var highBorder = renkos.Last().HighBorder;
                var lowBorder = renkos.Last().LowBorder;
                bars = contract.BarHistories.Where(x => x.Timeframe == (int)timeframe.TotalMinutes && x.Timestamp > renkos.Last().Timestamp).ToList();
                if (bars.Any())
                {
                    foreach (var b in bars)
                    {
                        if (b.Close <= lowBorder - brick)
                        {
                            while (b.Close <= lowBorder - brick)
                            {
                                lowBorder -= brick;
                                highBorder = lowBorder + brick;
                            }
                        }
                        else if (b.Close >= highBorder + brick)
                        {
                            while (b.Close >= highBorder + brick)
                            {
                                highBorder += brick;
                                lowBorder = highBorder - brick;
                            }
                        }
                        var renko = new Renko
                        {
                            Id = Guid.NewGuid(),
                            Timestamp = b.Timestamp,
                            Timeframe = b.Timeframe,
                            ContractId = contract.Id,
                            Contract = contract,
                            HighBorder = highBorder,
                            LowBorder = lowBorder
                        };
                        contract.Renkos.Add(renko);
                        await _tradingFutHistoryRepository.AddRenkoAsync(renko);
                    }
                }
            }
            _logger.Information($"Ренко по контракту {contract.Ticker}" +
                $" (таймфрейм {(int)timeframe.TotalMinutes} минут) актуализирован");

            _logger.Information($"Данные по контракту {contract.Ticker}" +
                $" (таймфрейм {(int)timeframe.TotalMinutes} минут) актуальны");
        }

        public async Task<List<Contract>> LoadContracts()
        {
            _logger.Information("Проверяем актуальность таблицы контрактов ..");
            var contracts = new List<Contract>();
            foreach (var contractOfBaseTicker in _contractDtos.GroupBy(t => t.BaseTicker))
            {
                contracts.AddRange(await _tradingFutHistoryRepository.GetContractsAsync(contractOfBaseTicker.Key));
            }
            await Parallel.ForEachAsync(_contractDtos, new ParallelOptions { MaxDegreeOfParallelism = 10 },
                async (contract, token) =>
                {
                    if (contracts.All(c => c.Ticker != contract.Ticker))
                    {
                        _logger.Information($"Загружаем {contract.Ticker} контракт");
                        await _tradingFutHistoryRepository.AddContractAsync(new Contract
                        {
                            Id = Guid.NewGuid(),
                            BaseTicker = contract.BaseTicker,
                            Ticker = contract.Ticker,
                            Description = contract.Description,
                            Expired = contract.Expired,
                            LastTradingDate = contract.DateLastTrade
                        });
                    }
                });
            var contracts_ = await _tradingFutHistoryRepository.GetCurrentContractsAsync(DateTime.Now);
            var renkoSection = _configuration.GetSection("Renko:Contracts");
            foreach (IConfigurationSection section in renkoSection.GetChildren())
            {
                var baseTicker = section.GetValue<string>("BaseTicker");
                var renkos = section.GetSection("Renkos");
                var renkos_ = new List<Brick>();
                foreach (IConfigurationSection renko in renkos.GetChildren())
                {
                    renkos_.Add(new Brick
                    {
                        Timeframe = new TimeSpan(0, renko.GetValue<int>("Timeframe"), 0),
                        Value = renko.GetValue<decimal>("Brick")
                    });
                }
                var contract_ = contracts_.FirstOrDefault(x => x.BaseTicker == baseTicker);
                if (contract_ != null)
                {
                    SettingsBrick.Bricks.TryAdd(contract_.Id, renkos_);
                }
            }
            foreach (var contract in contracts_)
            {
                var renkos = await _tradingFutHistoryRepository.GetRenkosByContract(contract);
                contract.Renkos.AddRange(renkos);
            }
            _logger.Information("Таблицы контрактов актуальны");
            return contracts_.ToList();
        }

        public async Task<OrderChannel?> LoadOrderChannel(OrderChannel channel)
        {
            var channelEx = await _tradingFutHistoryRepository.GetOrderChannelAsync(channel);
            if (channelEx == null)
            {
                await _tradingFutHistoryRepository.AddOrderChannelAsync(channel);
                return channel;
            }
            return channelEx;
        }

        public async Task RemoveDataOfLastTradeDay(Contract contract, TimeSpan timeframe)
        {
            var bar = await _tradingFutHistoryRepository.GetLastBarAsync(contract.BaseTicker, (int)timeframe.TotalMinutes);
            var date = bar?.Timestamp.Date;
            if (date != null)
            {
                await _tradingFutHistoryRepository.DeleteBarsByDateAsync(contract, (int)timeframe.TotalMinutes, date.Value);
                await _tradingFutHistoryRepository.DeleteTradesAsync(contract, date.Value);
                await _tradingFutHistoryRepository.DeleteRenkosByDate(contract, (int)timeframe.TotalMinutes, date.Value);
            }
        }

        public async Task RemoveDataOutOfCountTradeDays(Contract contract, TimeSpan timeframe, int depthLoading)
        {
            depthLoading = depthLoading + (1 + depthLoading / 5) * 2;
            var bars = await _tradingFutHistoryRepository.GetBarsAsync(contract, (int)timeframe.TotalMinutes);
            foreach (var key in bars.Where(b => b.Timestamp.Date < DateTime.Now.Date)
                .GroupBy(x => x.Timestamp.Date)
                .OrderBy(x => x.Key))
            {
                var date = key.Key;
                await _tradingFutHistoryRepository.DeleteBarsByDateAsync(contract, (int)timeframe.TotalMinutes, date);
                await _tradingFutHistoryRepository.DeleteTradesAsync(contract, date);
                await _tradingFutHistoryRepository.DeleteRenkosByDate(contract, (int)timeframe.TotalMinutes, date);
            }
        }

        private List<BarHistory> GenerateBarsByDate(Contract contract, List<TradeHistory> trades, DateTime date, TimeSpan timeframe, BarHistory? bar)
        {
            if (trades.Any())
            {
                var bars = new List<BarHistory>();
                var timestampSta = trades[0].Timestamp.StartOfBaseSession();
                var timestampEnd = timestampSta.EndOfEveningSession(timeframe);
                if (timestampSta >= new DateTime(2022, 2, 24) && timestampSta < new DateTime(2022, 7, 12))
                {
                    if ((int)timeframe.TotalMinutes > 15)
                    {
                        timestampEnd = timestampSta.EndOfBaseSession(timeframe).AddHours(1);
                    }
                    else
                    {
                        timestampEnd = timestampSta.EndOfBaseSession(timeframe);
                    }
                }
                while (timestampSta.Add(timeframe) <= timestampEnd)
                {
                    if (trades.Any(t => t.Timestamp >= timestampSta && t.Timestamp < timestampSta.Add(timeframe)))
                    {
                        var tradesOfTimeframe = trades
                            .Where(t => t.Timestamp >= timestampSta && t.Timestamp < timestampSta.Add(timeframe))
                            .ToList();
                        bars.Add(new BarHistory
                        {
                            Id = Guid.NewGuid(),
                            Timestamp = timestampSta.Add(timeframe),
                            Timeframe = (int)timeframe.TotalMinutes,
                            ContractId = contract.Id,
                            Open = tradesOfTimeframe.First().Price,
                            High = tradesOfTimeframe.Max(t => t.Price),
                            Low = tradesOfTimeframe.Min(t => t.Price),
                            Close = tradesOfTimeframe.Last().Price,
                            Volume = tradesOfTimeframe.Sum(t => t.Volume)
                        });
                    }
                    else if (timeframe.TotalMinutes < 30 && timestampSta >= timestampSta.EndOfBaseSession(timeframe) && timestampSta < timestampSta.StartOfEveningSession())
                    {
                        timestampSta = timestampSta.StartOfEveningSession() != null ? timestampSta.StartOfEveningSession().Value :
                            timestampSta.AddDays(1).StartOfBaseSession();
                        continue;
                    }
                    else
                    {
                        if (bars.Any())
                        {
                            bars.Add(new BarHistory
                            {
                                Id= Guid.NewGuid(),
                                Timestamp = timestampSta.Add(timeframe),
                                Timeframe = (int)timeframe.TotalMinutes,
                                ContractId = contract.Id,
                                Open = bars.Last().Open,
                                High = bars.Last().High,
                                Low = bars.Last().Low,
                                Close = bars.Last().Close,
                                Volume = 0
                            });
                        }
                        else
                        {
                            if (bar != null)
                            {
                                bars.Add(new BarHistory
                                {
                                    Id = Guid.NewGuid(),
                                    Timestamp = timestampSta.Add(timeframe),
                                    Timeframe = (int)timeframe.TotalMinutes,
                                    ContractId = contract.Id,
                                    Open = bar.Open,
                                    High = bar.High,
                                    Low = bar.Low,
                                    Close = bar.Close,
                                    Volume = 0
                                });
                            }
                            else
                            {
                                var trade = trades[0];
                                bars.Add(new BarHistory
                                {
                                    Id = Guid.NewGuid(),
                                    Timestamp = timestampSta.Add(timeframe),
                                    Timeframe = (int)timeframe.TotalMinutes,
                                    ContractId = contract.Id,
                                    Open = trade.Price,
                                    High = trade.Price,
                                    Low = trade.Price,
                                    Close = trade.Price,
                                    Volume = 0
                                });

                            }
                        }
                    }
                    timestampSta = timestampSta.Add(timeframe);
                    Console.WriteLine($"{contract.Ticker} - time {timestampSta:yyyy-MM-dd HH:mm:ss}");
                    _logger.Information($"{contract.Ticker} - time {timestampSta:yyyy-MM-dd HH:mm:ss}");
                }
                return bars;
            }
            return new List<BarHistory>();
        }
    }
}
