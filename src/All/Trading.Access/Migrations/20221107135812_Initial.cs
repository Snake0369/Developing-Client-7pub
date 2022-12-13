using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trading.Access.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "trading_cl10_bot");

            migrationBuilder.CreateTable(
                name: "FuturePositions",
                schema: "trading_cl10_bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExchangeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Exchange = table.Column<int>(type: "integer", nullable: true),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    PositionId = table.Column<string>(type: "text", nullable: true),
                    Side = table.Column<int>(type: "integer", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    EntryPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    MarkPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    LiquidationPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    BankruptcyPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Leverage = table.Column<decimal>(type: "numeric", nullable: true),
                    RealizedPnL = table.Column<decimal>(type: "numeric", nullable: true),
                    UnrealizedPnL = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalRealizedPnL = table.Column<decimal>(type: "numeric", nullable: true),
                    TakeProfitPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    WalletBalance = table.Column<decimal>(type: "numeric", nullable: true),
                    PositionStatus = table.Column<int>(type: "integer", nullable: true),
                    MarginType = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuturePositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                schema: "trading_cl10_bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FirstPositionsTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastPositionsTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Current = table.Column<decimal>(type: "numeric", nullable: false),
                    Maximum = table.Column<decimal>(type: "numeric", nullable: false),
                    IsTrade = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Strategies",
                schema: "trading_cl10_bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strategies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equities",
                schema: "trading_cl10_bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StrategyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Instrument = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    DrowDown = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equities_Strategies_StrategyId",
                        column: x => x.StrategyId,
                        principalSchema: "trading_cl10_bot",
                        principalTable: "Strategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "trading_cl10_bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StrategyId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseAsset = table.Column<string>(type: "text", nullable: false),
                    QuoteAsset = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    Side = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    TimestampTakeOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsTakeOn = table.Column<bool>(type: "boolean", nullable: false),
                    TimestampStop = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Stop = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Strategies_StrategyId",
                        column: x => x.StrategyId,
                        principalSchema: "trading_cl10_bot",
                        principalTable: "Strategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subpositions",
                schema: "trading_cl10_bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkSubpositionId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubpositionDirect = table.Column<int>(type: "integer", nullable: false),
                    SubpositionType = table.Column<int>(type: "integer", nullable: false),
                    BaseAsset = table.Column<string>(type: "text", nullable: false),
                    QuoteAsset = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    Side = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false),
                    ClientOrderId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subpositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subpositions_Positions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "trading_cl10_bot",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "trading_cl10_bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    Exchange = table.Column<int>(type: "integer", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubpositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseAsset = table.Column<string>(type: "text", nullable: false),
                    QuoteAsset = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false),
                    Side = table.Column<int>(type: "integer", nullable: false),
                    ClientOrderId = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    AmountRemaining = table.Column<decimal>(type: "numeric", nullable: true),
                    AmountFilled = table.Column<decimal>(type: "numeric", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    IsMargin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Subpositions_SubpositionId",
                        column: x => x.SubpositionId,
                        principalSchema: "trading_cl10_bot",
                        principalTable: "Subpositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trades",
                schema: "trading_cl10_bot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TradeId = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Exchange = table.Column<int>(type: "integer", nullable: false),
                    SubpositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseAsset = table.Column<string>(type: "text", nullable: false),
                    QuoteAsset = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    ClientOrderId = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Fee = table.Column<decimal>(type: "numeric", nullable: false),
                    FeeAsset = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trades_Subpositions_SubpositionId",
                        column: x => x.SubpositionId,
                        principalSchema: "trading_cl10_bot",
                        principalTable: "Subpositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equities_StrategyId",
                schema: "trading_cl10_bot",
                table: "Equities",
                column: "StrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SubpositionId",
                schema: "trading_cl10_bot",
                table: "Orders",
                column: "SubpositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_StrategyId",
                schema: "trading_cl10_bot",
                table: "Positions",
                column: "StrategyId");

            migrationBuilder.CreateIndex(
                name: "IX_Subpositions_PositionId",
                schema: "trading_cl10_bot",
                table: "Subpositions",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trades_SubpositionId",
                schema: "trading_cl10_bot",
                table: "Trades",
                column: "SubpositionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equities",
                schema: "trading_cl10_bot");

            migrationBuilder.DropTable(
                name: "FuturePositions",
                schema: "trading_cl10_bot");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "trading_cl10_bot");

            migrationBuilder.DropTable(
                name: "Returns",
                schema: "trading_cl10_bot");

            migrationBuilder.DropTable(
                name: "Trades",
                schema: "trading_cl10_bot");

            migrationBuilder.DropTable(
                name: "Subpositions",
                schema: "trading_cl10_bot");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "trading_cl10_bot");

            migrationBuilder.DropTable(
                name: "Strategies",
                schema: "trading_cl10_bot");
        }
    }
}
