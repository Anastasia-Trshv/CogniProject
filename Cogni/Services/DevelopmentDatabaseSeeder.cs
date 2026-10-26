using Cogni.Authentication;
using Cogni.Database.Context;
using Cogni.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cogni.Database.Seeding;

public static class DevelopmentDatabaseSeeder
{
    private const string DefaultPassword = "CogniDemo123!";
    private const string SakinorvaCatalogJson = """
{
  "scale": {
    "1": "Совсем не про меня",
    "2": "Скорее не про меня",
    "3": "Бывает по-разному",
    "4": "Скорее про меня",
    "5": "Точно про меня"
  },
  "questions": [
    {
      "id": "Te1",
      "function": "Te",
      "question": "Выбирая что-то серьёзное, я не успокоюсь, пока не сравню варианты по цифрам и отзывам.",
      "example": "Например: новый телефон, курс, врач."
    },
    {
      "id": "Te2",
      "function": "Te",
      "question": "Мне лучше работается, когда заранее ясно, кто что делает и к какому сроку.",
      "example": "Например: «ты делаешь до пятницы, я — до понедельника»."
    },
    {
      "id": "Te3",
      "function": "Te",
      "question": "Если результат нельзя измерить, мне трудно понять, получилось у меня или нет.",
      "example": "Сколько сделано, сколько сэкономлено, на сколько стало лучше."
    },
    {
      "id": "Te4",
      "function": "Te",
      "question": "Если в моей работе или быту что-то устроено неудобно, я нахожу, как сделать это быстрее и проще.",
      "example": "Например, меняю порядок действий, чтобы тратить меньше времени."
    },
    {
      "id": "Te5",
      "function": "Te",
      "question": "Если в группе никто не руководит, я легко беру на себя распределение задач.",
      "example": "Например, на совместном проекте или при организации поездки."
    },
    {
      "id": "Te6",
      "function": "Te",
      "question": "Чужое мнение меня не убеждает, пока за ним нет фактов.",
      "example": "«Мне так кажется» для меня не аргумент."
    },
    {
      "id": "Ti1",
      "function": "Ti",
      "question": "Меня задевает, когда в чьих-то словах есть противоречие, а все делают вид, что не заметили.",
      "example": "Например, человек сначала говорит одно, а через минуту — прямо противоположное."
    },
    {
      "id": "Ti2",
      "function": "Ti",
      "question": "Прежде чем пользоваться новой вещью или методом, я хочу понять, как он устроен.",
      "example": "Например, прочитать, как работает прибор, а не просто нажимать кнопки."
    },
    {
      "id": "Ti3",
      "function": "Ti",
      "question": "Я люблю подбирать точные слова, чтобы мысль была выражена без искажений.",
      "example": "Могу долго искать то самое слово или поправить собеседника, если он сказал неточно."
    },
    {
      "id": "Ti4",
      "function": "Ti",
      "question": "Мне трудно принять правило или совет, если мне не объяснили логику.",
      "example": "Ответ «потому что так надо» меня не устраивает."
    },
    {
      "id": "Ti5",
      "function": "Ti",
      "question": "Услышав новую теорию, я мысленно проверяю её на слабые места.",
      "example": "Например, думаю: «а если взять другой случай, это ещё будет работать?»"
    },
    {
      "id": "Ti6",
      "function": "Ti",
      "question": "Мне нравится разбираться в сложной задаче ради интереса, даже если практической пользы от этого нет.",
      "example": "Например, головоломки, логические игры, вопрос «почему это так устроено?»"
    },
    {
      "id": "Fe1",
      "function": "Fe",
      "question": "Я замечаю, что у человека испортилось настроение, ещё до того, как он об этом скажет.",
      "example": ""
    },
    {
      "id": "Fe2",
      "function": "Fe",
      "question": "Когда в компании повисает напряжение, мне хочется его разрядить.",
      "example": "Например, пошутить, сменить тему или предложить что-то сделать вместе."
    },
    {
      "id": "Fe3",
      "function": "Fe",
      "question": "Я говорю по-разному с разными людьми, подстраиваясь под них.",
      "example": "Например, с одним мягче, с другим проще, с третьим серьёзнее."
    },
    {
      "id": "Fe4",
      "function": "Fe",
      "question": "Если близкий человек расстроен, я сразу стараюсь его поддержать или развеселить.",
      "example": ""
    },
    {
      "id": "Fe5",
      "function": "Fe",
      "question": "Мне важно, чтобы за столом или на встрече всем было комфортно и никто не чувствовал себя лишним.",
      "example": ""
    },
    {
      "id": "Fe6",
      "function": "Fe",
      "question": "Прежде чем сказать что-то резкое, я стараюсь представить, как это воспримут, и смягчаю формулировку.",
      "example": ""
    },
    {
      "id": "Fi1",
      "function": "Fi",
      "question": "Если на работе от меня потребуют делать то, что противоречит моим личным ценностям, я уйду, даже если уходить невыгодно.",
      "example": "Например, рекламировать то, во что сам не верю."
    },
    {
      "id": "Fi2",
      "function": "Fi",
      "question": "Мне тяжело изображать радость или интерес, когда на самом деле их нет.",
      "example": "Даже из вежливости."
    },
    {
      "id": "Fi3",
      "function": "Fi",
      "question": "В споре о том, что для меня по-настоящему важно, я могу проиграть по аргументам, но своего мнения не изменю.",
      "example": ""
    },
    {
      "id": "Fi4",
      "function": "Fi",
      "question": "Выбирая занятие, работу или друзей, я ориентируюсь на то, что мне лично по-настоящему близко.",
      "example": "А не на то, что престижно или выгодно."
    },
    {
      "id": "Fi5",
      "function": "Fi",
      "question": "Я быстро чувствую, когда человек говорит красивые слова, но сам в них не верит.",
      "example": "Хвалит или обещает помочь, но чувствуется, что ему всё равно."
    },
    {
      "id": "Fi6",
      "function": "Fi",
      "question": "Несправедливость задевает меня лично, даже если касается незнакомых людей.",
      "example": "Например, когда читаю новость о том, как кого-то обидели."
    },
    {
      "id": "Se1",
      "function": "Se",
      "question": "Войдя в комнату, я сразу замечаю, что в ней изменилось.",
      "example": "Например, новую вещь, перестановку, незнакомый запах."
    },
    {
      "id": "Se2",
      "function": "Se",
      "question": "Если что-то внезапно пошло не так, я быстро соображаю на месте и начинаю действовать.",
      "example": "Например, что-то сломалось в дороге или сорвались планы."
    },
    {
      "id": "Se3",
      "function": "Se",
      "question": "Мне нравятся физическая активность и сильные впечатления.",
      "example": "Например, спорт, скорость, аттракционы, танцы."
    },
    {
      "id": "Se4",
      "function": "Se",
      "question": "Я сильно ощущаю вкус еды, запахи, звуки, прикосновения и получаю от этого большое удовольствие.",
      "example": ""
    },
    {
      "id": "Se5",
      "function": "Se",
      "question": "Я лучше усваиваю новое, когда пробую делать это своими руками, а не читаю или слушаю объяснение.",
      "example": ""
    },
    {
      "id": "Se6",
      "function": "Se",
      "question": "Если друзья вдруг предлагают поехать куда-нибудь прямо сегодня, я с радостью соглашаюсь.",
      "example": ""
    },
    {
      "id": "Si1",
      "function": "Si",
      "question": "Я хорошо помню подробности прошлого.",
      "example": "Например, кто что сказал, как выглядело место, какая была погода."
    },
    {
      "id": "Si2",
      "function": "Si",
      "question": "Мне нравится возвращаться к знакомому.",
      "example": "Например, в любимое кафе, на привычный маршрут, к любимому фильму."
    },
    {
      "id": "Si3",
      "function": "Si",
      "question": "Пробуя что-то новое, я сразу прикидываю, лучше оно или хуже того, к чему я привык.",
      "example": "«В прошлый раз было вкуснее»."
    },
    {
      "id": "Si4",
      "function": "Si",
      "question": "Мне комфортнее, когда день идёт по привычному распорядку, и неприятно, когда его внезапно ломают.",
      "example": ""
    },
    {
      "id": "Si5",
      "function": "Si",
      "question": "Мне трудно расстаться с вещами, которые связаны с воспоминаниями.",
      "example": "Например, подарки, старые фотографии, билеты, письма."
    },
    {
      "id": "Si6",
      "function": "Si",
      "question": "Я не меняю способ, который меня не раз выручал, даже если слышу про новые.",
      "example": "Как готовить, учить, добираться."
    },
    {
      "id": "Ne1",
      "function": "Ne",
      "question": "Когда я думаю над проблемой, мне в голову сразу приходит много разных вариантов решения.",
      "example": "Даже странных."
    },
    {
      "id": "Ne2",
      "function": "Ne",
      "question": "В разговоре я часто перескакиваю с одной темы на другую, потому что одно напоминает мне о другом.",
      "example": ""
    },
    {
      "id": "Ne3",
      "function": "Ne",
      "question": "Меня легко увлекают новые идеи, и я часто берусь за несколько дел одновременно.",
      "example": ""
    },
    {
      "id": "Ne4",
      "function": "Ne",
      "question": "Я люблю придумывать «а что, если?» просто ради забавы.",
      "example": "Например: «а что, если бы все люди могли читать мысли?»"
    },
    {
      "id": "Ne5",
      "function": "Ne",
      "question": "Мне интересно пробовать разные способы сделать одно и то же, даже если привычный способ работает.",
      "example": ""
    },
    {
      "id": "Ne6",
      "function": "Ne",
      "question": "После фильма или книги я часто думаю, как всё могло бы сложиться иначе.",
      "example": "«А если бы герой выбрал другую дорогу?»"
    },
    {
      "id": "Ni1",
      "function": "Ni",
      "question": "Иногда я заранее чувствую, чем закончится ситуация, и потом это подтверждается.",
      "example": ""
    },
    {
      "id": "Ni2",
      "function": "Ni",
      "question": "Ответ на сложный вопрос иногда приходит ко мне внезапно, когда я думаю совсем о другом.",
      "example": "Например, в душе, в дороге или перед сном."
    },
    {
      "id": "Ni3",
      "function": "Ni",
      "question": "Принимая решения, я думаю о том, кем или где хочу быть через несколько лет.",
      "example": "Например, выбирая работу, смотрю не на ближайшую зарплату, а на то, куда это меня приведёт."
    },
    {
      "id": "Ni4",
      "function": "Ni",
      "question": "Мне бывает трудно объяснить, откуда я знаю, что это верно: я просто понимаю.",
      "example": "Например, сразу чувствуешь, что договорённость сорвётся, хотя поводов нет."
    },
    {
      "id": "Ni5",
      "function": "Ni",
      "question": "Сложные вещи мне проще понять через образ или сравнение.",
      "example": "Например, представить идею как картинку или как историю."
    },
    {
      "id": "Ni6",
      "function": "Ni",
      "question": "Мне проще сосредоточиться на одной идее и довести её до конца, чем распылять внимание на много вариантов.",
      "example": ""
    }
  ],
  "controlQuestions": [
    {
      "id": "K3",
      "question": "Я никогда не раздражаюсь на близких людей."
    },
    {
      "id": "K4",
      "question": "Я всегда сразу признаю свою неправоту."
    }
  ],
  "stacks": {
    "ISTJ": [
      "Si",
      "Te",
      "Fi",
      "Ne"
    ],
    "ISFJ": [
      "Si",
      "Fe",
      "Ti",
      "Ne"
    ],
    "INFJ": [
      "Ni",
      "Fe",
      "Ti",
      "Se"
    ],
    "INTJ": [
      "Ni",
      "Te",
      "Fi",
      "Se"
    ],
    "ISTP": [
      "Ti",
      "Se",
      "Ni",
      "Fe"
    ],
    "ISFP": [
      "Fi",
      "Se",
      "Ni",
      "Te"
    ],
    "INFP": [
      "Fi",
      "Ne",
      "Si",
      "Te"
    ],
    "INTP": [
      "Ti",
      "Ne",
      "Si",
      "Fe"
    ],
    "ESTP": [
      "Se",
      "Ti",
      "Fe",
      "Ni"
    ],
    "ESFP": [
      "Se",
      "Fi",
      "Te",
      "Ni"
    ],
    "ENFP": [
      "Ne",
      "Fi",
      "Te",
      "Si"
    ],
    "ENFJ": [
      "Fe",
      "Ni",
      "Se",
      "Ti"
    ],
    "ENTP": [
      "Ne",
      "Ti",
      "Fe",
      "Si"
    ],
    "ENTJ": [
      "Te",
      "Ni",
      "Se",
      "Fi"
    ],
    "ESTJ": [
      "Te",
      "Si",
      "Ne",
      "Fi"
    ],
    "ESFJ": [
      "Fe",
      "Si",
      "Ne",
      "Ti"
    ]
  },
  "functionNames": {
    "Te": "Экстравертная логика (Te)",
    "Ti": "Интровертная логика (Ti)",
    "Fe": "Экстравертная этика (Fe)",
    "Fi": "Интровертная этика (Fi)",
    "Se": "Экстравертная сенсорика (Se)",
    "Si": "Интровертная сенсорика (Si)",
    "Ne": "Экстравертная интуиция (Ne)",
    "Ni": "Интровертная интуиция (Ni)"
  },
  "grantWeights": [
    4,
    3,
    0.5,
    -2
  ]
}
""";

    private static readonly Guid DemoChatId = Guid.Parse("8be7208d-0f36-4b3e-b2df-82502f4a5dd0");
    private const string AvatarUrl = "https://placehold.co/160x160/png?text=Cogni+Demo";
    private const string ArticleImageUrl = "https://placehold.co/1200x800/png?text=Cogni+Article";
    private const string PostImageUrl = "https://placehold.co/1200x800/png?text=Cogni+Post";

    public static async Task SeedAsync(CogniDbContext context)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        // Вопросы, контрольные утверждения и настройки алгоритма хранятся в БД.
        if (!await context.SakinorvaCatalogs.AnyAsync())
        {
            context.SakinorvaCatalogs.Add(new SakinorvaCatalog { Id = 1, JsonContent = SakinorvaCatalogJson });
            await context.SaveChangesAsync();
        }

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

        var additionalUsers = new[]
        {
            await GetOrCreateUserAsync(context, "anna.demo@cogni.local", "Demo Anna", "Sokolova", roleId, mbtiId, password),
            await GetOrCreateUserAsync(context, "ivan.demo@cogni.local", "Demo Ivan", "Smirnov", roleId, mbtiId, password),
            await GetOrCreateUserAsync(context, "olga.demo@cogni.local", "Demo Olga", "Volkova", roleId, mbtiId, password),
            await GetOrCreateUserAsync(context, "max.demo@cogni.local", "Demo Max", "Orlov", roleId, mbtiId, password),
            await GetOrCreateUserAsync(context, "kate.demo@cogni.local", "Demo Kate", "Morozova", roleId, mbtiId, password),
            await GetOrCreateUserAsync(context, "dmitry.demo@cogni.local", "Demo Dmitry", "Kuznetsov", roleId, mbtiId, password)
        };
        await context.SaveChangesAsync();

        await AddAvatarIfMissingAsync(context, firstUser.Id);
        await AddAvatarIfMissingAsync(context, secondUser.Id);
        foreach (var user in additionalUsers)
            await AddAvatarIfMissingAsync(context, user.Id);
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
        for (var i = 0; i < additionalUsers.Length; i++)
        {
            await AddUserTagIfMissingAsync(context, additionalUsers[i].Id, tags[(i + 1) % tags.Count].Id);
            await AddUserTagIfMissingAsync(context, additionalUsers[i].Id, tags[(i + 2) % tags.Count].Id);
        }

        var hobbies = await GetOrCreateHobbyCatalogAsync(context);
        await context.SaveChangesAsync();
        foreach (var (hobbyName, categoryName) in new[]
        {
            ("Творчество", "рисование"),
            ("Игры", "видеоигры"),
            ("Медиа", "сериалы"),
            ("Музыка", "рок"),
            ("Творчество", "бисероплетение"),
            ("Творчество", "вязание")
        })
        {
            var category = hobbies.First(item => item.Name == hobbyName).Categories.First(item => item.Name == categoryName);
            await AddUserTagCategoryIfMissingAsync(context, firstUser.Id, category.Id);
        }
        var secondUserMusic = hobbies.First(item => item.Name == "Музыка");
        var secondUserRock = secondUserMusic.Categories.First(item => item.Name == "рок");
        await AddUserTagCategoryIfMissingAsync(context, secondUser.Id, secondUserRock.Id);
        await AddUserTagIfMissingAsync(context, secondUser.Id, secondUserRock.Tags.First(tag => tag.NameTag == "Nirvana").Id);

        var firstArticle = await GetOrCreateArticleAsync(context, firstUser, "Cogni demo: first article");
        var secondArticle = await GetOrCreateArticleAsync(context, secondUser, "Cogni demo: second article");
        var firstPost = await GetOrCreatePostAsync(context, firstUser, "A first post from the Cogni demo account.");
        var secondPost = await GetOrCreatePostAsync(context, secondUser, "A second post from the Cogni demo account.");
        var additionalArticles = new List<Article>();
        var additionalPosts = new List<Post>();
        for (var i = 0; i < additionalUsers.Length; i++)
        {
            additionalArticles.Add(await GetOrCreateArticleAsync(context, additionalUsers[i], $"Cogni demo: article {i + 3}"));
            additionalPosts.Add(await GetOrCreatePostAsync(context, additionalUsers[i], $"A demo post from user {i + 3}."));
        }
        await context.SaveChangesAsync();

        await AddArticleImageIfMissingAsync(context, firstArticle.Id);
        await AddArticleImageIfMissingAsync(context, secondArticle.Id);
        await AddPostImageIfMissingAsync(context, firstPost.Id);
        await AddPostImageIfMissingAsync(context, secondPost.Id);
        foreach (var article in additionalArticles)
            await AddArticleImageIfMissingAsync(context, article.Id);
        foreach (var post in additionalPosts)
            await AddPostImageIfMissingAsync(context, post.Id);

        await AddFriendIfMissingAsync(context, firstUser.Id, secondUser.Id);
        await AddFriendIfMissingAsync(context, secondUser.Id, firstUser.Id);
        for (var i = 0; i < additionalUsers.Length; i++)
        {
            var user = additionalUsers[i];
            var nextUser = additionalUsers[(i + 1) % additionalUsers.Length];
            await AddFriendIfMissingAsync(context, user.Id, nextUser.Id);
            await AddFriendIfMissingAsync(context, firstUser.Id, user.Id);
        }

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
        foreach (var user in additionalUsers)
            await AddChatMemberIfMissingAsync(context, chat, user.Id);
        await AddMessageIfMissingAsync(context, firstUser.Id, "Hello from the Cogni demo account.");
        await AddMessageIfMissingAsync(context, secondUser.Id, "The demo data is ready.");
        var demoMessages = new[]
        {
            "Nice to meet everyone here.",
            "I am testing the new profile page.",
            "The articles section looks good.",
            "I added a few interests to my profile.",
            "Let's try the chat notifications.",
            "The development data is useful for testing."
        };
        for (var i = 0; i < additionalUsers.Length; i++)
            await AddMessageIfMissingAsync(context, additionalUsers[i].Id, demoMessages[i]);
        await AddMessageStatusIfMissingAsync(context, firstUser.Id);
        await AddMessageStatusIfMissingAsync(context, secondUser.Id);
        foreach (var user in additionalUsers)
            await AddMessageStatusIfMissingAsync(context, user.Id);
        await context.SaveChangesAsync();

        await AddLikeIfMissingAsync(context, firstUser.Id, secondPost.Id);
        await AddLikeIfMissingAsync(context, secondUser.Id, firstPost.Id);
        for (var i = 0; i < additionalUsers.Length; i++)
            await AddLikeIfMissingAsync(context, additionalUsers[i].Id, i % 2 == 0 ? firstPost.Id : secondPost.Id);
        await transaction.CommitAsync();

        Console.WriteLine("Development data is ready.");
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
        foreach (var name in new[] { "Cogni Demo", "Technology", "Books", "Music", "Travel", "Programming" })
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

    private static async Task<List<Hobby>> GetOrCreateHobbyCatalogAsync(CogniDbContext context)
    {
        var catalog = new Dictionary<string, Dictionary<string, string[]>>
        {
            ["Творчество"] = new()
            {
                ["рисование"] = [],
                ["бисероплетение"] = [],
                ["вязание"] = []
            },
            ["Медиа"] = new()
            {
                ["сериалы"] = ["Очень странные дела", "Шерлок", "Во все тяжкие", "Аркейн", "Офис"]
            },
            ["Игры"] = new()
            {
                ["видеоигры"] = ["Minecraft", "The Sims 4", "Stardew Valley", "Genshin Impact", "Baldur's Gate 3"]
            },
            ["Спорт"] = new()
            {
                ["бег"] = ["5 км", "марафон"],
                ["фитнес"] = ["силовые тренировки", "йога"],
                ["футбол"] = ["Чемпионат мира", "Лига чемпионов"]
            },
            ["Музыка"] = new()
            {
                ["к-поп"] = ["BTS", "BLACKPINK", "Stray Kids", "TWICE"],
                ["хип-хоп"] = ["Jay-Z", "Kendrick Lamar", "Eminem", "Drake"],
                ["рок"] = ["The Beatles", "Nirvana", "Queen", "Radiohead"]
            }
        };
        var hobbies = new List<Hobby>();

        foreach (var (hobbyName, categories) in catalog)
        {
            var hobby = await context.Hobbies
                .Include(item => item.Categories)
                .ThenInclude(item => item.Tags)
                .FirstOrDefaultAsync(item => item.Name == hobbyName);
            if (hobby is null)
            {
                hobby = new Hobby { Name = hobbyName };
                context.Hobbies.Add(hobby);
                await context.SaveChangesAsync();
            }

            foreach (var (categoryName, tagNames) in categories)
            {
                var category = hobby.Categories.FirstOrDefault(item => item.Name == categoryName);
                if (category is null)
                {
                    category = new TagCategory { Name = categoryName };
                    hobby.Categories.Add(category);
                }

                foreach (var tagName in tagNames)
                {
                    if (!category.Tags.Any(item => item.NameTag == tagName))
                        category.Tags.Add(new Tag { NameTag = tagName });
                }
            }
            hobbies.Add(hobby);
        }

        return hobbies;
    }

    private static async Task AddUserTagCategoryIfMissingAsync(CogniDbContext context, int userId, int categoryId)
    {
        var userTagCategory = await context.UserTagCategories
            .FirstOrDefaultAsync(item => item.IdUser == userId && item.IdCategory == categoryId);
        if (userTagCategory is null)
            context.UserTagCategories.Add(new UserTagCategory { IdUser = userId, IdCategory = categoryId });
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