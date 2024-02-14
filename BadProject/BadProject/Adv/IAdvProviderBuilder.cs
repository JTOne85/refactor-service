using BadProject.Errors;
using ThirdParty;

namespace Adv
{
    public interface IAdvProviderBuilder
    {
        Advertisement BuildProvider(Advertisement adv, IErrorManager errorManager);
        Advertisement CreateAdvProvider(string id, bool? useBackupProvider);
    }
}