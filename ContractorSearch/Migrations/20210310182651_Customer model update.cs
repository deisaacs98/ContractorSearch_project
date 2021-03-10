using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractorSearch.Migrations
{
    public partial class Customermodelupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "612d4394-0e07-4bca-be19-536629d652b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a271ec22-0248-4e4e-afec-14dbbf8a6060");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "946153df-6245-4591-a68a-2249ad2cf322", "733fe002-8200-45af-843a-43dd1974a814", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "69e03c83-31ee-47c4-88f9-2ad9d5118d32", "cffed9db-016b-44d7-90cb-0f0f02fc1025", "Contractor", "CONTRACTOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69e03c83-31ee-47c4-88f9-2ad9d5118d32");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "946153df-6245-4591-a68a-2249ad2cf322");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "612d4394-0e07-4bca-be19-536629d652b0", "e9c91ec2-7ebd-49fc-ac37-1688790b89f7", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a271ec22-0248-4e4e-afec-14dbbf8a6060", "7b865975-0a78-4cfa-a471-609e8a042679", "Contractor", "CONTRACTOR" });
        }
    }
}
