using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; private set; }
        [JsonIgnore]
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool Success { get; private set; }

        public List<string> Errors { get; set; }

        //Static Factory Method
        public static Response<T> Successfull(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, Success = true };
        }

        public static Response<T> Successfull(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, Success = true };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T>() { Errors = errors, StatusCode = statusCode, Success = false };
        }

        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T>() { Errors = new List<string>() { error }, StatusCode = statusCode, Success = false };
        }
    }
}
