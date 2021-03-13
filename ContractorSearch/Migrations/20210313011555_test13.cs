using Microsoft.EntityFrameworkCore.Migrations;

namespace ContractorSearch.Migrations
{
    public partial class test13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55e80225-6d3e-4911-9c7d-a4e76c6e158d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c6652635-d0cd-4051-a875-c67e2771f435");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba53a2a9-ad13-42fe-afbe-4926fc711f8d", "fd8d5f1f-107a-4d1f-ad6f-d3e6d08810e9", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2df4fbcb-4176-4566-8efb-c641f79b8ee2", "6cbbae67-9de8-457d-ba22-b8aff85cf88c", "Contractor", "CONTRACTOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2df4fbcb-4176-4566-8efb-c641f79b8ee2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba53a2a9-ad13-42fe-afbe-4926fc711f8d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "55e80225-6d3e-4911-9c7d-a4e76c6e158d", "76d02165-b392-49ea-9870-30df7e790df2", "Customer", "CUSTOMER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c6652635-d0cd-4051-a875-c67e2771f435", "715c33bb-7df6-40aa-96aa-2365ee9aa3be", "Contractor", "CONTRACTOR" });
        }
    }
}
