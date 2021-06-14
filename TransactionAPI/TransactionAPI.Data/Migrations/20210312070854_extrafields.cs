using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransactionAPI.Data.Migrations
{
    public partial class extrafields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "FK_TransactionCategoryId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PostDate",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "TransactionCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Deleted = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    Modified = table.Column<DateTimeOffset>(nullable: true),
                    ModifiedBy = table.Column<Guid>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    RecordSource = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FK_TransactionCategoryId",
                table: "Transactions",
                column: "FK_TransactionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionCategories_FK_TransactionCategoryId",
                table: "Transactions",
                column: "FK_TransactionCategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionCategories_FK_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "TransactionCategories");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_FK_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FK_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PostDate",
                table: "Transactions");
        }
    }
}
