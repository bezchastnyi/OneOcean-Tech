using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VesselNavigationAPI.Migrations
{
    /// <inheritdoc />
    public partial class VesselNavigationAPI_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vessel",
                columns: table => new
                {
                    VesselId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vessel", x => x.VesselId);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    VesselPositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    VesselId = table.Column<Guid>(type: "uuid", nullable: false),
                    X = table.Column<int>(type: "integer", nullable: false),
                    Y = table.Column<int>(type: "integer", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.VesselPositionId);
                    table.ForeignKey(
                        name: "FK_Position_Vessel_VesselId",
                        column: x => x.VesselId,
                        principalTable: "Vessel",
                        principalColumn: "VesselId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Position_VesselId",
                table: "Position",
                column: "VesselId");

            migrationBuilder.CreateIndex(
                name: "IX_Vessel_Name",
                table: "Vessel",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Vessel");
        }
    }
}
