using System.Net.Http.Json;

namespace SmartAuth.Infrastructure.Identity;

public class KeyCloakClient(HttpClient httpClient)
{
    public async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(
            "users",
            user,
            cancellationToken);
        
        httpResponseMessage.EnsureSuccessStatusCode();

        return ExtractIdentityFromLocationHeader(httpResponseMessage);
    }

    private static string ExtractIdentityFromLocationHeader(HttpResponseMessage httpResponseMessage)
    {
        const string userSegmentName = "users/";

        string? locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery;
        if(locationHeader is null)
        {
            throw new InvalidOperationException("Location header is null");
        }

        int userSegmentNameIndex = locationHeader.IndexOf(
            userSegmentName,
            StringComparison.InvariantCultureIgnoreCase);

        string identityId = locationHeader.Substring(userSegmentNameIndex + userSegmentName.Length);

        return identityId;
    }
}
