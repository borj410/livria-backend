namespace LivriaBackend.shared.ErrorHandling
{
    /// <summary>
    /// Represents a standardized error response structure for API errors.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// The HTTP status code of the error.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// A short, human-readable summary of the problem type.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the problem.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// An optional, unique identifier for the request, useful for correlating logs.
        /// </summary>
        public string TraceId { get; set; }
    }
}