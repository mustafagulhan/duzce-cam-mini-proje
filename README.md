# Mini Proje Takip Uygulaması

Basit proje ve görev yönetimi için geliştirilen örnek uygulama.

- Backend: .NET 8 Web API, EF Core, SQLite (isteğe bağlı InMemory)
- Frontend: Angular (routing + HttpClient), basit bileşenler ve formlar

## Gereksinimler
- .NET SDK 8.x (6+ da çalışır, önerilen 8)
- Node.js 18+
- Angular CLI (opsiyonel; `npx` ile de çalıştırabilirsiniz)

## Kurulum ve Çalıştırma

### Backend
```bash
# kök dizinde
 dotnet restore
 dotnet build
 dotnet run --project src/Api/Api.csproj
# Çalışınca: http://localhost:5248
```

- Veritabanı: Varsayılan SQLite, dosya `src/Api/app.db` altında oluşur.
- InMemory DB için `appsettings.json` içinde `"USE_INMEMORY": true` ayarlayabilirsiniz.

Swagger/OpenAPI:
- JSON: `http://localhost:5248/openapi/v1.json`
- UI: `http://localhost:5248/swagger`

### Frontend (Angular)
```bash
cd client
npm install
npm run start  # veya: npx ng serve -o
# Uygulama: http://localhost:4200
```

- `client/src/environments/environment.ts` içinde `apiUrl` backend portuna göre ayarlı: `http://localhost:5248/api`
- CORS, `http://localhost:4200` için açıktır.

## Ana Özellikler
- Proje listesi ve yeni proje ekleme
- Proje detayında görev listesi ve yeni görev ekleme
- Görevleri tamamlandı olarak işaretleme (toggle)

## API Uçları (özet)
- Projects
  - GET `/api/projects`
  - GET `/api/projects/{id}`
  - POST `/api/projects`
  - PUT `/api/projects/{id}`
  - DELETE `/api/projects/{id}`
  - GET `/api/projects/{id}/tasks`
  - POST `/api/projects/{id}/tasks`
- Tasks
  - GET `/api/tasks/{id}`
  - PUT `/api/tasks/{id}`
  - PATCH `/api/tasks/{id}/complete` body: `{ isCompleted: boolean }`
  - DELETE `/api/tasks/{id}`

## Mimari
- Katmanlar: `Domain`, `Application` (service & DTO), `Infrastructure` (EF & repo), `Api`
- Repository + Service kalıbı, SOLID’e uygun sade yapı

## Geliştirme Notları
- Development modunda örnek seed verisi otomatik eklenir (1 proje, birkaç görev)
- SQLite dosyası silinirse `EnsureCreated` ile tablo ve seed tekrar oluşturulur

## Lisans
Teknik değerlendirme amacıyla hazırlanmıştır.
