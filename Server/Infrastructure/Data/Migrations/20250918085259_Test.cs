using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teacher_Classrooms",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomStudents",
                columns: table => new
                {
                    ClassroomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomStudents", x => new { x.ClassroomId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_ClassroomStudents_Classrooms",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomStudents_Students",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classroom_Exercises",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercise_Ratings",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Student_Ratings",
                        column: x => x.AppUserId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Email", "Name", "Surname" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "test1@email.com", "User1", "Surname1" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "test2@email.com", "User2", "Surname2" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "test3@email.com", "User3", "Surname3" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "test4@email.com", "User4", "Surname4" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "test5@email.com", "User5", "Surname5" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "test6@email.com", "User6", "Surname6" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "test7@email.com", "User7", "Surname7" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "test8@email.com", "User8", "Surname8" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "test9@email.com", "User9", "Surname9" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "test10@email.com", "User10", "Surname10" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), "test11@email.com", "User11", "Surname11" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), "test12@email.com", "User12", "Surname12" },
                    { new Guid("00000000-0000-0000-0000-000000000013"), "test13@email.com", "User13", "Surname13" },
                    { new Guid("00000000-0000-0000-0000-000000000014"), "test14@email.com", "User14", "Surname14" },
                    { new Guid("00000000-0000-0000-0000-000000000015"), "test15@email.com", "User15", "Surname15" },
                    { new Guid("00000000-0000-0000-0000-000000000016"), "test16@email.com", "User16", "Surname16" },
                    { new Guid("00000000-0000-0000-0000-000000000017"), "test17@email.com", "User17", "Surname17" },
                    { new Guid("00000000-0000-0000-0000-000000000018"), "test18@email.com", "User18", "Surname18" },
                    { new Guid("00000000-0000-0000-0000-000000000019"), "test19@email.com", "User19", "Surname19" },
                    { new Guid("00000000-0000-0000-0000-000000000020"), "test20@email.com", "User20", "Surname20" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Email", "Name", "Surname" },
                values: new object[,]
                {
                    { new Guid("d290f1ee-6c54-4b01-90e6-d701748f0851"), "teacher1@email.com", "John", "Doe" },
                    { new Guid("d290f1ee-6c54-4b01-90e6-d701748f0852"), "teacher2@email.com", "Jane", "Smith" }
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name", "TeacherId" },
                values: new object[,]
                {
                    { new Guid("d291f1ee-6c54-4b01-90e6-d701748f0851"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Classroom 1", new Guid("d290f1ee-6c54-4b01-90e6-d701748f0851") },
                    { new Guid("d292f1ee-6c54-4b01-90e6-d701748f0851"), new DateTime(2022, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Classroom 2", new Guid("d290f1ee-6c54-4b01-90e6-d701748f0851") },
                    { new Guid("d293f1ee-6c54-4b01-90e6-d701748f0851"), new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Classroom 3", new Guid("d290f1ee-6c54-4b01-90e6-d701748f0851") },
                    { new Guid("d294f1ee-6c54-4b01-90e6-d701748f0851"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Classroom 4", new Guid("d290f1ee-6c54-4b01-90e6-d701748f0852") },
                    { new Guid("d295f1ee-6c54-4b01-90e6-d701748f0851"), new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Classroom 5", new Guid("d290f1ee-6c54-4b01-90e6-d701748f0852") }
                });

            migrationBuilder.InsertData(
                table: "ClassroomStudents",
                columns: new[] { "ClassroomId", "StudentId" },
                values: new object[,]
                {
                    { new Guid("d291f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("d291f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000006") },
                    { new Guid("d291f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000011") },
                    { new Guid("d291f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000016") },
                    { new Guid("d292f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("d292f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000007") },
                    { new Guid("d292f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000012") },
                    { new Guid("d292f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000017") },
                    { new Guid("d293f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000003") },
                    { new Guid("d293f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000008") },
                    { new Guid("d293f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000013") },
                    { new Guid("d293f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000018") },
                    { new Guid("d294f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000004") },
                    { new Guid("d294f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000009") },
                    { new Guid("d294f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000014") },
                    { new Guid("d294f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000019") },
                    { new Guid("d295f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000005") },
                    { new Guid("d295f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000010") },
                    { new Guid("d295f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000015") },
                    { new Guid("d295f1ee-6c54-4b01-90e6-d701748f0851"), new Guid("00000000-0000-0000-0000-000000000020") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_TeacherId",
                table: "Classrooms",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomStudents_StudentId",
                table: "ClassroomStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ClassroomId",
                table: "Exercises",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AppUserId",
                table: "Ratings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ExerciseId",
                table: "Ratings",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClassroomStudents");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
