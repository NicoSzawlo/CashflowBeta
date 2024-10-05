using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashflowBeta.Migrations
{
    /// <inheritdoc />
    public partial class ExtendTransactionPartnerWithParentChildRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentPartnerID",
                table: "TransactionsPartners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsPartners_ParentPartnerID",
                table: "TransactionsPartners",
                column: "ParentPartnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsPartners_TransactionsPartners_ParentPartnerID",
                table: "TransactionsPartners",
                column: "ParentPartnerID",
                principalTable: "TransactionsPartners",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionsPartners_TransactionsPartners_ParentPartnerID",
                table: "TransactionsPartners");

            migrationBuilder.DropIndex(
                name: "IX_TransactionsPartners_ParentPartnerID",
                table: "TransactionsPartners");

            migrationBuilder.DropColumn(
                name: "ParentPartnerID",
                table: "TransactionsPartners");
        }
    }
}
