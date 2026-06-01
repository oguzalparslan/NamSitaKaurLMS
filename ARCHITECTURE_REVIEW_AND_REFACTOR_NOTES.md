# NamSitaKaur LMS - Mimari İnceleme ve Refactor Notları

## Kapsam

Bu paket, mevcut veritabanı yapısını bozmadan proje kodunun daha profesyonel bir katmanlı mimari yaklaşımına yaklaştırılması için düzenlenmiştir. Migration dosyaları korunmuştur; yeni migration üretilmemiştir. Amaç, aynı DB üzerinde çalışabilecek şekilde business logic, validation, Identity akışı, repository/service ayrımı ve admin ekranlarında tablo standardını iyileştirmektir.

> Not: Bu çalışma ChatGPT sandbox ortamında yapıldı. Ortamda `dotnet` CLI bulunmadığı için `dotnet restore/build` çalıştırılamadı. Dosya/sözdizimi bütünlüğü statik taramalarla kontrol edildi; projeyi kendi makinenizde Visual Studio veya `dotnet build` ile derleyip son küçük uyarlamaları yapmanız gerekir.

---

## 1. Mimari Analiz

### Mevcut yapı

Projede 4 ana katman bulunuyor:

- `NamSitaKaurLMS.Core`: Entity, DTO ve repository interface tanımları.
- `NamSitaKaurLMS.Infrastructure`: EF Core DbContext, migration, concrete repository ve UnitOfWork.
- `NamSitaKaurLMS.Application`: Service interface ve business service implementasyonları.
- `NamSitaKaurLMS.Web`: MVC controller, view model, Razor view, admin/public UI.

Bu ayrım doğru bir başlangıç. Fakat önceki halinde service katmanı çoğunlukla repository passthrough gibi davranıyordu. Business kuralları controller içinde veya hiç yoktu. Bu durum özellikle kurs oluşturma, katılımcı ekleme, ders ekleme ve içerik ekleme işlemlerinde veri tutarsızlığına sebep olabilirdi.

### Yapılan mimari düzenlemeler

- Service katmanına business validation eklendi.
- Controller seviyesindeki tekrarlı ve riskli kontroller azaltıldı.
- Repository metotları async EF Core kullanımına yaklaştırıldı.
- Generic repository artık aktif kayıtları filtreliyor ve tarih alanlarını normalize ediyor.
- UnitOfWork namespace altına alındı ve DI kullanımı netleştirildi.
- Business hataları için `BusinessRuleException` eklendi.

---

## 2. Business Logic Analizi

### Kritik eski problemler

- `CourseService.GetAllAsync(filter)` parametreyi almıyor, tüm kursları dönüyordu.
- Kurs güncellemede detached entity update kullanıldığı için `CreatedDate`, `IsActive` gibi alanlar ezilebilirdi.
- Kurs bitiş tarihi başlangıçtan önce olabilir, negatif ücret/süre/kontenjan girilebilir durumdaydı.
- Aynı slug ile birden fazla kurs oluşturulması engellenmiyordu.
- Katılımcı eklerken kontenjan ve duplicate kayıt kontrolü yoktu.
- Ders eklerken ders tarihi kurs tarih aralığına göre kontrol edilmiyordu.
- Ders sıra numarası duplicate olabiliyordu.
- İçerik tipine göre URL/metin zorunluluğu kontrol edilmiyordu.
- Delete işlemleri FK sorununa açık hard delete yapıyordu.

### Eklenen kurallar

- Kurs:
  - Title, Slug, Level, Category, Language zorunlu.
  - Slug format ve benzersizlik kontrolü.
  - EndDate >= StartDate.
  - DurationMinutes > 0.
  - Quota > 0.
  - Price >= 0.
  - IsFree ise Price = 0.
  - Delete artık soft-delete: `IsActive=false`, `IsPublished=false`.

- Ders:
  - CourseId, Order, Title, DurationMinutes zorunlu/anlamlı.
  - LessonDate kurs tarih aralığında olmalı.
  - Aynı kursta aynı Order tekrar edemez.
  - Delete soft-delete yapar.

- Ders İçeriği:
  - ContentType yalnızca `Video`, `Audio`, `Text`, `PDF` değerlerinden biri olmalı.
  - Video/Audio/PDF için URL zorunlu.
  - Text için metin zorunlu.
  - Aynı derste aynı Order tekrar edemez.
  - Delete soft-delete yapar.

- Katılımcı/Kurs:
  - Aynı kullanıcı aynı kursa tekrar eklenemez.
  - Kalan kontenjan kontrol edilir.
  - UserCourse insert sırasında tarih, progress ve active alanları set edilir.

---

## 3. İlişkisel Yapı Analizi

### Mevcut tablo/ilişki yaklaşımı

Veri modeli LMS domain için mantıklı:

- Course -> Lessons
- Lesson -> LessonContents
- Course -> CourseDescription
- Course -> CourseEnvironment
- AppUser -> UserCourse -> Course
- AppUser -> application profile table `Users`

### Riskli noktalar

- Identity `AspNetUsers` ile domain `Users` tablosu ayrılmış. Bu çalışabilir ama naming kafa karıştırabilir. Uzun vadede `UserProfile` gibi bir isim daha iyi olurdu. DB aynı kalsın istendiği için tablo/entity adı değiştirilmedi.
- `UserCourse` için uygulama seviyesinde duplicate kontrolü eklendi. DB seviyesinde unique index önerildi fakat yeni migration oluşturulmadı.
- DeleteBehavior daha kontrollü hale getirildi. Kurs/ders/içerik silmede soft-delete tercih edildiği için FK kırılma riski azaltıldı.

### DbContext düzenlemeleri

`OnModelCreating` içinde temel property length, unique index ve ilişki tanımları daha açık hale getirildi. Bunlar model tarafını güçlendirir; var olan DB’ye otomatik migration uygulanmadığı sürece DB şemasını değiştirmez.

---

## 4. Identity Analizi

### Mevcut durum

- ASP.NET Core Identity doğru şekilde `AppUser`, `IdentityRole`, `IdentityDbContext` ile kurulmuş.
- Admin role authorization mevcut.
- Cookie ayarları tanımlı.
- Seed mekanizması var.

### Düzeltilen noktalar

- Login akışındaki `Redirect(returnUrl)` için `return` eksikti; düzeltildi.
- Hatalı login mesajları güvenlik açısından genelleştirildi.
- Lockout durumu kullanıcıya düzgün dönüyor.
- Logout GET yerine POST + AntiForgery olacak şekilde düzenlendi.
- Admin seed işlemi artık `SeedAdminData:username` değerini dikkate alıyor.
- Seed edilen admin için domain `Users` profil kaydı da oluşturuluyor.
- Kullanıcı update işlemlerinde `.Result` / `.Wait()` kaldırılıp async hale getirildi.
- Role update işleminde current role null ise patlayabilecek kod güvenli hale getirildi.
- User delete redirect typo düzeltildi: `GelAllUsers` -> `GetAllUsers`.

---

## 5. Validation Çalışmaları

### DTO/ViewModel seviyesinde

Aşağıdaki modellere DataAnnotation ve/veya `IValidatableObject` kontrolleri eklendi:

- `CourseDto`
- `AddUsersToCourseDto`
- `CreateLessonViewModel`
- `CreateLessonContentViewModel`
- `CreateUserViewModel`
- `UpdateUserViewModel`
- `UpdateUserRoleViewModel`
- `LoginViewModel`

### Service seviyesinde

UI validasyonu atlatılsa bile iş kurallarının server tarafında çalışması için service katmanına tekrar kontrol eklendi. Bu, profesyonel mimaride önemli bir noktadır; controller veya JavaScript validasyonuna güvenilmez.

---

## 6. Admin UI / Table Standardizasyonu

### Eski durum

Admin ekranlarında table class kullanımı dağınıktı:

- `table table-bordered`
- `table table-bordered table-hover`
- `table table-bordered table-striped`
- Bazı ekranlarda inline style vardı.

### Yapılan düzenleme

Yeni ortak CSS dosyası eklendi:

- `NamSitaKaurLMS.Web/wwwroot/css/components/tables.css`

Admin layout içine dahil edildi. Admin tablolarında `admin-table` ve `admin-table-wrapper` standardı uygulanmaya başlandı.

Güncellenen ekranlar:

- `Areas/Admin/Views/Dashboard/Courses.cshtml`
- `Areas/Admin/Views/Dashboard/CoursesForLesson.cshtml`
- `Areas/Admin/Views/Dashboard/CreateCourseLesson.cshtml`
- `Areas/Admin/Views/Dashboard/GetAllUsers.cshtml`
- `Areas/Admin/Views/Dashboard/GetSystemSettings.cshtml`
- `Areas/Admin/Views/Course/ActiveCoursePartial.cshtml`
- `Areas/Admin/Views/Course/DeactiveCourse.cshtml`

Ayrıca RAR arşivinden bazı sıkıştırılmış vendor CSS dosyaları çıkarılamadığı için minimal `sb-admin-2.css/min.css` stub dosyası oluşturuldu. Bootstrap 5 CDN kullanımı korunmuştur.

---

## 7. Önemli Güvenlik Notu

`appsettings.json` içinde gerçek SQL Server bağlantı bilgisi ve kullanıcı parolası mevcut. Paket içinde mevcut dosya korunmuştur çünkü aynı DB ile çalışma istendi. Fakat gerçek projede bu bilgi:

- User Secrets,
- environment variable,
- Azure Key Vault / secret manager,
- CI/CD secret store

üzerinden yönetilmelidir. Bu connection string canlıysa parolayı değiştirmeniz önerilir.

---

## 8. Sonraki Önerilen Adımlar

1. `dotnet restore`
2. `dotnet build`
3. Uygulamayı localde çalıştırıp login/admin akışını test etme
4. Var olan DB’de duplicate slug veya duplicate user-course kayıtları varsa temizleme
5. İsterseniz daha sonra yeni migration ile DB constraint’lerini de kalıcı hale getirme
6. Controller mapping tekrarlarını AutoMapper veya elle yazılmış mapper class’larına taşıma
7. Service result pattern veya FluentValidation entegrasyonu ile hata dönüşlerini daha sistematik hale getirme

