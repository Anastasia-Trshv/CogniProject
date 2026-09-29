# Для упрощения процесса разработки и тестирования можно использовать докер 🐳

### Пошаговый гайд:

1. Скачать [Docker desktop](https://www.docker.com/products/docker-desktop/) 😉
2. Запустить docker desktop 🤭
3. Создать файл secrets.json с актуальными секретами в корне проекта 🔐
4. Выполнить `docker compose -f compose.dev.yml up dev_cogni dev_chat_service` в корне проекта 👾
5. Подождать запуска ⌛

С vpn на 4 шаг может выполниться не с первого раза при первом запуске 😥

Запуск проекта проекта происходит в watch режиме - он будет смотреть за изменениями и пересобирать при надобности 🤯 (_но он пересобирает далеко не все, например CORS в Program.cs не получится менять в рантайме_)

Запуск фронтенда для чатов: `docker compose -f .\compose.dev.yml watch dev_chat_frontend`

Немного больше команд:
`docker compose -f compose.dev.yml up dev_cogni dev_chat_service -d` - Запуск в фоновом режиме - так он не будет привязан к сессии консоли, в таком случае логи нужно смотреть с помощью `docker compose -f compose.dev.yml logs -f dev_chat_service` (именно эта команда смотрит логи только для `dev_chat_service`, также есть `dev_cogni`) 🫣

### Миграции и тестовые данные

В dev-конфигурации для `dev_cogni` задано `MIGRATE=true`. При запуске приложения EF Core автоматически применяет все ожидающие миграции к базе из корневого `secrets.json`. Перед запуском убедитесь, что PostgreSQL доступен по указанной там строке подключения.

Запуск приложения и автоматическое применение миграций:

```powershell
docker compose -f compose.dev.yml up dev_cogni dev_chat_service
```

Посмотреть логи миграций:

```powershell
docker compose -f compose.dev.yml logs -f dev_cogni
```

Чтобы создать новую миграцию, выполните из корня `CogniProject` (нужен установленный `dotnet-ef`):

```powershell
dotnet ef migrations add <MigrationName> --project Cogni/Cogni.csproj --startup-project Cogni/Cogni.csproj
```

В dev-окружении миграция применится при следующем запуске `dev_cogni`. Применить её вручную можно командой:

```powershell
dotnet ef database update --project Cogni/Cogni.csproj --startup-project Cogni/Cogni.csproj
```

После запуска `dev_cogni` наполните таблицы тестовыми данными:

```powershell
.\Cogni\seed-dev.ps1
```

Скрипт идемпотентный: повторный запуск не должен создавать дубликаты. По умолчанию создаются пользователи `alex.demo@cogni.local` и `maya.demo@cogni.local` с паролем `CogniDemo123!`. Задать пароль для новых тестовых пользователей можно при первом запуске:

```powershell
.\Cogni\seed-dev.ps1 -Password "YourDemoPassword123!"
```

Сид применяет миграции перед заполнением и создаёт данные во всех 17 таблицах. Фотографии представлены тестовыми URL; Uploadcare для наполнения не требуется. Существующим demo-аккаунтам скрипт пароль не меняет.
