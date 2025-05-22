using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowEngine.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNodeConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "NodeConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FromNodeInstanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FromPortName = table.Column<string>(type: "TEXT", nullable: false),
                    ToNodeInstanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ToPortName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeConnections_NodeInstances_FromNodeInstanceId",
                        column: x => x.FromNodeInstanceId,
                        principalTable: "NodeInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeConnections_NodeInstances_ToNodeInstanceId",
                        column: x => x.ToNodeInstanceId,
                        principalTable: "NodeInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NodeConnections_FromNodeInstanceId",
                table: "NodeConnections",
                column: "FromNodeInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeConnections_ToNodeInstanceId",
                table: "NodeConnections",
                column: "ToNodeInstanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NodeConnections");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
