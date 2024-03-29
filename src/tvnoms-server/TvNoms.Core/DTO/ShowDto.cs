using TvNoms.Core.MediaLookup;

namespace TvNoms.Core.DTO;

using AbstractProfile = AutoMapper.Profile;

public class ShowDto {
  public string Title { get; set; }
  public long TheMovieDbId { get; set; }

  public float Popularity { get; set; }
  public float AverageVote { get; set; }
  public long VoteCount { get; set; }
  public string BackdropImage { get; set; }
  public string PosterImage { get; set; }
  public DateTime FirstAired { get; set; }
  public bool IsAdult { get; set; }

  public long[] GenreIds { get; set; }
}

public class ShowDtoProfile : AbstractProfile {
  public ShowDtoProfile() {
    CreateMap<RemoteShowModel, ShowDto>()
      .ForMember(src => src.PosterImage,
        dest => dest.MapFrom(e => $"https://image.tmdb.org/t/p/w1280{e.PosterImage}"))
      .ForMember(src => src.BackdropImage,
        dest => dest.MapFrom(e => $"https://image.tmdb.org/t/p/w1280{e.BackdropImage}"));
  }
}

public class ShowPageModel {
  public long Offset { get; set; }

  public int Limit { get; set; }

  public long Length { get; set; }

  public long? Previous { get; set; }

  public long? Next { get; set; }

  public IList<ShowDto> Items { get; set; } = new List<ShowDto>();
}
