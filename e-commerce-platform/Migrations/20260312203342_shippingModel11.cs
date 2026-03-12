using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce_platform.Migrations
{
    /// <inheritdoc />
    public partial class shippingModel11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Step",
                table: "ShippingStatuses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Step",
                table: "ShippingStatuses");
        }
    }
}
