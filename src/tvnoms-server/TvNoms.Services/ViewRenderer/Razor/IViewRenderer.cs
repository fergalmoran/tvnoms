namespace TvNoms.Server.Services.ViewRenderer.Razor;

public interface IViewRenderer {
  Task<string> RenderAsync(string name, object? model = null, CancellationToken cancellationToken = default);
}
