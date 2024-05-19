using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class seedQualificationDataToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Qualifications",
                columns: new[] { "Id", "ApplicationUserId", "Course", "EndYear", "Percentage", "StartYear", "University" },
                values: new object[] { 1, "968e7c70-1abe-4d1a-a698-86550cf457f3", "Computer Science", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 56m, new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kerala University" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Qualifications",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
