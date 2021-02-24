using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BazarJok.DataAccess.Domain.Migrations
{
    public partial class ViewCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Blogs_BlogId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Blogs_BlogId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_BlogId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Categories_BlogId",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("070e9e94-d732-4350-87c0-e48b772e43d8"));

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Categories");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "Blogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Blogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "PasswordHash", "Role" },
                values: new object[] { new Guid("c87568c6-674a-4175-8551-4a677d2813c5"), new DateTime(2021, 2, 22, 15, 57, 55, 847, DateTimeKind.Local).AddTicks(5859), "Zulu", "$2a$11$a2G0ztajcFKlg.2pLuntoO0mhkUZIgzJSj1dD9vmMt5J8ozqWh1Rq", (byte)5 });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AuthorId",
                table: "Blogs",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CategoryId",
                table: "Blogs",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Admins_AuthorId",
                table: "Blogs",
                column: "AuthorId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Categories_CategoryId",
                table: "Blogs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Admins_AuthorId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Categories_CategoryId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AuthorId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_CategoryId",
                table: "Blogs");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("c87568c6-674a-4175-8551-4a677d2813c5"));

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Blogs");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BlogId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "PasswordHash", "Role" },
                values: new object[] { new Guid("070e9e94-d732-4350-87c0-e48b772e43d8"), new DateTime(2021, 2, 2, 16, 45, 34, 220, DateTimeKind.Local), "dev", "$2a$11$tyCiVXTnJvBYG4XSFe9oNuPcG9fdwVEehmTlB7JBoBn/G.jpdFpR6", (byte)5 });

            migrationBuilder.CreateIndex(
                name: "IX_Images_BlogId",
                table: "Images",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BlogId",
                table: "Categories",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Blogs_BlogId",
                table: "Categories",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Blogs_BlogId",
                table: "Images",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
