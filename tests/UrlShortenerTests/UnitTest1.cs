using UrlShortenerService.Application.Url.Commands;
using UrlShortenerService.Application.Common.Interfaces;
using HashidsNet;
using Moq;

namespace UrlShortenerTests;


// FluentAssertions
public class TestCreateShortUrlCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IHashids _hashids;

    // constructors here get executed for this test context 
    public TestCreateShortUrlCommandHandler()
    {
        _context = new Mock<IApplicationDbContext>().Object;
        var x = new Mock<IHashids>();
        _ = x.Setup(h => h.DecodeLong("foobar")).Returns(new[] { 1576L });
        _hashids = x.Object;
    }

    [Fact]
    public void Test1()
    {
        var handler = new CreateShortUrlCommandHandler(_context, _hashids);
        // mock hashids, context using Moq (e.g.)
        // create new CreateShortUrlCommand
        // var actual = handler.Handle()
        // Assert.Equal()
    }

    // add additional tests with the same setup/teardown

    // dispose method for tearing down 
}

// is it possible to NOT decode every time
// eg. store shortened url and query that hash
// algorithmic/logic details of implementation (profiler argument)
