using Cogni.Authentication;
using Cogni.Database.Context;
using Cogni.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Seeding;

public static class DevelopmentDatabaseSeeder
{
    private const string DefaultPassword = "CogniDemo123!";
    private static readonly Guid DemoChatId = Guid.Parse("8be7208d-0f36-4b3e-b2df-82502f4a5dd0");
    private const string AvatarUrl = "https://placehold.co/160x160/png?text=Cogni+Demo";
    private const string ArticleImageUrl = "https://placehold.co/1200x800/png?text=Cogni+Article";
    private const string PostImageUrl = "https://placehold.co/1200x800/png?text=Cogni+Post";

    public static async Task SeedAsync(CogniDbContext context)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        var roleId = await context.Roles.Where(role => role.NameRole == "User")
            .Select(role => (int?)role.Id).FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("The User role is missing. Apply migrations first.");
        var mbtiId = await context.MbtiTypes.Where(type => type.NameOfType == "ENFP")
            .Select(type => (int?)type.Id).FirstOrDefaultAsync()
            ?? throw new InvalidOperationException("The ENFP type is missing. Apply migrations first.");

        var password = Environment.GetEnvironmentVariable("COGNI_SEED_PASSWORD");
        if (string.IsNullOrWhiteSpace(password)) password = DefaultPassword;

        var firstUser = await GetOrCreateUserAsync(context, "alex.demo@cogni.local", "Demo Alex", "Petrov", roleId, mbtiId, password);
        var secondUser = await GetOrCreateUserAsync(context, "maya.demo@cogni.local", "Demo Maya", "Ivanova", roleId, mbtiId, password);
        await context.SaveChangesAsync();

        await AddAvatarIfMissingAsync(context, firstUser.Id);
        await AddAvatarIfMissingAsync(context, secondUser.Id);
        if (!await context.MbtiQuestions.AnyAsync())
        {
            context.MbtiQuestions.AddRange(
                new MbtiQuestion { Question = "Вы предпочитаете проводить время в компании людей." },
                new MbtiQuestion { Question = "Вам интереснее изучать новые идеи, чем проверенные методы." },
                new MbtiQuestion { Question = "При принятии решений вы чаще опираетесь на логику, чем на чувства." },
                new MbtiQuestion { Question = "Вы предпочитаете заранее планировать дела." });
        }

        var tags = await GetOrCreateTagsAsync(context);
        await context.SaveChangesAsync();
        await AddUserTagIfMissingAsync(context, firstUser.Id, tags[0].Id);
        await AddUserTagIfMissingAsync(context, firstUser.Id, tags[1].Id);
        await AddUserTagIfMissingAsync(context, secondUser.Id, tags[0].Id);
        await AddUserTagIfMissingAsync(context, secondUser.Id, tags[2].Id);

        var firstArticle = await GetOrCreateArticleAsync(context, firstUser, "Cogni demo: first article");
        var secondArticle = await GetOrCreateArticleAsync(context, secondUser, "Cogni demo: second article");
        var firstPost = await GetOrCreatePostAsync(context, firstUser, "A first post from the Cogni demo account.");
        var secondPost = await GetOrCreatePostAsync(context, secondUser, "A second post from the Cogni demo account.");
        await context.SaveChangesAsync();

        await AddArticleImageIfMissingAsync(context, firstArticle.Id);
        await AddArticleImageIfMissingAsync(context, secondArticle.Id);
        await AddPostImageIfMissingAsync(context, firstPost.Id);
        await AddPostImageIfMissingAsync(context, secondPost.Id);
        await AddFriendIfMissingAsync(context, firstUser.Id, secondUser.Id);
        await AddFriendIfMissingAsync(context, secondUser.Id, firstUser.Id);

        var chat = await context.Chats.FirstOrDefaultAsync(item => item.Id == DemoChatId);
        if (chat is null)
        {
            chat = new Chat
            {
                Id = DemoChatId,
                Name = "Cogni demo chat",
                OwnerId = firstUser.Id,
                isDm = false,
                CreatedAt = DateTime.UtcNow,
                Members = new List<ChatMember>()
            };
            context.Chats.Add(chat);
        }

        await context.SaveChangesAsync();
        await AddChatMemberIfMissingAsync(context, chat, firstUser.Id);
        await AddChatMemberIfMissingAsync(context, chat, secondUser.Id);
        await AddMessageIfMissingAsync(context, firstUser.Id, "Hello from the Cogni demo account.");
        await AddMessageIfMissingAsync(context, secondUser.Id, "The demo data is ready.");
        await AddMessageStatusIfMissingAsync(context, firstUser.Id);
        await AddMessageStatusIfMissingAsync(context, secondUser.Id);
        await context.SaveChangesAsync();

        await AddLikeIfMissingAsync(context, firstUser.Id, secondPost.Id);
        await AddLikeIfMissingAsync(context, secondUser.Id, firstPost.Id);
        await transaction.CommitAsync();

        Console.WriteLine("Development data is ready in all 17 tables.");
        Console.WriteLine("Demo accounts: alex.demo@cogni.local and maya.demo@cogni.local");
        Console.WriteLine("Password for newly created demo accounts: COGNI_SEED_PASSWORD, or CogniDemo123! by default.");
    }

    private static async Task<User> GetOrCreateUserAsync(CogniDbContext context, string email, string name, string surname, int roleId, int mbtiId, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(item => item.Email == email);
        if (user is not null) return user;

        var hash = new PasswordHasher().HashPassword(password, out var salt);
        user = new User
        {
            Name = name,
            Surname = surname,
            Email = email,
            PasswordHash = hash,
            Salt = salt,
            IdRole = roleId,
            IdMbtiType = mbtiId,
            LastLogin = UtcTimestampWithoutTimeZone(),
            RefreshTokenExpiryTime = DateTime.UtcNow,
            BannerImage = "https://placehold.co/1200x300/png?text=Cogni+Demo+Banner"
        };
        context.Users.Add(user);
        return user;
    }

    private static async Task AddAvatarIfMissingAsync(CogniDbContext context, int userId)
    {
        if (!await context.Avatars.AnyAsync(item => item.UserId == userId && item.IsActive == true))
            context.Avatars.Add(new Avatar { UserId = userId, AvatarUrl = AvatarUrl, DateAdded = UtcTimestampWithoutTimeZone(), IsActive = true });
    }

    private static async Task<List<Tag>> GetOrCreateTagsAsync(CogniDbContext context)
    {
        var tags = new List<Tag>();
        foreach (var name in new[] { "Cogni Demo", "Technology", "Books" })
        {
            var tag = await context.Tags.FirstOrDefaultAsync(item => item.NameTag == name);
            if (tag is null)
            {
                tag = new Tag { NameTag = name };
                context.Tags.Add(tag);
            }
            tags.Add(tag);
        }
        return tags;
    }

    private static async Task AddUserTagIfMissingAsync(CogniDbContext context, int userId, int tagId)
    {
        if (!await context.UserTags.AnyAsync(item => item.IdUser == userId && item.IdTag == tagId))
            context.UserTags.Add(new UserTag { IdUser = userId, IdTag = tagId });
    }

    private static async Task<Article> GetOrCreateArticleAsync(CogniDbContext context, User user, string title)
    {
        var article = await context.Articles.FirstOrDefaultAsync(item => item.ArticleName == title);
        if (article is not null) return article;
        article = new Article
        {
            ArticleName = title,
            ArticleBody = "Sample article content for local development.",
            ArticlePreview = ArticleImageUrl,
            Annotation = "Cogni development seed data",
            Created = DateTime.UtcNow,
            ReadsNumber = 1,
            IdUser = user.Id
        };
        context.Articles.Add(article);
        return article;
    }

    private static async Task<Post> GetOrCreatePostAsync(CogniDbContext context, User user, string body)
    {
        var post = await context.Posts.FirstOrDefaultAsync(item => item.PostBody == body);
        if (post is not null) return post;
        post = new Post { PostBody = body, IdUser = user.Id, CreatedAt = DateTime.UtcNow };
        context.Posts.Add(post);
        return post;
    }

    private static async Task AddArticleImageIfMissingAsync(CogniDbContext context, int articleId)
    {
        if (!await context.ArticleImages.AnyAsync(item => item.ArticleId == articleId))
            context.ArticleImages.Add(new ArticleImage { ArticleId = articleId, ImageUrl = ArticleImageUrl });
    }

    private static async Task AddPostImageIfMissingAsync(CogniDbContext context, int postId)
    {
        if (!await context.PostImages.AnyAsync(item => item.PostId == postId))
            context.PostImages.Add(new PostImage { PostId = postId, ImageUrl = PostImageUrl });
    }

    private static async Task AddFriendIfMissingAsync(CogniDbContext context, int userId, int friendId)
    {
        if (!await context.Friends.AnyAsync(item => item.UserId == userId && item.FriendId == friendId))
            context.Friends.Add(new Friend { UserId = userId, FriendId = friendId, DateAdded = UtcTimestampWithoutTimeZone() });
    }

    private static async Task AddChatMemberIfMissingAsync(CogniDbContext context, Chat chat, int userId)
    {
        if (!await context.Set<ChatMember>().AnyAsync(item => item.ChatId == chat.Id && item.UserId == userId))
            context.Set<ChatMember>().Add(new ChatMember { ChatId = chat.Id, UserId = userId, Chat = chat });
    }

    private static async Task AddMessageIfMissingAsync(CogniDbContext context, int senderId, string message)
    {
        if (!await context.Messages.AnyAsync(item => item.ChatId == DemoChatId && item.SenderId == senderId && item.Msg == message))
            context.Messages.Add(new Message { ChatId = DemoChatId, SenderId = senderId, Msg = message, Date = DateTime.UtcNow });
    }

    private static async Task AddMessageStatusIfMissingAsync(CogniDbContext context, int userId)
    {
        if (!await context.Set<MessageStatus>().AnyAsync(item => item.ChatId == DemoChatId && item.UserId == userId))
            context.Set<MessageStatus>().Add(new MessageStatus { ChatId = DemoChatId, UserId = userId, LastReaden = 0 });
    }

    private static async Task AddLikeIfMissingAsync(CogniDbContext context, int userId, int postId)
    {
        await context.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO likes (user_id, post_id, liked_at)
            SELECT {userId}, {postId}, CURRENT_TIMESTAMP AT TIME ZONE 'UTC'
            WHERE NOT EXISTS (
                SELECT 1 FROM likes WHERE user_id = {userId} AND post_id = {postId}
            )
            """);
    }

    private static DateTime UtcTimestampWithoutTimeZone() =>
        DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
}