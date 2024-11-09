using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VictuzAppMVC.Migrations
{
    /// <inheritdoc />
    public partial class SyncDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activiteiten",
                columns: table => new
                {
                    ActiviteitID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime", nullable: false),
                    MaxDeelnemers = table.Column<int>(type: "int", nullable: true),
                    Beschrijving = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVoorstel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Activite__B14595E035BB1C25", x => x.ActiviteitID);
                });

            migrationBuilder.CreateTable(
                name: "EvenementCategorieën",
                columns: table => new
                {
                    CategorieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategorieNaam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Evenemen__F643AD86180B9053", x => x.CategorieID);
                });

            migrationBuilder.CreateTable(
                name: "Lidmaatschappen",
                columns: table => new
                {
                    LidmaatschapID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Beschrijving = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Lidmaats__27325D7CD2D147F7", x => x.LidmaatschapID);
                });

            migrationBuilder.CreateTable(
                name: "ActiviteitCategorieKoppeling",
                columns: table => new
                {
                    ActiviteitID = table.Column<int>(type: "int", nullable: false),
                    CategorieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Activite__CE21AF388DC1EBB2", x => new { x.ActiviteitID, x.CategorieID });
                    table.ForeignKey(
                        name: "FK__Activitei__Activ__412EB0B6",
                        column: x => x.ActiviteitID,
                        principalTable: "Activiteiten",
                        principalColumn: "ActiviteitID");
                    table.ForeignKey(
                        name: "FK__Activitei__Categ__4222D4EF",
                        column: x => x.CategorieID,
                        principalTable: "EvenementCategorieën",
                        principalColumn: "CategorieID");
                });

            migrationBuilder.CreateTable(
                name: "Gebruikers",
                columns: table => new
                {
                    GebruikerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Wachtwoord = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsBestuurslid = table.Column<bool>(type: "bit", nullable: false),
                    LidmaatschapID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Gebruike__4282ABF5F068F3BC", x => x.GebruikerID);
                    table.ForeignKey(
                        name: "FK__Gebruiker__Lidma__44FF419A",
                        column: x => x.LidmaatschapID,
                        principalTable: "Lidmaatschappen",
                        principalColumn: "LidmaatschapID");
                });

            migrationBuilder.CreateTable(
                name: "Aanmeldingen",
                columns: table => new
                {
                    AanmeldingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GebruikerID = table.Column<int>(type: "int", nullable: false),
                    ActiviteitID = table.Column<int>(type: "int", nullable: false),
                    AanmeldDatum = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Aanmeldi__D705C3B943894FF4", x => x.AanmeldingID);
                    table.ForeignKey(
                        name: "FK__Aanmeldin__Activ__2C3393D0",
                        column: x => x.ActiviteitID,
                        principalTable: "Activiteiten",
                        principalColumn: "ActiviteitID");
                    table.ForeignKey(
                        name: "FK__Aanmeldin__Gebru__2B3F6F97",
                        column: x => x.GebruikerID,
                        principalTable: "Gebruikers",
                        principalColumn: "GebruikerID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aanmeldingen_ActiviteitID",
                table: "Aanmeldingen",
                column: "ActiviteitID");

            migrationBuilder.CreateIndex(
                name: "UQ_Gebruiker_Activiteit",
                table: "Aanmeldingen",
                columns: new[] { "GebruikerID", "ActiviteitID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActiviteitCategorieKoppeling_CategorieID",
                table: "ActiviteitCategorieKoppeling",
                column: "CategorieID");

            migrationBuilder.CreateIndex(
                name: "IX_Gebruikers_LidmaatschapID",
                table: "Gebruikers",
                column: "LidmaatschapID");

            migrationBuilder.CreateIndex(
                name: "UQ__Gebruike__A9D1053470D9A463",
                table: "Gebruikers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aanmeldingen");

            migrationBuilder.DropTable(
                name: "ActiviteitCategorieKoppeling");

            migrationBuilder.DropTable(
                name: "Gebruikers");

            migrationBuilder.DropTable(
                name: "Activiteiten");

            migrationBuilder.DropTable(
                name: "EvenementCategorieën");

            migrationBuilder.DropTable(
                name: "Lidmaatschappen");
        }
    }
}
