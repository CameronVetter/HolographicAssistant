namespace Assets.REST_Client.Scripts
{
    public class RestExceptionFactory
    {
        public const int ServerNotAvailable = -1;
        public const int WrongResponseFormat = -2;
        public const int HttpError = -3;

        public static RestException Create(int status, object error, string tag)
        {
            switch (status)
            {
                case ServerNotAvailable:
                    return new ServerNotAvailableException(tag);
                case HttpError:
                    return new HttpErrorException(error, tag);
                case WrongResponseFormat:
                    return new WrongResponseFormatException(error, tag);
                case 400:
                    return new BadRequestException(status, error, tag);
                case 401:
                    return new UnauthorizedException(status, error, tag);
                case 403:
                    return new ForbiddenException(status, error, tag);
                case 404:
                    return new NotFoundException(status, error, tag);
                case 500:
                    return new ServerInternalErrorException(status, error, tag);
                default:
                    return new RestException(status, error, tag);
            }
        }
    }
}
