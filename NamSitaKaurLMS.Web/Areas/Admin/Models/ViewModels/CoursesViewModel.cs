namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class CoursesViewModel
    {

        public string Title { get; set; }           // Kurs başlığı
        public string Slug { get; set; }            // URL dostu isim
        public string ThumbnailUrl { get; set; }     // Kapak görseli
        public string Level { get; set; }            // Beginner / Intermediate / Advanced
        public string Category { get; set; }         // Yoga, Meditasyon, Pranayama…
        public decimal Price { get; set; }           // Satış fiyatı
        public bool IsFree { get; set; }             // Ücretsiz kurs mı?
        public int DurationMinutes { get; set; }     // Toplam süre
        public string Language { get; set; }         // TR / EN gibi
        public bool IsPublished { get; set; }        //Kurs yayınlandı mı?
        public string Status { get; set; }             // 0: Planlanan, 1: Devam Eden, 2: Tamamlandı
        public string StartDate { get; set; }     // Kurs başlangıç tarihi
        public string EndDate { get; set; }       // Kurs bitiş tarihi
        public int Quota { get; set; }              // Maksimum öğrenci sayısı
        public string Environment { get; set; }     // Ortam tipi (Online, Offline, Hybrid)
    }
}
