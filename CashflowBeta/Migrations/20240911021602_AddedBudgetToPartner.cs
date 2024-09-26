using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashflowBeta.Migrations
{
    /// <inheritdoc />
    public partial class AddedBudgetToPartner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetID",
                table: "TransactionsPartners",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsPartners_BudgetID",
                table: "TransactionsPartners",
                column: "BudgetID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsPartners_Budgets_BudgetID",
                table: "TransactionsPartners",
                column: "BudgetID",
                principalTable: "Budgets",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionsPartners_Budgets_BudgetID",
                table: "TransactionsPartners");

            migrationBuilder.DropIndex(
                name: "IX_TransactionsPartners_BudgetID",
                table: "TransactionsPartners");

            migrationBuilder.DropColumn(
                name: "BudgetID",
                table: "TransactionsPartners");
        }
    }
}
