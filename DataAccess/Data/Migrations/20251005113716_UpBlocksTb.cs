using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpBlocksTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArContent",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ArDescription",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ArLine1",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ArLine2",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ArName",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ArPicture",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ArYoutubeVideo",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "EnLine1",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "EnLine2",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "EnYoutubeVideo",
                table: "Blocks",
                newName: "Line");

            migrationBuilder.RenameColumn(
                name: "EnPicture",
                table: "Blocks",
                newName: "Picture");

            migrationBuilder.RenameColumn(
                name: "EnName",
                table: "Blocks",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "EnDescription",
                table: "Blocks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "EnContent",
                table: "Blocks",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "HPImage",
                table: "profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DImage",
                table: "profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "lastestNews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "profiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOnUtc", "UpdatedOnUtc" },
                values: new object[] { new DateTime(2025, 10, 5, 11, 37, 13, 7, DateTimeKind.Utc).AddTicks(581), new DateTime(2025, 10, 5, 11, 37, 13, 7, DateTimeKind.Utc).AddTicks(586) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Blocks",
                newName: "EnName");

            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Blocks",
                newName: "EnPicture");

            migrationBuilder.RenameColumn(
                name: "Line",
                table: "Blocks",
                newName: "EnYoutubeVideo");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Blocks",
                newName: "EnDescription");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Blocks",
                newName: "EnContent");

            migrationBuilder.AlterColumn<string>(
                name: "HPImage",
                table: "profiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DImage",
                table: "profiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "lastestNews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ArContent",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArDescription",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArLine1",
                table: "Blocks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArLine2",
                table: "Blocks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArName",
                table: "Blocks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArPicture",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArYoutubeVideo",
                table: "Blocks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnLine1",
                table: "Blocks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnLine2",
                table: "Blocks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "profiles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOnUtc", "UpdatedOnUtc" },
                values: new object[] { new DateTime(2025, 10, 3, 1, 8, 1, 369, DateTimeKind.Utc).AddTicks(2002), new DateTime(2025, 10, 3, 1, 8, 1, 369, DateTimeKind.Utc).AddTicks(2006) });
        }
    }
}
