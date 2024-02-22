using FluentValidation;
using HashidsNet;
using MediatR;
using UrlShortenerService.Application.Common.Interfaces;

namespace UrlShortenerService.Application.Url.Commands;

public record CreateShortUrlCommand : IRequest<string>
{
    public string Url { get; init; } = default!;
}

public class CreateShortUrlCommandEmptyStringValidator : AbstractValidator<CreateShortUrlCommand>
{
    public CreateShortUrlCommandEmptyStringValidator()
    {
        _ = RuleFor(v => v.Url)
          .NotEmpty()
          .WithMessage("Non empty string required.");
    }
}

public class CreateShortUrlCommandValidUrlValidator : AbstractValidator<CreateShortUrlCommand>
{
    public CreateShortUrlCommandValidUrlValidator()
    {
        _ = RuleFor(v => v.Url)
          .Must(BeAValidUrl)
          .WithMessage("Valid URL is required.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

}

public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IHashids _hashids;

    public CreateShortUrlCommandHandler(IApplicationDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    public async Task<string> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var urlEntity = new Domain.Entities.Url
        {
            OriginalUrl = request.Url
        };

        _ = _context.Urls.Add(urlEntity);
        _ = await _context.SaveChangesAsync(cancellationToken);

        var uniqueId = _hashids.EncodeLong(urlEntity.Id);

        return uniqueId;
    }
}
