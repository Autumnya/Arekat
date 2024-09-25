using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arekat.Server.Migrations
{
    /// <inheritdoc />
    public partial class NullableAvator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Avator",
                table: "BaseUser",
                type: "nvarchar(max)",
                maxLength: 16777216,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldMaxLength: 16384);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Avator",
                table: "BaseUser",
                type: "varbinary(max)",
                maxLength: 16384,
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 16777216,
                oldNullable: true);
        }
    }
}
