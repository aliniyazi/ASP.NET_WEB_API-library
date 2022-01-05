using Microsoft.EntityFrameworkCore.Migrations;

namespace API.DataAccess.Migrations
{
    public partial class ChangeBookModelPropertyFromImageUrlToImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Books",
                newName: "ImageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Books",
                newName: "ImageUrl");
        }
    }
}
