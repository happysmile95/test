using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SoldTo = table.Column<string>(nullable: false),
                    CustName = table.Column<string>(nullable: false),
                    ShipTo = table.Column<string>(nullable: false),
                    ShipToNa = table.Column<string>(nullable: false),
                    OrderType = table.Column<string>(nullable: false),
                    Dv = table.Column<string>(nullable: false),
                    OrderNum = table.Column<string>(nullable: false),
                    Material = table.Column<string>(nullable: false),
                    MatDes = table.Column<string>(nullable: false),
                    Size = table.Column<string>(nullable: false),
                    AltSize = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Id);
                }); ;
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Goods");
        }
    }
}
