using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSchemaIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.EnsureSchema(
                name: "HumanResource");

            migrationBuilder.CreateTable(
                name: "ApplicationController",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationController", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationEndPoint",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsAction = table.Column<bool>(type: "bit", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Platform = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationEndPoint", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRole",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    SVersion = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "CONVERT(NVARCHAR(MAX), CONVERT(BINARY(8), Version), 1)"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "HumanResource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)"),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationControllerMethod",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ApplicationControllerId = table.Column<int>(type: "int", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationControllerMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationControllerMethod_ApplicationController_ApplicationControllerId",
                        column: x => x.ApplicationControllerId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationController",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationNavigation",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApplicationControllerId = table.Column<int>(type: "int", nullable: true),
                    Index = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)"),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationNavigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationNavigation_ApplicationController_ApplicationControllerId",
                        column: x => x.ApplicationControllerId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationController",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: true),
                    ProfilePicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsAzureUser = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)"),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    SVersion = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "CONVERT(NVARCHAR(MAX), CONVERT(BINARY(8), Version), 1)"),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PasswordOwn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HumanResource",
                        principalTable: "Employee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationRoleClaim",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationControllerMethodId = table.Column<int>(type: "int", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationRoleClaim_ApplicationControllerMethod_ApplicationControllerMethodId",
                        column: x => x.ApplicationControllerMethodId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationControllerMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationRoleClaim_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationNavigationRole",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationNavigationId = table.Column<int>(type: "int", nullable: false),
                    ApplicationRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)"),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationNavigationRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationNavigationRole_ApplicationNavigation_ApplicationNavigationId",
                        column: x => x.ApplicationNavigationId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationNavigation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationNavigationRole_ApplicationRole_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserApproval",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "date", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)"),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserApproval_ApplicationRole_ApplicationRoleId",
                        column: x => x.ApplicationRoleId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUserApproval_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserClaim",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationControllerMethodId = table.Column<int>(type: "int", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserClaim_ApplicationControllerMethod_ApplicationControllerMethodId",
                        column: x => x.ApplicationControllerMethodId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationControllerMethod",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUserClaim_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserCompany",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCompany_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUserCompany_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Organization",
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserInfo",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LoginDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppVersion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserInfo_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserLogin",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_ApplicationUserLogin_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRole",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SVersion = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "CONVERT(NVARCHAR(MAX), CONVERT(BINARY(8), Version), 1)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole_ApplicationRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationRole",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUserRole_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserToken",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_ApplicationUserToken_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserApprovalDetail",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserApprovalId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)"),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserApprovalDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserApprovalDetail_ApplicationUserApproval_ApplicationUserApprovalId",
                        column: x => x.ApplicationUserApprovalId,
                        principalSchema: "Identity",
                        principalTable: "ApplicationUserApproval",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserApprovalDetail_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "Organization",
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationController_Name",
                schema: "Identity",
                table: "ApplicationController",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationControllerMethod_ApplicationControllerId",
                schema: "Identity",
                table: "ApplicationControllerMethod",
                column: "ApplicationControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationControllerMethod_Name_ApplicationControllerId",
                schema: "Identity",
                table: "ApplicationControllerMethod",
                columns: new[] { "Name", "ApplicationControllerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationEndPoint_Name",
                schema: "Identity",
                table: "ApplicationEndPoint",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationNavigation_ApplicationControllerId",
                schema: "Identity",
                table: "ApplicationNavigation",
                column: "ApplicationControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationNavigation_Code",
                schema: "Identity",
                table: "ApplicationNavigation",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationNavigation_Name_Icon_Url_ParentCode",
                schema: "Identity",
                table: "ApplicationNavigation",
                columns: new[] { "Name", "Icon", "Url", "ParentCode" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationNavigationRole_ApplicationNavigationId",
                schema: "Identity",
                table: "ApplicationNavigationRole",
                column: "ApplicationNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationNavigationRole_ApplicationRoleId",
                schema: "Identity",
                table: "ApplicationNavigationRole",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "ApplicationRole",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleClaim_ApplicationControllerMethodId",
                schema: "Identity",
                table: "ApplicationRoleClaim",
                column: "ApplicationControllerMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationRoleClaim_RoleId_ApplicationControllerMethodId",
                schema: "Identity",
                table: "ApplicationRoleClaim",
                columns: new[] { "RoleId", "ApplicationControllerMethodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "ApplicationUser",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_EmployeeId_UserName",
                schema: "Identity",
                table: "ApplicationUser",
                columns: new[] { "EmployeeId", "UserName" },
                unique: true,
                filter: "[EmployeeId] IS NOT NULL AND [UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "ApplicationUser",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserApproval_ApplicationRoleId",
                schema: "Identity",
                table: "ApplicationUserApproval",
                column: "ApplicationRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserApproval_UserId",
                schema: "Identity",
                table: "ApplicationUserApproval",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserApprovalDetail_ApplicationUserApprovalId",
                schema: "Identity",
                table: "ApplicationUserApprovalDetail",
                column: "ApplicationUserApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserApprovalDetail_CompanyId",
                schema: "Identity",
                table: "ApplicationUserApprovalDetail",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserClaim_ApplicationControllerMethodId",
                schema: "Identity",
                table: "ApplicationUserClaim",
                column: "ApplicationControllerMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserClaim_UserId_ApplicationControllerMethodId",
                schema: "Identity",
                table: "ApplicationUserClaim",
                columns: new[] { "UserId", "ApplicationControllerMethodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCompany_ApplicationUserId_CompanyId",
                schema: "Identity",
                table: "ApplicationUserCompany",
                columns: new[] { "ApplicationUserId", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCompany_CompanyId",
                schema: "Identity",
                table: "ApplicationUserCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserInfo_ApplicationUserId_IpAddress_DeviceName",
                schema: "Identity",
                table: "ApplicationUserInfo",
                columns: new[] { "ApplicationUserId", "IpAddress", "DeviceName" });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserLogin_UserId",
                schema: "Identity",
                table: "ApplicationUserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRole_RoleId",
                schema: "Identity",
                table: "ApplicationUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Code",
                schema: "HumanResource",
                table: "Employee",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_Name",
                schema: "HumanResource",
                table: "Employee",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationEndPoint",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationNavigationRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationRoleClaim",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserApprovalDetail",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserClaim",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserCompany",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserInfo",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserLogin",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserToken",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationNavigation",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUserApproval",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationControllerMethod",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationRole",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationUser",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationController",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "HumanResource");
        }
    }
}
