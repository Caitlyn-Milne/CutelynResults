*last updated: 19/10/2022*
CutelynResults creates away to manage exceptions in C# while enforcing exception handling. Heavily inspired by Rust's results, a result can either be `Success` or `Error`. An `Error` here contains an Exception. `Success<T>` also can contain a value. 

The benefits of using IResults is it requires you to check if the result is successful or not before being able to get the success value. Prompting handling of the error cases. 

## Basic Usage

### Returning IResult
Returning a result from a function is as simple as calling `IResult.Success(VALUE)` where your function wants to successfully return a value, and `IResult.Exception(EXCEPTION)` where an error arises. 
```cs
public IResult<HttpContent> IoCall(){
	...
	//success case
	if(Response.IsSuccessStatusCode)
	{
		return IResult.Success(Response.Content)
	}
	//error case 
	return IResult.Error(new IoException())
	...
}
```

### Handling I Result
Using c# pattern matching you can check wether a result is `ISuccess` or `IError` ,  and then get the `.Value` or `.Exception` from these casted variables.

```cs
public void UserRequestsIoCall(){
	IResult<HttpContent> requestResult = IoCall();
	//on error
	if(requestResult is IError requestError){
		var exception = requestError.Exception;
		Console.WriteLine(exception.Message)
		return;
	}
	 //on success
	var requestSuccess = (Success<HttpContent>) requestResult;
	var requestContent = requestSuccess.Value;
	...
}
```

*Documentation Incomplete...*