using PasswordManager.Contracts.DTOs;
using Refit;

namespace PasswordManager.SDK;

public interface IPasswordManagerApi
{
    /// <summary>
    /// Gets a single credential from API
    /// </summary>
    /// <param name="id">The ID of the credential to be retrieved</param>
    /// <returns>HTTP Response from API, including the Data, if found</returns>
    [Get("/api/Credentials/{id}")]
    Task<ApiResponse<GetSingleCredentialResponse>> GetSingleCredential(int id);

    /// <summary>
    /// Gets all credentials for current Subject logged in
    /// </summary>
    /// <param name="subjectId">The user's ID in IdentityServer</param>
    /// <returns>HTTP Response from API, including the Data, if found</returns>
    [Get("/api/Credentials")]
    Task<ApiResponse<GetAllCredentialsResponse>> GetAllCredentials([Query] string subjectId);

    /// <summary>
    /// Creates a new credential to be saved in API
    /// </summary>
    /// <param name="requestBody">HTTP Payload</param>
    /// <returns>HTTP Response from API, including the newly created record if success</returns>
    [Post("/api/Credentials")]
    Task<ApiResponse<CreateCredentialResponse>> CreateCredential([Body] CreateCredentialRequest requestBody);

    /// <summary>
    /// Updates an existing credential in API
    /// </summary>
    /// <param name="id">The ID of the credential to be updated</param>
    /// <param name="requestBody">HTTP Payload</param>
    /// <returns>HTTP Response from API, including the updated record if success</returns>
    [Put("/api/Credentials/{id}")]
    Task<ApiResponse<UpdateCredentialResponse>> UpdateCredential(int id, [Body] UpdateCredentialRequest requestBody);

    /// <summary>
    /// Deletes an existing Credential in API
    /// </summary>
    /// <param name="id">The ID of the credential to be deleted</param>
    /// <returns>HTTP Response from API</returns>
    [Delete("/api/Credentials/{id}")]
    Task<IApiResponse> DeleteCredential(int id);
}
