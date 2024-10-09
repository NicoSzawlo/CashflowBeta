using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashflowBeta.Migrations
{
    /// <inheritdoc />
    public partial class FixedExtensionWithNotNullforPartner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionsPartners_TransactionsPartners_ParentPartnerID",
                table: "TransactionsPartners");

            migrationBuilder.AlterColumn<int>(
                name: "ParentPartnerID",
                table: "TransactionsPartners",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsPartners_TransactionsPartners_ParentPartnerID",
                table: "TransactionsPartners",
                column: "ParentPartnerID",
                principalTable: "TransactionsPartners",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionsPartners_TransactionsPartners_ParentPartnerID",
                table: "TransactionsPartners");

            migrationBuilder.AlterColumn<int>(
                name: "ParentPartnerID",
                table: "TransactionsPartners",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionsPartners_TransactionsPartners_ParentPartnerID",
                table: "TransactionsPartners",
                column: "ParentPartnerID",
                principalTable: "TransactionsPartners",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
