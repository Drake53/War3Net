namespace War3Net.IO.Mpq
{
    /// <summary>
    /// Represents errors that occur when parsing an <see cref="MpqArchive"/>.
    /// </summary>
    public class MpqParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MpqParserException"/> class.
        /// </summary>
        public MpqParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqParserException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MpqParserException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MpqParserException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MpqParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}