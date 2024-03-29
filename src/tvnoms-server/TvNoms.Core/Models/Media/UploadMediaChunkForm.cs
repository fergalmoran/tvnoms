using Microsoft.Extensions.Options;
using TvNoms.Core.FileStorage;

namespace TvNoms.Core.Models.Medias;

public class UploadMediaChunkForm : UploadMediaContentForm {
  public Guid Id { get; set; }

  public long Offset { get; set; }
}

public class UploadMediaChunkFormValidator : UploadMediaContentFormValidator<UploadMediaChunkForm> {
  public UploadMediaChunkFormValidator(IOptions<FileRuleOptions> fileTypeOptions) : base(fileTypeOptions) {
  }
}
