using FluentValidation;
using HashidsNet;
using MediatR;
using UrlShortenerService.Application.Common.Exceptions;
using UrlShortenerService.Application.Common.Interfaces;

namespace UrlShortenerService.Application.Url.Commands;

public record RedirectToUrlCommand : IRequest<string>
{
    public string ShortenedUrl { get; init; } = default!;
}

public class RedirectToUrlCommandValidator : AbstractValidator<RedirectToUrlCommand>
{
    public RedirectToUrlCommandValidator()
    {
        _ = RuleFor(v => v.ShortenedUrl)
          .NotEmpty()
          .WithMessage("Id is required.");
    }
}

public class RedirectToUrlCommandHandler : IRequestHandler<RedirectToUrlCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IHashids _hashids;

    public RedirectToUrlCommandHandler(IApplicationDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    public async Task<string> Handle(RedirectToUrlCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        try
        {
            // TODO: check on how decodelong works re: multiple encoded ids
            var requestedId = _hashids.DecodeLong(request.ShortenedUrl).First();
            var urlEntity = _context.Urls.First(o => o.Id == requestedId);
            return urlEntity.OriginalUrl;
        }
        catch (Exception)
        {
            throw new NotFoundException($"{request.ShortenedUrl} not found");
        }
    }
}

// when id doesn't exist:
// n exception occurred while iterating over the results of a query for context type 'UrlShortenerService.Infrastructure.Persistence.ApplicationDbContext'.
//   System.InvalidOperationException: Sequence contains no elements
