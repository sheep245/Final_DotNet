using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point_Of_Sales.Migrations
{
    /// <inheritdoc />
    public partial class Update_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_PurchaseHistories_PurchaseHistoryId",
                table: "PurchaseDetails");

            /*migrationBuilder.DropIndex(
                name: "IX_Inventories_RetailStoreId",
                table: "Inventories");*/

            migrationBuilder.DropColumn(
                name: "pdetail_ID",
                table: "PurchaseDetails");

            migrationBuilder.RenameColumn(
                name: "pID",
                table: "PurchaseHistories",
                newName: "purchaseId");

            migrationBuilder.RenameColumn(
                name: "PurchaseHistoryId",
                table: "PurchaseDetails",
                newName: "PurchaseId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetails_PurchaseHistoryId",
                table: "PurchaseDetails",
                newName: "IX_PurchaseDetails_PurchaseId");

            migrationBuilder.AddColumn<int>(
                name: "InventoryId",
                table: "RetailStores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "PurchaseDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            /*migrationBuilder.CreateIndex(
                name: "IX_Inventories_RetailStoreId",
                table: "Inventories",
                column: "RetailStoreId",
                unique: true);*/

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_PurchaseHistories_PurchaseId",
                table: "PurchaseDetails",
                column: "PurchaseId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseDetails_PurchaseHistories_PurchaseId",
                table: "PurchaseDetails");

            /*migrationBuilder.DropIndex(
                name: "IX_Inventories_RetailStoreId",
                table: "Inventories");*/

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "RetailStores");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "PurchaseDetails");

            migrationBuilder.RenameColumn(
                name: "purchaseId",
                table: "PurchaseHistories",
                newName: "pID");

            migrationBuilder.RenameColumn(
                name: "PurchaseId",
                table: "PurchaseDetails",
                newName: "PurchaseHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseDetails_PurchaseId",
                table: "PurchaseDetails",
                newName: "IX_PurchaseDetails_PurchaseHistoryId");

            migrationBuilder.AddColumn<string>(
                name: "pdetail_ID",
                table: "PurchaseDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            /*migrationBuilder.CreateIndex(
                name: "IX_Inventories_RetailStoreId",
                table: "Inventories",
                column: "RetailStoreId");*/

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseDetails_PurchaseHistories_PurchaseHistoryId",
                table: "PurchaseDetails",
                column: "PurchaseHistoryId",
                principalTable: "PurchaseHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
