# Cogni Backend & Chat Platform 🧠💬

Бэкенд-платформа проекта Cogni, включающая основной сервис бизнес-логики (профили, MBTI-тестирование, посты, статьи, теги и связи), сервис обмена сообщениями в реальном времени на базе SignalR, а также вспомогательный frontend-клиент для тестирования чата.

---

## 🚀 Быстрый запуск (Quick Start)

Для локальной разработки и тестирования используется **Docker Compose**. Он поднимает всю необходимую инфраструктуру (PostgreSQL, Redis, RabbitMQ, MinIO) и сервисы приложения.

### Предварительные требования
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (запущенный)
* PowerShell (или совместимый терминал)
* *(Опционально)* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) — только если планируется запуск бэкенда без Docker.

---

### Вариант 1: Запуск через Docker Compose (Рекомендуемый)

1. **Проверьте конфигурацию секретов**  
   Убедитесь в наличии файла `dockerized.secrets.json` в корне проекта.  
   Compose автоматически монтирует его в оба backend-контейнера (`dev_cogni` и `dev_chat_service`) как `secrets.json`.

2. **Запустите бэкенд и инфраструктуру**  
   Из корня проекта выполните:
   ```powershell
   # Запуск с выводом логов в консоль:
   docker compose -f compose.dev.yml up dev_cogni dev_chat_service

   # Или запуск в фоновом режиме (-d):
   docker compose -f compose.dev.yml up dev_cogni dev_chat_service -d
   ```
   > ℹ️ **Примечание:** При первом запуске с активным VPN скачивание базовых образов может потребовать больше времени или повторной попытки.

3. **Заполните базу тестовыми данными (сидирование)**  
   После успешного старта контейнеров накатите тестовые профили:
   ```powershell
   .\Cogni\seed-dev.ps1 -Password "CogniDemo123!"
   ```

4. **Запустите frontend для отладки чатов (опционально)**  
   Фронтенд запускается в режиме Compose Watch (с авто-синхронизацией изменений кода):
   ```powershell
   docker compose -f compose.dev.yml watch dev_chat_frontend
   ```

---

### Вариант 2: Локальный запуск бэкенда (.NET SDK) + Инфраструктура в Docker

Если вы хотите отлаживать C# код непосредственно в Visual Studio / Rider или через `dotnet run`:

1. Поднимите только инфраструктурные сервисы:
   ```powershell
   docker compose -f compose.dev.yml up postgres redis rabbitmq minio -d
   ```
2. Скопируйте файл `localdev.secrets.json` в проекты как `secrets.json`:
   * В папку `Cogni/secrets.json`
   * В папку `ChatService/secrets.json`
3. Запустите проекты через IDE или терминал:
   ```powershell
   dotnet run --project Cogni/Cogni.csproj
   dotnet run --project ChatService/ChatService.csproj
   ```

---

### Полезные команды при разработке

* **Просмотр логов сервисов:**
  ```powershell
  docker compose -f compose.dev.yml logs -f dev_cogni
  docker compose -f compose.dev.yml logs -f dev_chat_service
  ```
* **Перезапуск backend-сервисов после изменения C#-кода:**  
  Контейнеры `dev_cogni` и `dev_chat_service` используют `dotnet run` (без `dotnet watch`). Исходники примонтированы через volumes. После изменения кода перезапустите контейнеры:
  ```powershell
  docker compose -f compose.dev.yml restart dev_cogni dev_chat_service
  ```
* **Остановка всех контейнеров:**
  ```powershell
  docker compose -f compose.dev.yml down
  ```

---

## 🌐 Порты и доступ к сервисам

| Сервис / Интерфейс | Адрес / URL | Описание | Учётные данные (по умолчанию) |
|---|---|---|---|
| **Cogni API (Swagger)** | [http://localhost:5279/swagger](http://localhost:5279/swagger) | Swagger UI основного сервиса | — |
| **ChatService (Swagger)** | [http://localhost:5108/swagger](http://localhost:5108/swagger) | Swagger UI чат-сервиса & SignalR Docs | — |
| **SignalR Chat Hub** | `ws://localhost:5108/chat` | WebSocket/SignalR хаб общения | JWT Bearer |
| **Chat Dev Frontend** | [http://localhost:5173](http://localhost:5173) | Vite SPA для тестирования чата | — |
| **RabbitMQ Management UI**| [http://localhost:15672](http://localhost:15672) | Панель управления очередями RabbitMQ | `user` / `password` |
| **MinIO Console / S3** | [http://localhost:9000](http://localhost:9000) (API), `:9090` (UI) | S3-совместимое файловое хранилище | `admin` / `password123` |
| **PostgreSQL** | `localhost:5004` | База данных Cogni (`cogni`) | `user` / `password` |
| **Redis** | `localhost:5002` | In-memory хранилище сессий и кеша | `user` / `password` |
| **HAProxy (Production)** | `http://localhost:9100` | Балансировщик нагрузки и reverse proxy | — |

---

## 📁 Структура проекта

```text
CogniProject/
├── Cogni/                   # Основной сервис бизнес-логики (ASP.NET Core Web API)
│   ├── Abstractions/        # Интерфейсы сервисов и репозиториев
│   ├── Authentication/      # Логика аутентификации и хэширования паролей
│   ├── Contracts/           # DTO и контракты взаимодействия
│   ├── Controllers/         # API-контроллеры (пользователи, посты, статьи, теги, MBTI)
│   ├── Migrations/          # Миграции Entity Framework Core для PostgreSQL
│   ├── Models/              # Модели данных и DTO
│   ├── Repositories/        # Репозитории доступа к данным через EF Core
│   ├── Services/            # Сервисный слой бизнес-логики и сидер БД
│   ├── seed-dev.ps1         # PowerShell-скрипт для заполнения базы тестовыми данными
│   ├── Dockerfile / .dev    # Dockerfile для production и development
│   └── Program.cs           # Точка входа основного приложения
│
├── ChatService/             # Микросервис реального времени (SignalR Hub + Web API)
│   ├── Abstractions/        # Интерфейсы репозиториев и хаба
│   ├── Controllers/         # ChatHubController (SignalR Hub) и FilesController (загрузка в MinIO)
│   ├── Models/              # DTO чата, событий и полезной нагрузки
│   ├── Repository/          # Хранилище сообщений в БД и кеш-репозиторий в Redis
│   ├── Services/            # HubService и ConsumerService (обработка сообщений RabbitMQ)
│   ├── SwaggerExt/          # Документирование SignalR в Swagger UI
│   ├── Dockerfile / .dev    # Dockerfile для production и development
│   └── Program.cs           # Точка входа сервиса чата
│
├── Shared/                  # Общая библиотека (.NET Class Library)
│   ├── Database/
│   │   ├── Context/         # CogniDbContext (контекст базы данных EF Core)
│   │   └── Entities/        # Модели сущностей БД (User, Chat, Message, Post, Article, Hobby и др.)
│   └── Jwt/                 # Компоненты генерации, валидации и middleware для JWT
│
├── ChatDevFrontend/         # Тестовый клиент чата (Vite + JavaScript + Tailwind CSS)
│   ├── src/                 # Исходный код SPA (работа с SignalR, отображение диалогов)
│   ├── Dockerfile / .dev    # Dockerfile сборки и dev-сервера с поддержкой compose watch
│   └── package.json         # Зависимости фронтенда (@microsoft/signalr, tailwindcss, vite)
│
├── haproxy/                 # Балансировщик и reverse proxy для production
│   └── haproxy.cfg          # Конфигурация маршрутизации запросов между фронтендом и бэкендами
│
├── minio/                   # Конфигурация объектного хранилища файлов
│   └── init.sh              # Скрипт инициализации бакета 'chat-files' и политик доступа
│
├── compose.dev.yml          # Docker Compose для локальной разработки (монтирование исходников, debug)
├── compose.prod.yml         # Docker Compose для релизного развёртывания (HAProxy, production build)
├── dockerized.secrets.json  # Конфигурация строк подключения для контейнеров Docker
├── localdev.secrets.json    # Конфигурация строк подключения для запуска на хосте (127.0.0.1)
└── Cogni.sln                # Файл решения Visual Studio / Rider
```

### Пояснения к основным модулям:

* **`Cogni`** — ядро системы. Отвечает за регистрацию и авторизацию пользователей, хранение профилей, прохождение тестов MBTI, ленту постов, публикации статей, каталог категорий хобби и тегов, управление списком друзей и подписчиков.
* **`ChatService`** — изолированный сервис для обмена сообщениями. Использует SignalR (WebSockets) для мгновенной доставки сообщений, Redis для хранения активных соединений и состояний, RabbitMQ для брокера событий и MinIO для хранения медиафайлов и вложений сообщений.
* **`Shared`** — разделяемый проект, исключающий дублирование кода. Содержит общую схему базы данных PostgreSQL (`CogniDbContext`), описание всех 17+ сущностей и общую логику подписания и проверки JWT-токенов.
* **`ChatDevFrontend`** — легковесный стенд, позволяющий разработчикам тестировать подключение к хабу, обмен сообщениями и загрузку файлов без необходимости поднимать полномасштабный клиентский интерфейс.

---

## 🗄️ Миграции и тестовые данные

### Автоматическое применение миграций
В dev-конфигурации для `dev_cogni` установлена переменная окружения `MIGRATE=true`. При запуске контейнера Entity Framework Core автоматически применяет все новые миграции к базе данных PostgreSQL.

### Создание новой миграции
Для создания миграции требуется установленная утилита `dotnet-ef`. Выполните команду из корня `CogniProject`:

```powershell
dotnet ef migrations add <MigrationName> --project Cogni/Cogni.csproj --startup-project Cogni/Cogni.csproj
```

При следующем запуске `dev_cogni` миграция применится автоматически.  
Применить её вручную к запущенной БД можно командой:

```powershell
dotnet ef database update --project Cogni/Cogni.csproj --startup-project Cogni/Cogni.csproj
```

### Наполнение базы тестовыми данными (Seeding)
После запуска контейнера `dev_cogni` наполните таблицы демонстрационными данными:

```powershell
.\Cogni\seed-dev.ps1 -Password "NewDemoPassword"
```

* Скрипт **идемпотентный**: повторный вызов не создает дубликатов.
* Перед заполнением скрипт автоматически валидирует и применяет миграции.
* Создаёт данные во всех таблицах базы (пользователи, профили, статьи, посты, теги и категории хобби). Фотографии подставляются тестовыми ссылками (аккаунт Uploadcare для локального сидирования не требуется).
* По умолчанию для всех аккаунтов задаётся пароль: `CogniDemo123!` (или значение, переданное в параметре `-Password`).

#### Предустановленные тестовые пользователи:
* `alex.demo@cogni.local`
* `maya.demo@cogni.local`
* `anna.demo@cogni.local`
* `ivan.demo@cogni.local`
* `olga.demo@cogni.local`
* `max.demo@cogni.local`
* `kate.demo@cogni.local`
* `dmitry.demo@cogni.local`
