using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_custom_full_text_search : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_jobs_jobs_title_description_fts",
                schema: "jobs",
                table: "jobs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_jobs_jobs_title_description_fts",
                schema: "jobs",
                table: "jobs",
                column: "id")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }
    }
}
