using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetCoreAPIwithAuthentication.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderMasters",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: true),
                    IsComplete = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMasters", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    PlantId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.PlantId);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    DetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: false),
                    PlantId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderMasters_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderMasters",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OrderMasters",
                columns: new[] { "OrderId", "CustomerName", "ImagePath", "IsComplete", "OrderDate" },
                values: new object[,]
                {
                    { 1, "Moin Khan", null, true, new DateTime(2024, 6, 4, 9, 33, 32, 242, DateTimeKind.Local).AddTicks(4997) },
                    { 2, "Shorob Ali", null, false, new DateTime(2024, 6, 3, 9, 33, 32, 243, DateTimeKind.Local).AddTicks(5544) }
                });

            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "PlantId", "Name" },
                values: new object[,]
                {
                    { 1, "Mango" },
                    { 2, "Jasmine" },
                    { 3, "Aeromatic Jui" }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "DetailId", "OrderId", "PlantId", "Price", "Quantity" },
                values: new object[] { 1, 1, 1, 255m, 1 });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "DetailId", "OrderId", "PlantId", "Price", "Quantity" },
                values: new object[] { 2, 1, 2, 165m, 2 });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "DetailId", "OrderId", "PlantId", "Price", "Quantity" },
                values: new object[] { 3, 2, 3, 400m, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_PlantId",
                table: "OrderDetails",
                column: "PlantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderMasters");

            migrationBuilder.DropTable(
                name: "Plants");
        }
    }
}
