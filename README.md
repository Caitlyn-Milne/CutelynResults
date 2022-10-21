*last updated: 19/10/2022*

CutelynResults creates away to manage exceptions in C# while enforcing exception handling. Heavily inspired by Rust's results, a result can either be `Success` or `Error`. An `Error` here contains an Exception. `Success<T>` also can contain a value. 

The benefits of using IResults is it requires you to check if the result is successful or not before being able to get the success value. Prompting handling of the error cases. 

## Basic Usage

### Returning IResult
Returning a result from a function is as simple as calling `IResult.Success(VALUE)` where your function wants to successfully return a value, and `IResult.Exception(EXCEPTION)` where an error arises. 
```cs
public IResult<HttpContent> IoCall()
{
	...
	//success case
	if(Response.IsSuccessStatusCode)
	{
		return IResult.Success(Response.Content);
	}
	//error case 
	return IResult.Error<HttpContent>(new IoException());
	...
}
```

### Handling IResult
Using c# pattern matching you can check wether a result is `ISuccess` or `IError` ,  and then get the `.Value` or `.Exception` from these casted variables.

```cs
public void UserRequestsIoCall()
{
	IResult<HttpContent> requestResult = IoCall();
	//on error
	if(requestResult is IError requestError){
		var exception = requestError.Exception;
		Console.WriteLine(exception.Message)
		return;
	}
	 //on success
	var requestSuccess = (ISuccess<HttpContent>) requestResult;
	var requestContent = requestSuccess.Value;
	...
}
```

## Unwrap
Unwrap allows you to declare a result is successful and grab the value. If you call `Unwrap` on an `IError`, it will throw that error's exception instead.  

Below `ProcessResult` and `ProcessResultUnwrap` are functionally equalient
```cs
public void ProcessResult()
{
	var result = AlwaysSuccessful();
	if(result is IError error)
	{
		throw error.Exception;
	}
	var val = ((ISuccess<string>)result).Value;
	...
}

public void ProcessResultUnwrap()
{
	var val = AlwaysSuccessful().Unwrap();
	...
}

public IResult<string> AlwaysSuccessful()
{
	return IResult.Success("Hello world");
}
```

Use `Unwrap` where you know the result is `ISuccess` (aka you've checked its not `IError`) or when you are wanting to throw an exception if the result is an error. 

## Convert
Frequently when working with IResults, you will have a result of a type, and you want to return a result of a different type. Convert allows you to do this.

### Parsing success value
If you have a result, that if successful you want to parse that data, but if the result is an error you want to return an error, you can do this with convert. 

Below you can see a convertion from `IResult<byte[]> to IResult<string>`
```cs
public IResult<string> ReadString()
{
	IResult<byte[]> bytesResult = Read();
	IResult<string> stringResult = bytesResult
		.Convert(bytes =>  Encoding.Unicode.GetBytes(bytes));
	return stringResult;
}
public IResult<byte[]> Read()
{
	...
	if(failedToRead)
	{
		return IResult.Error<byte[]>(new Exception());
	}
	return IResult.Success(bytes);
	...
}
```

### Where the convertion might error.
In the standard convert function shown above throwing an error inside the 'convert lambda' will return a `IError`, however, the recommended function to use for this problem is `.ConvertShaky(Func<T,IResult<A>> convertionBlock)`. ConvertShaky runs the given lambda when the result is Success, but expects you to return a Result, `IError` if the convertion fails, `ISucess` if it suceeds.

In the below example the convertion might fail if the input has less than 10 bytes. 
```cs
public IResult<string> ReadString()
{
	IResult<byte[]> bytesResult = Read();
	IResult<string> stringResult = bytesResult.ConvertShaky(bytes => 
	{
		if(bytes.Length < 10)
		{
			return IResult.Error<string>
			(
				new FormatException("bytes length is too short")
			);
		}
			
		return IResult.Success(Encoding.Unicode.GetBytes(bytes));
	});
	return stringResult;
}

public IResult<byte[]> Read()
{
	...
	if(failedToRead)
	{
		return IResult.Error<byte[]>(new Exception());
	}
	return IResult.Success(bytes);
	...
}
```

### Converting an error
If you already know the value is an error, you dont need to worry about the success case. In CutelynResults, `IError<Foo>` and `IError<Bar>` are not equaliant even though they hold the same data. (*If you know of a way to fix this in C# please submit a pull request*). However, you can easily convert `IError<Foo>` to `IError<Bar>` with the `IError.Convert<T>()`
```cs
public IResult<string> ReadString()
{
	IResult<byte[]> result = Read();
	if(result is IError error)
	{
		return error.Convert<string>();
	}
	...
}

public IResult<byte[]> Read()
{
	...
	if(failedToRead)
	{
		return IResult.Error<byte[]>(new Exception());
	}
	return IResult.Success(bytes);
	...
}

```

## Try
The try block runs the given lambda inside a try block. If the code block doesnt catch it will return an `ISuccess<T>` where T is the return value. If it catches it will instead return `IError` with the thrown exception.

Throwing and catching exceptions can be very inefficent in C#, and so IResult.Try should be avoided in general, but it is a great way of converting external code into a IResult.  

The below example is a function that gets the name from a json string, if the name property exsits. `JObject.Parse` throws exceptions if the input is not in the json format. 
```cs
public IResult<string> getName(string json) => IResult.Try(() =>
{
	var jObject = JObject.Parse(json);
	string? name = jObject.GetValue("name").Value<string>();
	if(name is null)
	{
		throw new FormatException();
	}
	return name;
});

```

## OnSuccess OnError
As the name suggests, on `OnSuccess(Action<T> actionBlock)` and `OnError(Action<Exception> actionBlock)` runs a given lambda on success or on error.

```cs
public void ReadAndLog(){
	IResult<string> result = ReadString();
	result
	.OnSuccess(str => 
	{
		Console.WriteLine(
			$"successfully read file, content:\n\r{str}");
	})
	.OnError(exception =>
	{
		Console.WriteLine(
			$"failed to read file, message:\n\r{exception.message}");
	});
}

```