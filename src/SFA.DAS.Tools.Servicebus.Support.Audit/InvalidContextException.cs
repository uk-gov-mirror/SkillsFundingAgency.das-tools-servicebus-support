using System;

namespace SFA.DAS.Tools.Servicebus.Support.Audit;

public class InvalidContextException(string message) : Exception(message);