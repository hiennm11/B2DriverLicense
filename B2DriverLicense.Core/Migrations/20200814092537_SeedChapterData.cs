using Microsoft.EntityFrameworkCore.Migrations;

namespace B2DriverLicense.Core.Migrations
{
    public partial class SeedChapterData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Chapters",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Khái niệm và quy tắc giao thông đường bộ" },
                    { 2, "Nghiệp vụ vận tải" },
                    { 3, "Văn hóa, đạo đức người lái xe" },
                    { 4, "Kỹ thuật lái xe" },
                    { 5, "Cấu tạo và sửa chữa xe" },
                    { 6, "Biển báo hiệu đường bộ" },
                    { 7, "Giải các thế sa hình và kỹ năng xử lý tình huống giao thông" },
                    { 8, "Câu hỏi điểm liệt" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Chapters",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
