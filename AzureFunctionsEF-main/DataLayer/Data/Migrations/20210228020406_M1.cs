using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Data.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    School = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentId", "FirstName", "LastName", "School" },
                values: new object[,]
                {
                    { "08418e17-c0b7-4690-a0f9-2ee3582231a9", "Jane", "Smith", "Medicine" },
                    { "bc87159e-26ef-474c-9ddc-b1e05ad4e611", "John", "Fisher", "Engineering" },
                    { "644e4e18-b40c-49d1-9dde-a49512f2d0ed", "Pamela", "Baker", "Food Science" },
                    { "03eb5f61-affd-4aa2-80dd-4f1f1cbd7697", "Peter", "Taylor", "Mining" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
