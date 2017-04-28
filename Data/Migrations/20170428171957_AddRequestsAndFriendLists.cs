using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SkateDate.Data.Migrations
{
    public partial class AddRequestsAndFriendLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendLists",
                columns: table => new
                {
                    OwnerID = table.Column<string>(maxLength: 256, nullable: false),
                    FriendID = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLists", x => new { x.OwnerID, x.FriendID });
                });

            migrationBuilder.CreateTable(
                name: "FriendRequestLists",
                columns: table => new
                {
                    OwnerID = table.Column<string>(maxLength: 256, nullable: false),
                    RequesterID = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequestLists", x => new { x.OwnerID, x.RequesterID });
                });

            migrationBuilder.CreateTable(
                name: "FriendRequestsUserMades",
                columns: table => new
                {
                    OwnerID = table.Column<string>(maxLength: 256, nullable: false),
                    RequesteeID = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequestsUserMades", x => new { x.OwnerID, x.RequesteeID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendLists");

            migrationBuilder.DropTable(
                name: "FriendRequestLists");

            migrationBuilder.DropTable(
                name: "FriendRequestsUserMades");
        }
    }
}
