namespace Trading.Bot.UI
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Windows;

	using Ecng.Common;
	using Ecng.Configuration;
	using Ecng.Serialization;
	using Ecng.Xaml;
	using Ecng.Collections;

	using StockSharp.Algo;
	using StockSharp.Algo.Storages;
	using StockSharp.BusinessEntities;
	using StockSharp.Configuration;
	using StockSharp.Localization;
	using StockSharp.Logging;
	using StockSharp.Messages;
	using StockSharp.Xaml;
	using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
	using System.ServiceModel;
	using Trading.Bot.Transfer;
	using StockSharp.Algo.Candles;
	using PubnubApi;
	using StockSharp.Xaml.PropertyGrid;
	using StockSharp.Algo.Storages.Csv;
	using Trading.Bot.UI.Services;
	using FeedOSManaged;

	public partial class MainPanel
	{
        const string USERNAME = "Sundancer0369";
        const string IP = "127.0.0.1";
        const int PORT = 8020;
        const int maxServerExceptionCount = 5;

        private CandleSeries _series;
        
		//объявляем хранилище торговых объектов CsvEntityRegistry и хранилище маркет-данных StorageRegistry
        private readonly CsvEntityRegistry _csvEntityRegistry;
        private readonly StorageRegistry _storageRegistry;
        private readonly SnapshotRegistry _snapshotRegistry;
        // Путь к данным истории
        private const string _historyPath = @"e:\DataServer\";

        private readonly OrdersWindow _ordersWindow = new();
		private readonly PortfoliosWindow _portfoliosWindow = new();
		private readonly MyTradesWindow _myTradesWindow = new();
		private readonly TradesWindow _tradesWindow = new();
		private readonly OrdersLogWindow _orderLogWindow = new();
		private readonly WSHttpBinding _binding;
        private readonly EndpointAddress _address;
		private readonly ConcurrentDictionary<int, Candle> _candles;

        public Connector Connector { get; private set; }

		private bool _isConnected;
        private IWorkService _proxy;
        private int _serverExceptionCount;
        private volatile bool _isServerConnectionEstablished;

        private readonly string _defaultDataPath = "Data";
		private readonly string _settingsFile;

		public MainPanel()
		{
			InitializeComponent();

			_ordersWindow.MakeHideable();
			_myTradesWindow.MakeHideable();
			_tradesWindow.MakeHideable();
			_portfoliosWindow.MakeHideable();
			_orderLogWindow.MakeHideable();

            _binding = new WSHttpBinding(SecurityMode.None, true);
            _address = new EndpointAddress(string.Format("http://{0}:{1}/WorkService", IP, PORT));
			_candles = new ConcurrentDictionary<int, Candle>();
            _defaultDataPath = _defaultDataPath.ToFullPath();

			_settingsFile = Path.Combine(_defaultDataPath, $"connection{Paths.DefaultSettingsExt}");
		}

		public event Func<string, Connector> CreateConnector;

		private void MainPanel_OnLoaded(object sender, RoutedEventArgs e)
		{
			var logManager = new LogManager();
			logManager.Listeners.Add(new FileLogListener { LogDirectory = Path.Combine(_defaultDataPath, "Logs") });
			logManager.Listeners.Add(new GuiLogListener(Monitor));

			Connector = CreateConnector?.Invoke(_defaultDataPath) ?? new Connector();
			logManager.Sources.Add(Connector);

			InitConnector();

			InitConnectionToServer();

            Connector.SecurityIdGenerator = new CustomSecurityIdGenerator();
            var message = new SecurityLookupMessage();
			SecurityId securityId = new SecurityId
			{
				SecurityCode = "ALL",
				BoardCode = "Forts"
			};
            message.SecurityId = securityId;

			foreach (var interval in intervals) {
				var editor = new CandleSettingsEditor();
				Security security = new Security();
				var candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromMinutes(5))
				{
					BuildCandlesMode = MarketDataBuildModes.Load,
				};
				Connector.GetCandles(candleSeries,
					Ecng.ComponentModel.Range(DateTime.Now.AddDays(-3), DateTime.Now);
			}
		}

		public void Close()
		{
			_ordersWindow.DeleteHideable();
			_myTradesWindow.DeleteHideable();
			_tradesWindow.DeleteHideable();
			_portfoliosWindow.DeleteHideable();
			_orderLogWindow.DeleteHideable();

			_tradesWindow.Close();
			_myTradesWindow.Close();
			_ordersWindow.Close();
			_portfoliosWindow.Close();
			_orderLogWindow.Close();

			Connector.Dispose();
		}

        private void InitConnectionToServer()
        {
            _serverExceptionCount = 0;
            var channel = new ChannelFactory<IWorkService>(_binding, _address);
			_proxy = channel.CreateChannel();
            var errorMessage = _proxy.InitConnection(USERNAME);
            if (errorMessage != null)
            {
                throw new Exception(errorMessage);
            }
        }

        private void InitConnector()
		{
			// subscribe on connection successfully event
			Connector.Connected += () =>
			{
				this.GuiAsync(() => ChangeConnectStatus(true));

				if (Connector.Adapter.IsMarketDataTypeSupported(DataType.News) && !Connector.Adapter.IsSecurityNewsOnly)
				{
					if (Connector.Subscriptions.All(s => s.DataType != DataType.News))
						Connector.SubscribeNews();
				}
			};

			// subscribe on connection error event
			Connector.ConnectionError += error => this.GuiAsync(() =>
			{
				ChangeConnectStatus(false);
				MessageBox.Show(this.GetWindow(), error.ToString(), LocalizedStrings.Str2959);
			});

			Connector.Disconnected += () => this.GuiAsync(() => ChangeConnectStatus(false));

			// subscribe on error event
			//Connector.Error += error =>
			//	this.GuiAsync(() => MessageBox.Show(this, error.ToString(), LocalizedStrings.Str2955));

			// subscribe on error of market data subscription event
			Connector.MarketDataSubscriptionFailed += (security, msg, error) =>
				this.GuiAsync(() => MessageBox.Show(this.GetWindow(), error.ToString(), LocalizedStrings.Str2956Params.Put(msg.DataType2, security)));

			Connector.TickTradeReceived += (s, t) =>
			{
				_tradesWindow.TradeGrid.Trades.Add(t);

			};
			Connector.OrderLogItemReceived += (s, ol) => _orderLogWindow.OrderLogGrid.LogItems.Add(ol);

			Connector.NewMyTrade += _myTradesWindow.TradeGrid.Trades.Add;

			Connector.PositionReceived += (sub, p) => _portfoliosWindow.PortfolioGrid.Positions.TryAdd(p);


			var nativeIdStorage = ServicesRegistry.TryNativeIdStorage;

			if (nativeIdStorage != null)
			{
				Connector.Adapter.NativeIdStorage = nativeIdStorage;

				try
				{
					nativeIdStorage.Init();
				}
				catch (Exception ex)
				{
					MessageBox.Show(this.GetWindow(), ex.ToString());
				}
			}

			if (Connector.StorageAdapter != null)
			{
				LoggingHelper.DoWithLog(ServicesRegistry.EntityRegistry.Init);
				LoggingHelper.DoWithLog(ServicesRegistry.ExchangeInfoProvider.Init);

				Connector.Adapter.StorageSettings.DaysLoad = TimeSpan.FromDays(3);
				Connector.Adapter.StorageSettings.Mode = StorageModes.Snapshot;
				Connector.LookupAll();

				Connector.SnapshotRegistry.Init();
			}

			ConfigManager.RegisterService<IMessageAdapterProvider>(new FullInMemoryMessageAdapterProvider(Connector.Adapter.InnerAdapters));

			try
			{
				if (_settingsFile.IsConfigExists())
				{
					var ctx = new ContinueOnExceptionContext();
					ctx.Error += ex => ex.LogError();

					using (ctx.ToScope())
						Connector.LoadIfNotNull(_settingsFile.Deserialize<SettingsStorage>());
				}
			}
			catch
			{
			}
		}

		private void SettingsClick(object sender, RoutedEventArgs e)
		{
			if (Connector.Configure(this.GetWindow()))
				Connector.Save().Serialize(_settingsFile);
		}

		private void ConnectClick(object sender, RoutedEventArgs e)
		{
			if (!_isConnected)
			{
				Connector.Connect();
			}
			else
			{
				Connector.Disconnect();
			}
		}

		private void ChangeConnectStatus(bool isConnected)
		{
			_isConnected = isConnected;
			ConnectBtn.Content = isConnected ? LocalizedStrings.Disconnect : LocalizedStrings.Connect;
		}

		private void ThemeSwitchClick(object sender, RoutedEventArgs e)
		{
			ThemeExtensions.Invert();
		}

		private void ShowPortfoliosClick(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_portfoliosWindow);
		}

		private void ShowOrdersClick(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_ordersWindow);
		}

		private void ShowTradesClick(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_tradesWindow);
		}

		private void ShowMyTradesClick(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_myTradesWindow);
		}

		private void ShowOrderLogClick(object sender, RoutedEventArgs e)
		{
			ShowOrHide(_orderLogWindow);
		}

		private static void ShowOrHide(Window window)
		{
			if (window == null)
				throw new ArgumentNullException(nameof(window));

			if (window.Visibility == Visibility.Visible)
				window.Hide();
			else
				window.Show();
		}
	}
}