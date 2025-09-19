using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.BulkReplayAllMessages;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Queries.GetMessages;
using SFA.DAS.Tools.Servicebus.Support.Application.Services;
using SFA.DAS.Tools.Servicebus.Support.Domain.Queue;
using SFA.DAS.Tools.Servicebus.Support.Infrastructure.Services;

namespace SFA.DAS.Tools.Servicebus.Support.Application.UnitTests.Queue.Commands.BulkReplayAllMessages;

public class WhenHandlingBulkReplayAllMessagesCommand
{
    private readonly Mock<IQueryHandler<GetMessagesQuery, GetMessagesQueryResponse>> _mockGetMessagesQuery;
    private readonly Mock<IMessageService> _mockMessageService;
    private readonly Mock<ILogger<BulkReplayAllMessagesCommandHandler>> _mockLogger;
    private readonly BulkReplayAllMessagesCommandHandler _sut;

    public WhenHandlingBulkReplayAllMessagesCommand()
    {
        _mockGetMessagesQuery = new Mock<IQueryHandler<GetMessagesQuery, GetMessagesQueryResponse>>();
        _mockMessageService = new Mock<IMessageService>();
        _mockLogger = new Mock<ILogger<BulkReplayAllMessagesCommandHandler>>();
        _sut = new BulkReplayAllMessagesCommandHandler(_mockGetMessagesQuery.Object, _mockMessageService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task ShouldProcessAllMessagesInBatches()
    {
        // Arrange
        var command = new BulkReplayAllMessagesCommand
        {
            UserId = "test-user",
            QueueName = "test-queue",
            BatchSize = 2
        };

        var messages1 = new List<QueueMessage>
        {
            new QueueMessage { Id = "1", Body = "Message 1" },
            new QueueMessage { Id = "2", Body = "Message 2" }
        };

        var messages2 = new List<QueueMessage>
        {
            new QueueMessage { Id = "3", Body = "Message 3" }
        };

        var emptyMessages = new List<QueueMessage>();

        _mockGetMessagesQuery.SetupSequence(x => x.Handle(It.IsAny<GetMessagesQuery>()))
            .ReturnsAsync(new GetMessagesQueryResponse { Messages = messages1 })
            .ReturnsAsync(new GetMessagesQueryResponse { Messages = messages2 })
            .ReturnsAsync(new GetMessagesQueryResponse { Messages = emptyMessages });

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.Success.Should().BeTrue();
        result.TotalMessagesProcessed.Should().Be(3);
        result.TotalBatchesProcessed.Should().Be(2);

        _mockMessageService.Verify(x => x.ReplayMessages(It.IsAny<IEnumerable<QueueMessage>>(), "test-queue"), Times.Exactly(2));
    }

    [Test]
    public async Task ShouldHandleEmptyResult()
    {
        // Arrange
        var command = new BulkReplayAllMessagesCommand
        {
            UserId = "test-user",
            QueueName = "test-queue",
            BatchSize = 100
        };

        _mockGetMessagesQuery.Setup(x => x.Handle(It.IsAny<GetMessagesQuery>()))
            .ReturnsAsync(new GetMessagesQueryResponse { Messages = new List<QueueMessage>() });

        // Act
        var result = await _sut.Handle(command);

        // Assert
        result.Success.Should().BeTrue();
        result.TotalMessagesProcessed.Should().Be(0);
        result.TotalBatchesProcessed.Should().Be(0);

        _mockMessageService.Verify(x => x.ReplayMessages(It.IsAny<IEnumerable<QueueMessage>>(), It.IsAny<string>()), Times.Never);
    }
}