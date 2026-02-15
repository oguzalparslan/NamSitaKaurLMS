namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models
{
    public record AjaxResponse(
    bool Success,
    string? Message = null,
    string? RedirectUrl = null,
    int? CourseId = null,
    bool? Redirected = null
);

}
