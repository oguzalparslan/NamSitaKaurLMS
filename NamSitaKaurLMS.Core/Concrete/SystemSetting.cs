using NamSitaKaurLMS.Core.Abstract;

namespace NamSitaKaurLMS.Core.Concrete
{
    public class SystemSetting : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
