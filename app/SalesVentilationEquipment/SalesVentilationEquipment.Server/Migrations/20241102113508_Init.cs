using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesVentilationEquipment.Server.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contractor",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    contact_info = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    price = table.Column<double>(type: "double precision", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    feature = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    address = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    address = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contractor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_sum = table.Column<double>(type: "double precision", nullable: false),
                    discount = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cart_Contractor_contractor_id",
                        column: x => x.contractor_id,
                        principalTable: "Contractor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractorAndStore",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contractor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractorAndStore", x => x.id);
                    table.ForeignKey(
                        name: "FK_ContractorAndStore_Contractor_contractor_id",
                        column: x => x.contractor_id,
                        principalTable: "Contractor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractorAndStore_Store_store_id",
                        column: x => x.store_id,
                        principalTable: "Store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Remains",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remains", x => x.id);
                    table.ForeignKey(
                        name: "FK_Remains_Warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalTable: "Warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remains_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreAndWarehouse",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreAndWarehouse", x => x.id);
                    table.ForeignKey(
                        name: "FK_StoreAndWarehouse_Store_store_id",
                        column: x => x.store_id,
                        principalTable: "Store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreAndWarehouse_Warehouse_warehouse_id",
                        column: x => x.warehouse_id,
                        principalTable: "Warehouse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contractor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                    table.ForeignKey(
                        name: "FK_Order_Cart_cart_id",
                        column: x => x.cart_id,
                        principalTable: "Cart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Contractor_contractor_id",
                        column: x => x.contractor_id,
                        principalTable: "Contractor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInCart",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    unit_price = table.Column<double>(type: "double precision", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInCart", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductInCart_Cart_cart_id",
                        column: x => x.cart_id,
                        principalTable: "Cart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductInCart_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_contractor_id",
                table: "Cart",
                column: "contractor_id");

            migrationBuilder.CreateIndex(
                name: "IX_ContractorAndStore_contractor_id",
                table: "ContractorAndStore",
                column: "contractor_id");

            migrationBuilder.CreateIndex(
                name: "IX_ContractorAndStore_store_id",
                table: "ContractorAndStore",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_cart_id",
                table: "Order",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_contractor_id",
                table: "Order",
                column: "contractor_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInCart_cart_id",
                table: "ProductInCart",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInCart_product_id",
                table: "ProductInCart",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Remains_product_id",
                table: "Remains",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Remains_warehouse_id",
                table: "Remains",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAndWarehouse_store_id",
                table: "StoreAndWarehouse",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAndWarehouse_warehouse_id",
                table: "StoreAndWarehouse",
                column: "warehouse_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractorAndStore");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "ProductInCart");

            migrationBuilder.DropTable(
                name: "Remains");

            migrationBuilder.DropTable(
                name: "StoreAndWarehouse");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Contractor");
        }
    }
}
