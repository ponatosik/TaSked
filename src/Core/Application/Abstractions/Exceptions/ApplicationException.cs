﻿namespace TaSked.Application.Exceptions;

public class ApplicationException : Exception
{
	protected ApplicationException(string message) : base(message) { }
	protected ApplicationException(string message, Exception inner) : base(message, inner) { }
}
