using Microsoft.Extensions.Logging;
using Moq;
using NoRslinx.Application.Common.Behaviours;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Application.TodoItems.Commands.CreateTodoItem;
using NUnit.Framework;

namespace NoRslinx.Application.UnitTests.Common.Behaviours;
public class RequestLoggerTests
{
    private Mock<ILogger<CreateTodoItemCommand>> _logger = null!;


    [SetUp]
    public void Setup()
    {
        _logger = new Mock<ILogger<CreateTodoItemCommand>>();

    }

    [Test]
    public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
    {


        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());


    }

    [Test]
    public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
    {
        var requestLogger = new LoggingBehaviour<CreateTodoItemCommand>(_logger.Object);

        await requestLogger.Process(new CreateTodoItemCommand { ListId = 1, Title = "title" }, new CancellationToken());


    }
}
