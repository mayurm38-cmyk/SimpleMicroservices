namespace EmployeeService.DTOs
{
    /// <summary>
    /// Standard API error response.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        /// <example>404</example>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <example>Employee not found.</example>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier used for tracing the request.
        /// </summary>
        /// <example>0HN123ABC456</example>
        public string TraceId { get; set; } = string.Empty;
    }
}