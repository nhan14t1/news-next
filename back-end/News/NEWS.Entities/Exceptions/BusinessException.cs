﻿namespace NEWS.Entities.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message): base(message) { }
    }
}
