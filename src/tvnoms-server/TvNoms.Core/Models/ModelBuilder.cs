using AutoMapper;
using Humanizer;
using TvNoms.Core.Entities;
using TvNoms.Core.Extensions.Identity;
using TvNoms.Core.FileStorage;
using TvNoms.Core.Models.Users;
using TvNoms.Core.Repositories;
using TvNoms.Core.Utilities;

namespace TvNoms.Core.Models;

public interface IModelBuilder {
  // User
  Task<UserWithSessionModel> BuildAsync(User user, UserSessionInfo session,
    CancellationToken cancellationToken = default);

  Task<UserModel> BuildAsync(User user, CancellationToken cancellationToken = default);
  Task<UserPageModel> BuildAsync(IPageable<User> users, CancellationToken cancellationToken = default);
}

public class ModelBuilder : IModelBuilder {
  private readonly IMapper _mapper;
  private readonly IFileStorage _fileStorage;
  private readonly IUserRepository _userRepository;
  private readonly IMediaRepository _mediaRepository;
  private readonly IClientRepository _clientRepository;

  public ModelBuilder(IMapper mapper, IFileStorage fileStorage, IUserRepository userRepository,
    IMediaRepository mediaRepository, IClientRepository clientRepository) {
    _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    _mediaRepository = mediaRepository ?? throw new ArgumentNullException(nameof(mediaRepository));
    _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
  }

  public async Task<UserWithSessionModel> BuildAsync(User user, UserSessionInfo session,
    CancellationToken cancellationToken = default) {
    ArgumentNullException.ThrowIfNull(user);
    ArgumentNullException.ThrowIfNull(session);

    var model = _mapper.Map(session, _mapper.Map<UserWithSessionModel>(user));
    model.Online = await _clientRepository.AnyAsync(_ => _.Active && _.UserId == user.Id, cancellationToken);
    model.Roles = (await _userRepository.GetRolesAsync(user, cancellationToken)).Select(_ => _.Camelize()).ToList();
    if ((user.Avatar = user.AvatarId != null ? await _mediaRepository.GetByIdAsync(user.AvatarId.Value) : null) !=
        null) {
      model.AvatarUrl = await _fileStorage.GetPublicUrlAsync(user.Avatar.Path, cancellationToken);
    }

    return model;
  }

  public async Task<UserModel> BuildAsync(User user, CancellationToken cancellationToken = default) {
    if (user == null) throw new ArgumentNullException(nameof(user));

    var model = _mapper.Map<UserModel>(user);

    model.Online = await _clientRepository.AnyAsync(_ => _.Active && _.UserId == user.Id, cancellationToken);
    model.Roles = (await _userRepository.GetRolesAsync(user, cancellationToken)).Select(_ => _.Camelize()).ToList();

    if ((user.Avatar = user.AvatarId != null ? await _mediaRepository.GetByIdAsync(user.AvatarId.Value) : null) !=
        null) {
      model.AvatarUrl = await _fileStorage.GetPublicUrlAsync(user.Avatar.Path, cancellationToken);
    }

    return model;
  }

  public async Task<UserPageModel> BuildAsync(IPageable<User> users, CancellationToken cancellationToken = default) {
    if (users == null) throw new ArgumentNullException(nameof(users));

    var items = new List<UserModel>();

    foreach (var user in users) {
      items.Add(await BuildAsync(user, cancellationToken));
    }

    var listModel = new UserPageModel {
      Items = items,
      Offset = users.Offset,
      Limit = users.Limit,
      Length = users.Length,
      Previous = users.Previous,
      Next = users.Next
    };
    return listModel;
  }
}
