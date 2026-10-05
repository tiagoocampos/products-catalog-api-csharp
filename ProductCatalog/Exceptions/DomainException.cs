using System;

namespace ProductCatalog.Exceptions;

public class DomainException : Exception
{   
    public DomainException(string message) : base(message){ }
}
