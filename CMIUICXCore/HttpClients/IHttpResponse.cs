namespace SBCommon.HttpClients.New
{
    public interface IHttpResponse
    {
        HttpResponse HandleResponse(IResponseHandler responseHandler);
    }
}