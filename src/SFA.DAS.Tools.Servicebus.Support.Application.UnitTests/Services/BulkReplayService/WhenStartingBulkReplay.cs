using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Tools.Servicebus.Support.Application.Queue.Commands.BulkReplayAllMessages;

namespace SFA.DAS.Tools.Servicebus.Support.Application.UnitTests.Services.BulkReplayService;

public class WhenStartingBulkReplay
{
    private readonly Mock<ICommandHandler<BulkReplayAllMessagesCommand, BulkReplayAllMessagesCommandResponse>> _mockCommandHandler;
    private readonly Mock<ILogger<SFA.DAS.Tools.Servicebus.Support.Application.Services.BulkReplayService>> _mockLogger;
    private readonly SFA.DAS.Tools.Servicebus.Support.Application.Services.BulkReplayService _sut;

    public WhenStartingBulkReplay()
    {
        _mockCommandHandler = new Mock<ICommandHandler<BulkReplayAllMessagesCommand, BulkReplayAllMessagesCommandResponse>>();
        _mockLogger = new Mock<ILogger<SFA.DAS.Tools.Servicebus.Support.Application.Services.BulkReplayService>>();
        _sut = new SFA.DAS.Tools.Servicebus.Support.Application.Services.BulkReplayService(_mockCommandHandler.Object, _mockLogger.Object);
    }

    [Test]
    public async Task ShouldReturnOperationId()
    {
        // Arrange
        var userId = "test-user";
        var queueName = "test-queue";
        var batchSize = 100;

        _mockCommandHandler.Setup(x => x.Handle(It.IsAny<BulkReplayAllMessagesCommand>()))
            .ReturnsAsync(new BulkReplayAllMessagesCommandResponse
            {
                Success = true,
                TotalMessagesProcessed = 50,
                TotalBatchesProcessed = 1
            });

        // Act
        var result = await _sut.StartBulkReplayAsync(userId, queueName, batchSize);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task ShouldReturnStatusForValidOperationId()
    {
        // Arrange
        var userId = "test-user";
        var queueName = "test-queue";
        var batchSize = 100;

        _mockCommandHandler.Setup(x => x.Handle(It.IsAny<BulkReplayAllMessagesCommand>()))
            .ReturnsAsync(new BulkReplayAllMessagesCommandResponse
            {
                Success = true,
                TotalMessagesProcessed = 50,
                TotalBatchesProcessed = 1
            });

        var operationId = await _sut.StartBulkReplayAsync(userId, queueName, batchSize);

        // Act
        var status = await _sut.GetBulkReplayStatusAsync(operationId);

        // Assert
        status.Should().NotBeNull();
        status.OperationId.Should().Be(operationId);
        status.Status.Should().Be("Running");
    }

    [Test]
    public async Task ShouldReturnNotFoundForInvalidOperationId()
    {
        // Arrange
        var invalidOperationId = "invalid-id";

        // Act
        var status = await _sut.GetBulkReplayStatusAsync(invalidOperationId);

        // Assert
        status.Should().NotBeNull();
        status.OperationId.Should().Be(invalidOperationId);
        status.Status.Should().Be("NotFound");
        status.ErrorMessage.Should().Be("Operation not found");
    }
}