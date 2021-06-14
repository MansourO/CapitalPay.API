using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionAPI.Data.Migrations
{
    public partial class tcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TransactionCategories",
                columns: new[] { "Id", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "IsDeleted", "Modified", "ModifiedBy", "Name", "RecordSource" },
                values: new object[] { new Guid("ebdc7b31-b159-4c14-88c8-554bc504b4f7"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "A category to group all utility transactions", false, null, null, "Utilities", null });

            migrationBuilder.InsertData(
                table: "TransactionCategories",
                columns: new[] { "Id", "Created", "CreatedBy", "Deleted", "DeletedBy", "Description", "IsDeleted", "Modified", "ModifiedBy", "Name", "RecordSource" },
                values: new object[] { new Guid("9a9e48e7-7cb7-4dcf-b7b9-afec6e38aa45"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, null, "A category to group all food transactions", false, null, null, "Food", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: new Guid("9a9e48e7-7cb7-4dcf-b7b9-afec6e38aa45"));

            migrationBuilder.DeleteData(
                table: "TransactionCategories",
                keyColumn: "Id",
                keyValue: new Guid("ebdc7b31-b159-4c14-88c8-554bc504b4f7"));
        }
    }
}
