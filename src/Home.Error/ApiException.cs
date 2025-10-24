namespace Home.Error {
    public class ApiException<T> : Exception, IApiException {
        private readonly IApiError<T> Error;

        public ApiException(IApiError<T> error) {
            Error = error;
        }

        public IApiError<T> GetError() {
            return Error;
        }
    }
}
