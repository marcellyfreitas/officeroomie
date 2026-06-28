using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeRoomie.Migrations
{
    /// <inheritdoc />
    public partial class M01initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administradores",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    senha = table.Column<string>(type: "TEXT", nullable: false),
                    cpf = table.Column<string>(type: "TEXT", nullable: true),
                    permissoes = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<string>(type: "TEXT", nullable: true),
                    updated_at = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administradores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    cpf = table.Column<string>(type: "TEXT", nullable: true),
                    endereco_logradouro = table.Column<string>(type: "TEXT", nullable: true),
                    endereco_numero = table.Column<string>(type: "TEXT", nullable: true),
                    endereco_complemento = table.Column<string>(type: "TEXT", nullable: true),
                    endereco_cep = table.Column<string>(type: "TEXT", nullable: true),
                    endereco_bairro = table.Column<string>(type: "TEXT", nullable: true),
                    endereco_cidade = table.Column<string>(type: "TEXT", nullable: true),
                    endereco_estado = table.Column<string>(type: "TEXT", nullable: true),
                    endereco_pais = table.Column<string>(type: "TEXT", nullable: true),
                    created_at = table.Column<string>(type: "TEXT", nullable: true),
                    updated_at = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "salas",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    nome = table.Column<string>(type: "TEXT", nullable: false),
                    descricao = table.Column<string>(type: "TEXT", nullable: false),
                    capacidade = table.Column<string>(type: "TEXT", nullable: false),
                    categoria = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<string>(type: "TEXT", nullable: true),
                    updated_at = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cartoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    numeroDoCartao = table.Column<string>(type: "TEXT", nullable: false),
                    nomeDoTitular = table.Column<string>(type: "TEXT", nullable: false),
                    validade = table.Column<DateTime>(type: "TEXT", nullable: false),
                    cvv = table.Column<int>(type: "INTEGER", nullable: false),
                    cliente_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_at = table.Column<string>(type: "TEXT", nullable: true),
                    updated_at = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cartoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_cartoes_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    hora_inicio = table.Column<string>(type: "TEXT", nullable: false),
                    hora_fim = table.Column<string>(type: "TEXT", nullable: false),
                    data_reserva = table.Column<string>(type: "TEXT", nullable: false),
                    protocolo = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    sala_id = table.Column<int>(type: "INTEGER", nullable: false),
                    cliente_id = table.Column<int>(type: "INTEGER", nullable: false),
                    cartao_id = table.Column<int>(type: "INTEGER", nullable: true),
                    created_at = table.Column<string>(type: "TEXT", nullable: true),
                    updated_at = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservas_cartoes_cartao_id",
                        column: x => x.cartao_id,
                        principalTable: "cartoes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_reservas_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservas_salas_sala_id",
                        column: x => x.sala_id,
                        principalTable: "salas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cartoes_cliente_id",
                table: "cartoes",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_cartao_id",
                table: "reservas",
                column: "cartao_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_cliente_id",
                table: "reservas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_sala_id",
                table: "reservas",
                column: "sala_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administradores");

            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "cartoes");

            migrationBuilder.DropTable(
                name: "salas");

            migrationBuilder.DropTable(
                name: "clientes");
        }
    }
}
