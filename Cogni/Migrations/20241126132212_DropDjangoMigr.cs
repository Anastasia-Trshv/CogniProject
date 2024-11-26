using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cogni.Migrations
{
    /// <inheritdoc />
    public partial class DropDjangoMigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mbti_question",
                columns: table => new
                {
                    id_mbti_question = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mbti_question_pkey", x => x.id_mbti_question);
                });

            migrationBuilder.CreateTable(
                name: "mbti_type",
                columns: table => new
                {
                    id_mbti_type = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_of_type = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mbti_type_pkey", x => x.id_mbti_type);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id_role = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_role = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.id_role);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    id_tag = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_tag = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tag_pkey", x => x.id_tag);
                });

            migrationBuilder.CreateTable(
                name: "customuser",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    description = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    email = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    password = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    image = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    type_mbti = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    id_role = table.Column<int>(type: "integer", nullable: false),
                    id_mbti_type = table.Column<int>(type: "integer", nullable: false),
                    last_login = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("customuser_pkey", x => x.id_user);
                    table.ForeignKey(
                        name: "customuser_id_mbti_type_fkey",
                        column: x => x.id_mbti_type,
                        principalTable: "mbti_type",
                        principalColumn: "id_mbti_type");
                    table.ForeignKey(
                        name: "customuser_id_role_fkey",
                        column: x => x.id_role,
                        principalTable: "role",
                        principalColumn: "id_role");
                });

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    id_article = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    article_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    article_body = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    id_user = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("article_pkey", x => x.id_article);
                    table.ForeignKey(
                        name: "article_id_user_fkey",
                        column: x => x.id_user,
                        principalTable: "customuser",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "avatars",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    avatar_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    date_added = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_active = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("avatars_pkey", x => x.id);
                    table.ForeignKey(
                        name: "avatars_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "customuser",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("chats_pkey", x => x.id);
                    table.ForeignKey(
                        name: "chats_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "customuser",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "friends",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    friend_id = table.Column<int>(type: "integer", nullable: false),
                    date_added = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("friends_pkey", x => x.id);
                    table.ForeignKey(
                        name: "friends_friend_id_fkey",
                        column: x => x.friend_id,
                        principalTable: "customuser",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "friends_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "customuser",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    id_post = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    post_body = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    id_user = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("post_pkey", x => x.id_post);
                    table.ForeignKey(
                        name: "post_id_user_fkey",
                        column: x => x.id_user,
                        principalTable: "customuser",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "user_tags",
                columns: table => new
                {
                    id_user_tags = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_tag = table.Column<int>(type: "integer", nullable: false),
                    id_user = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_tags_pkey", x => x.id_user_tags);
                    table.ForeignKey(
                        name: "user_tags_id_tag_fkey",
                        column: x => x.id_tag,
                        principalTable: "tag",
                        principalColumn: "id_tag");
                    table.ForeignKey(
                        name: "user_tags_id_user_fkey",
                        column: x => x.id_user,
                        principalTable: "customuser",
                        principalColumn: "id_user");
                });

            migrationBuilder.CreateTable(
                name: "article_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    article_id = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("article_images_pkey", x => x.id);
                    table.ForeignKey(
                        name: "article_images_article_id_fkey",
                        column: x => x.article_id,
                        principalTable: "article",
                        principalColumn: "id_article",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    avatar_id = table.Column<int>(type: "integer", nullable: false),
                    message_body = table.Column<string>(type: "text", nullable: false),
                    chat_id = table.Column<int>(type: "integer", nullable: false),
                    attachment_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("messages_pkey", x => x.id);
                    table.ForeignKey(
                        name: "messages_avatar_id_fkey",
                        column: x => x.avatar_id,
                        principalTable: "avatars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "messages_chat_id_fkey",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    post_id = table.Column<int>(type: "integer", nullable: true),
                    liked_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "likes_post_id_fkey",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "id_post",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "likes_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "customuser",
                        principalColumn: "id_user",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    post_id = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("post_images_pkey", x => x.id);
                    table.ForeignKey(
                        name: "post_images_post_id_fkey",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "id_post",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_article_id_user",
                table: "article",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_article_images_article_id",
                table: "article_images",
                column: "article_id");

            migrationBuilder.CreateIndex(
                name: "IX_avatars_user_id",
                table: "avatars",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_chats_user_id",
                table: "chats",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "customuser_name_key",
                table: "customuser",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_customuser_id_mbti_type",
                table: "customuser",
                column: "id_mbti_type");

            migrationBuilder.CreateIndex(
                name: "IX_customuser_id_role",
                table: "customuser",
                column: "id_role");

            migrationBuilder.CreateIndex(
                name: "IX_friends_friend_id",
                table: "friends",
                column: "friend_id");

            migrationBuilder.CreateIndex(
                name: "IX_friends_user_id",
                table: "friends",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_likes_post_id",
                table: "likes",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_likes_user_id",
                table: "likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_avatar_id",
                table: "messages",
                column: "avatar_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_chat_id",
                table: "messages",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "IX_post_id_user",
                table: "post",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_post_images_post_id",
                table: "post_images",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_tags_id_tag",
                table: "user_tags",
                column: "id_tag");

            migrationBuilder.CreateIndex(
                name: "IX_user_tags_id_user",
                table: "user_tags",
                column: "id_user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "article_images");

            migrationBuilder.DropTable(
                name: "friends");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "mbti_question");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "post_images");

            migrationBuilder.DropTable(
                name: "user_tags");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "avatars");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "customuser");

            migrationBuilder.DropTable(
                name: "mbti_type");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
