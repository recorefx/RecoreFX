---
uid: Recore.Result`2
example:
- *content
---

@Recore.Result`2 is basically a reskin of
@Recore.Either`2 with some additional semantics.

To explain the use case, let's think about exception-based and status-code-based error handling. (If you haven't read it before, I strongly recommend Joe Duffy's [blog post on the topic](http://joeduffyblog.com/2016/02/07/the-error-model/).)

For an example of exception-based handling, think of @System.Int32.Parse(System.String).
For an example of exception-based handling, think of [`Int32.TryParse(String, out Int32)`](https://docs.microsoft.com/en-us/dotnet/api/system.int32.tryparse).

Basically, the debate looks like this:

<table>
<thead>
    <tr>
        <th></th>
        <th>Pros</th>
        <th>Cons</th>
    </tr>
</thead>
<tbody>
    <tr>
        <td>Status codes</td>
        <td>
            <ul>
                <li>Low perf hit</li>
                <li>Better choice for when errors are expected, like when validating untrusted data</li>
            </ul>
        </td>
        <td>
            <ul>
                <li>Mess up the method signature</li>
                <li>Can be ignored or forgotten about</li>
            </ul>
        </td>
    </tr>
    <tr>
        <td>Exceptions</td>
        <td>
            <ul>
                <li>Let code focus on happy-path logic and push error handling up into another layer</li>
                <li>Can't be ignored, preventing unsafe execution of the program</li>
            </ul>
        </td>
        <td>
            <ul>
                <li>Horribly slow when thrown (dominated by collecting the stack trace)</li>
                <li>Can be forgotten about, leading to your users seeing internal error messages or introducing bugs in the error flow</li>
            </ul>
        </td>
    </tr>
</tbody>
</table>

As you can tell, exceptions are great for handling *fatal* errors.
But what about *recoverable* errors?
Status codes are a better choice, but are a bit primitive.

This is where @Recore.Result`2 comes in.
It has the same pros as status codes, but addresses the cons:
- Less disruptive to the method signature; just return @Recore.Result`2
- Can't get to the value unless you handle the error case

For an example, imagine you're calling [`HttpClient.GetAsync()`](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.getasync?view=netcore-3.1#System_Net_Http_HttpClient_GetAsync_System_String_) to fetch some JSON data and deserialize it. Its signature is

```cs
Task<HttpResponseMessage> GetAsync(string requestUri);
```

With exceptions, you could implement it like this:

```cs
async Task<Person> GetPersonAsync(int id)
{
    var response = await httpClient.GetAsync($"/api/v1/person/{id}");
    if (!response.IsSuccessStatusCode)
    {
        throw new HttpRequestException(response.StatusCode.ToString());
    }

    var json = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<Person>(json);
}
```

But HTTP calls fail for all kinds of reasons.
Who handles that exception?
How do you decide to retry?
How do you report the error to the user?

A better choice is to go with @Recore.Result`2:

```cs
async Task<Result<Person, HttpStatusCode>> GetPersonAsync(int id)
{
    var response = await httpClient.GetAsync($"/api/v1/person/{id}");
    if (response.IsSuccessStatusCode)
    {
        var json = await response.Content.ReadAsStringAsync();
        var person = JsonSerializer.Deserialize<Person>(json);
        return Result.Success<Person, HttpStatusCode>(person);
    }
    else
    {
        return Result.Failure<Person, HttpStatusCode>(response.StatusCode);
    }
}
```

If you're working with an API that returns a special error response in case of errors [(such as GitHub)](https://developer.github.com/v3/#client-errors), you can even do this:

```cs
async Task<Result<Person, Error>> GetPersonAsync(int id)
{
    // ...
    else
    {
        var json = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<Error>(json);
        return Result.Failure<Person, HttpStatusCode>(error);
    }
}
```

Anyway, let's say we just pass on the status code.
Downstream code can then handle the error like this:

```cs
async Task<bool> GetPersonAndPrint(int id)
{
    bool retry = false;
    var personResult = await GetPersonAsync(id);
    personResult.Switch(
        person => Console.WriteLine(person),
        status =>
        {
            if ((int)status >= 500)
            {
                retry = true;
            }
            else
            {
                Console.Error.WriteLine($"Fatal error: {status}");
            }
        });

    return retry;
}
```