using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class addConfigurationTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EnName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ArMetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnMetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ArFooterBrief = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnFooterBrief = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Android = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IOS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Youtube = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Tiktok = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Snapchat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DefaultEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DefaultEmailName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DefaultNotificationEmails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SocialPicture = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    WebsiteURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailSender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordEmailSender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: true),
                    UseSSL = table.Column<bool>(type: "bit", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleAnalytics = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GoogleAnalyticsEmail = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SEOScripts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableSubscription = table.Column<bool>(type: "bit", nullable: false),
                    EnableOTP = table.Column<bool>(type: "bit", nullable: false),
                    Tax = table.Column<int>(type: "int", nullable: false),
                    CleanCartAfter = table.Column<int>(type: "int", nullable: false),
                    CleanOrderAfter = table.Column<int>(type: "int", nullable: false),
                    PointsPerOrder = table.Column<int>(type: "int", nullable: false),
                    PayTabs = table.Column<bool>(type: "bit", nullable: false),
                    ApplePay = table.Column<bool>(type: "bit", nullable: false),
                    Mada = table.Column<bool>(type: "bit", nullable: false),
                    STCPay = table.Column<bool>(type: "bit", nullable: false),
                    AnyFailedPaymentReturnToCart = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Hidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configurations");
        }
    }
}
