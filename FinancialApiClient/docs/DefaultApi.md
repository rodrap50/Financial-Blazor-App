# IO.Swagger.Api.DefaultApi

All URIs are relative to *http://localhost:7071/api*

Method | HTTP request | Description
------------- | ------------- | -------------
[**AccountIdGet**](DefaultApi.md#accountidget) | **GET** /account/{id} | Lists the account and recent transactions.
[**AccountPut**](DefaultApi.md#accountput) | **PUT** /account | Reserves the next account with a new account number, name, and starting balance.
[**AccountsGet**](DefaultApi.md#accountsget) | **GET** /accounts | Lists the financial accounts.

<a name="accountidget"></a>
# **AccountIdGet**
> InlineResponse2001 AccountIdGet (string id)

Lists the account and recent transactions.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AccountIdGetExample
    {
        public void main()
        {
            var apiInstance = new DefaultApi();
            var id = id_example;  // string | The account id for programmatic access.

            try
            {
                // Lists the account and recent transactions.
                InlineResponse2001 result = apiInstance.AccountIdGet(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DefaultApi.AccountIdGet: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **string**| The account id for programmatic access. | 

### Return type

[**InlineResponse2001**](InlineResponse2001.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="accountput"></a>
# **AccountPut**
> InlineResponse2001 AccountPut (Body body)

Reserves the next account with a new account number, name, and starting balance.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AccountPutExample
    {
        public void main()
        {
            var apiInstance = new DefaultApi();
            var body = new Body(); // Body | The minimal information to reserve the next accounts.

            try
            {
                // Reserves the next account with a new account number, name, and starting balance.
                InlineResponse2001 result = apiInstance.AccountPut(body);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DefaultApi.AccountPut: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **body** | [**Body**](Body.md)| The minimal information to reserve the next accounts. | 

### Return type

[**InlineResponse2001**](InlineResponse2001.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
<a name="accountsget"></a>
# **AccountsGet**
> InlineResponse200 AccountsGet ()

Lists the financial accounts.

### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class AccountsGetExample
    {
        public void main()
        {
            var apiInstance = new DefaultApi();

            try
            {
                // Lists the financial accounts.
                InlineResponse200 result = apiInstance.AccountsGet();
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling DefaultApi.AccountsGet: " + e.Message );
            }
        }
    }
}
```

### Parameters
This endpoint does not need any parameter.

### Return type

[**InlineResponse200**](InlineResponse200.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)
