﻿namespace NEWS.Entities.Models.Responses
{
    public class ErrorResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorResult(int statusCode, string message) { StatusCode = statusCode; Message = message; }

        public ErrorResult(string message) { Message = message; }
    }
}
