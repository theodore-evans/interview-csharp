namespace UrlShortenerService.Api.Endpoints.Url.Requests;

/// <summary>
/// Request model for the <see cref="UrlShortenerService.Api.Endpoints.Url.ShortenUrlEndpoint"/> endpoint.
/// </summary>
public class RedirectToUrlRequest
{
    /// <summary>
    /// The shortened URL.
    /// </summary>
    public string shortenedUrl { get; set; } = default!;
}
