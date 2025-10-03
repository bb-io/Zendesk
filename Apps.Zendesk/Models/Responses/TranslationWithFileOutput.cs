using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Models.Responses;
public class TranslationWithFileOutput : SingleTranslation, IDownloadContentOutput
{
    [Display("Content")]
    public FileReference Content { get; set; }
}
