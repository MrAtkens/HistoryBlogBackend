using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BazarJok.DataAccess.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Image_ImageId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Blogs_BlogId",
                table: "Image");

            migrationBuilder.DropTable(
                name: "KeyWord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("95179b4a-be59-4301-a8e7-518bec89a362"));

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "Images");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Images",
                newName: "ImageName");

            migrationBuilder.RenameIndex(
                name: "IX_Image_BlogId",
                table: "Images",
                newName: "IX_Images_BlogId");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "PasswordHash", "Role" },
                values: new object[] { new Guid("070e9e94-d732-4350-87c0-e48b772e43d8"), new DateTime(2021, 2, 2, 16, 45, 34, 220, DateTimeKind.Local), "dev", "$2a$11$tyCiVXTnJvBYG4XSFe9oNuPcG9fdwVEehmTlB7JBoBn/G.jpdFpR6", (byte)5 });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ImageId",
                table: "Categories",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Images_ImageId",
                table: "Blogs",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories",
                column: "ImageId",
                principalTable: "Images",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Images_ImageId",
                table: "Blogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Blogs_BlogId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ImageId",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("070e9e94-d732-4350-87c0-e48b772e43d8"));

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Blogs");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Image",
                newName: "ImagePath");

            migrationBuilder.RenameIndex(
                name: "IX_Images_BlogId",
                table: "Image",
                newName: "IX_Image_BlogId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "KeyWord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyWord_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "PasswordHash", "Role" },
                values: new object[] { new Guid("95179b4a-be59-4301-a8e7-518bec89a362"), new DateTime(2021, 1, 12, 11, 42, 43, 130, DateTimeKind.Local).AddTicks(3482), "dev", "$2a$11$gn7/tx9l6NtWD2yUD6tETOCX7A.JVOr8e2nwWinGNZ8xnijhVxao2", (byte)5 });

            migrationBuilder.CreateIndex(
                name: "IX_KeyWord_BlogId",
                table: "KeyWord",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Image_ImageId",
                table: "Blogs",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Blogs_BlogId",
                table: "Image",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
