using System;
using System.Collections;
using UnityEngine;

namespace Assets.REST_Client.Scripts
{
    /// <summary>
    /// Base Rest Exception that all custom exceptions for the REST Client derive from
    /// </summary>
    public class RestException : Exception
    {
        /// <summary>
        /// HTTP Status Code
        /// </summary>
        public int Status;

        /// <summary>
        /// HashTable of Errors
        /// </summary>
        public Hashtable ErrorRaw;

        /// <summary>
        /// List of Errors
        /// </summary>
        public ArrayList ErrorListRaw;

        /// <summary>
        /// Tag Associated with Request
        /// </summary>
        public string Tag;

        /// <summary>
        /// Constructor that takes information from more specific exception and populates it into the correct fields of the base class.
        /// </summary>
        /// <param name="status">HTTP Status Code</param>
        /// <param name="error">Boxed object containing the errors</param>
        /// <param name="tag">Tag Associated with Request</param>
        public RestException(int status, object error, string tag)
        {
            Status = status;
            Tag = tag;

            if (error is ArrayList)
            {
                ErrorListRaw = (ArrayList)error;
            }
            else if (error is Hashtable)
            {
                ErrorRaw = (Hashtable)error;
            }
            else if (error is string)
            {
                ErrorListRaw = new ArrayList
                {
                    error
                };
            }

            else if (error != null)
            {
                Debug.LogWarning("Unsupported type in response: " + error.GetType());
            }

            // ReSharper disable once VirtualMemberCallInConstructor
            Parse();
        }

        protected virtual void Parse()
        {
        }

        /// <summary>
        /// Convert the error information into a human readable string
        /// </summary>
        /// <returns>human readable string</returns>
        public override string ToString()
        {
            string result = "tag: " + Tag + ", status: " + Status + ", raw: ";
            if (ErrorRaw != null)
            {
                result += ErrorRaw.ToString();
            }
            else if (ErrorListRaw != null)
            {
                result += ErrorListRaw.ToString();
            }
            else
            {
                result += "<EMPTY>";
            }

            return result;
        }
    }

    /// <summary>
    /// Exception thrown when the server responds with 400 error code
    /// </summary>
    public class BadRequestException : RestException
    {
        /// <summary>
        /// Constructor used to create exception
        /// </summary>
        /// <param name="status">HTTP Status Code</param>
        /// <param name="error">Boxed object containing the errors</param>
        /// <param name="tag">Tag Associated with Request</param>
        public BadRequestException(int status, object error, string tag) : base(status, error, tag)
        {
        }

        protected override void Parse()
        {
            // TODO: Implement
        }
    }

    /// <summary>
    /// Exception thrown when the server responds with 401 error code
    /// </summary>
    public class UnauthorizedException : RestException
    {
        public new string Message;

        /// <summary>
        /// Constructor used to create exception
        /// </summary>
        /// <param name="status">HTTP Status Code</param>
        /// <param name="error">Boxed object containing the errors</param>
        /// <param name="tag">Tag Associated with Request</param>
        public UnauthorizedException(int status, object error, string tag) : base(status, error, tag)
        {
        }

        protected override void Parse()
        {
            Message = ErrorRaw["message"] as string;
        }
    }

    /// <summary>
    /// Exception thrown when the server responds with 403 error code
    /// </summary>
    public class ForbiddenException : RestException
    {
        /// <summary>
        /// Constructor used to create exception
        /// </summary>
        /// <param name="status">HTTP Status Code</param>
        /// <param name="error">Boxed object containing the errors</param>
        /// <param name="tag">Tag Associated with Request</param>
        public ForbiddenException(int status, object error, string tag) : base(status, error, tag)
        {
        }

        protected override void Parse()
        {
            // TODO: Implement
        }
    }

    /// <summary>
    /// Exception thrown when the server responds with 404 error code
    /// </summary>
    public class NotFoundException : RestException
    {
        /// <summary>
        /// Constructor used to create exception
        /// </summary>
        /// <param name="status">HTTP Status Code</param>
        /// <param name="error">Boxed object containing the errors</param>
        /// <param name="tag">Tag Associated with Request</param>
        public NotFoundException(int status, object error, string tag) : base(status, error, tag)
        {
        }

        protected override void Parse()
        {
            // TODO: Implement
        }
    }

    /// <summary>
    /// Exception thrown when the server responds with 500 error code
    /// </summary>
    public class ServerInternalErrorException : RestException
    {
        /// <summary>
        /// Constructor used to create exception
        /// </summary>
        /// <param name="status">HTTP Status Code</param>
        /// <param name="error">Boxed object containing the errors</param>
        /// <param name="tag">Tag Associated with Request</param>
        public ServerInternalErrorException(int status, object error, string tag) : base(status, error, tag)
        {
        }

        protected override void Parse()
        {
            // TODO: Implement
        }
    }

    /// <summary>
    /// Exception thrown when unable to communicate with the server
    /// </summary>
    public class ServerNotAvailableException : RestException
    {
        /// <summary>
        /// Constructor used to create exception
        /// </summary>
        /// <param name="tag">Tag Associated with Request</param>
        public ServerNotAvailableException(string tag) : base(-1, null, tag)
        {
        }
    }

    /// <summary>
    /// Exception thrown when we are unable to parse the response as valid json
    /// </summary>
    public class WrongResponseFormatException : RestException
    {
        public string UnparsedResponse;

        /// <summary>
        /// Constructor used to create exception
        /// </summary>
        /// <param name="error">Boxed object containing the errors</param>
        /// <param name="tag">Tag Associated with Request</param>
        public WrongResponseFormatException(object error, string tag) : base(-2, null, tag)
        {
            UnparsedResponse = error as string;
        }
    }

    /// <summary>
    /// Exception thrown when an unknown http protocol error has occured
    /// </summary>
    public class HttpErrorException : RestException
    {
        public string UnparsedResponse;

        /// <summary>
        /// Constructor used to create exception
        /// </summary>
        /// <param name="error">Boxed object containing the errors</param>
        /// <param name="tag">Tag Associated with Request</param>
        public HttpErrorException(object error, string tag) : base(-3, error, tag)
        {
            UnparsedResponse = error as string;
        }
    }

}
