using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication1.Migrations
{
    public partial class haha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currencyWordMatch",
                table: "suplierModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "invoiceDateWordMatch",
                table: "suplierModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "purchOrderNoWordMatch",
                table: "suplierModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "suplierBgNrWordMatch",
                table: "suplierModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "suplierOrcNrWordMatch",
                table: "suplierModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "suplierOrgNrWordMatch",
                table: "suplierModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "suplierPgNrWordMatch",
                table: "suplierModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currencyWordMatch",
                table: "suplierModel");

            migrationBuilder.DropColumn(
                name: "invoiceDateWordMatch",
                table: "suplierModel");

            migrationBuilder.DropColumn(
                name: "purchOrderNoWordMatch",
                table: "suplierModel");

            migrationBuilder.DropColumn(
                name: "suplierBgNrWordMatch",
                table: "suplierModel");

            migrationBuilder.DropColumn(
                name: "suplierOrcNrWordMatch",
                table: "suplierModel");

            migrationBuilder.DropColumn(
                name: "suplierOrgNrWordMatch",
                table: "suplierModel");

            migrationBuilder.DropColumn(
                name: "suplierPgNrWordMatch",
                table: "suplierModel");
        }
    }
}
