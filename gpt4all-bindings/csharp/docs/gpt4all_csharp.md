# GPT4All C# API

# Types
| | |
|-|-|
|`class`|[Gpt4All.PredictRequestOptionsExtensions](#Gpt4AllPredictRequestOptionsExtensions)|
|`class`|[Gpt4All.Gpt4All](#Gpt4AllGpt4All)|
|`class`|[Gpt4All.DefaultPromptFormatter](#Gpt4AllDefaultPromptFormatter)|
|`class`|[Gpt4All.Gpt4AllModelFactory](#Gpt4AllGpt4AllModelFactory)|
|`interface`|[Gpt4All.IGpt4AllModel](#Gpt4AllIGpt4AllModel)|
|`interface`|[Gpt4All.IGpt4AllModelFactory](#Gpt4AllIGpt4AllModelFactory)|
|`interface`|[Gpt4All.IPromptFormatter](#Gpt4AllIPromptFormatter)|
|`class`|[Gpt4All.ModelFileUtils](#Gpt4AllModelFileUtils)|
|`class`|[Gpt4All.ModelOptions](#Gpt4AllModelOptions)|
|`enum`|[Gpt4All.ModelType](#Gpt4AllModelType)|
|`interface`|[Gpt4All.ITextPrediction](#Gpt4AllITextPrediction)|
|`interface`|[Gpt4All.ITextPredictionResult](#Gpt4AllITextPredictionResult)|
|`interface`|[Gpt4All.ITextPredictionStreamingResult](#Gpt4AllITextPredictionStreamingResult)|
|`class`|[Gpt4All.PredictRequestOptions](#Gpt4AllPredictRequestOptions)|
|`class`|[Gpt4All.TextPredictionResult](#Gpt4AllTextPredictionResult)|
|`class`|[Gpt4All.TextPredictionStreamingResult](#Gpt4AllTextPredictionStreamingResult)|
|`interface`|[Gpt4All.LibraryLoader.ILibraryLoader](#Gpt4AllLibraryLoaderILibraryLoader)|
|`class`|[Gpt4All.LibraryLoader.LoadResult](#Gpt4AllLibraryLoaderLoadResult)|
|`class`|[Gpt4All.LibraryLoader.NativeLibraryLoader](#Gpt4AllLibraryLoaderNativeLibraryLoader)|
|`interface`|[Gpt4All.Bindings.ILLModel](#Gpt4AllBindingsILLModel)|
|`class`|[Gpt4All.Bindings.ModelResponseEventArgs](#Gpt4AllBindingsModelResponseEventArgs)|
|`class`|[Gpt4All.Bindings.ModelPromptEventArgs](#Gpt4AllBindingsModelPromptEventArgs)|
|`class`|[Gpt4All.Bindings.ModelRecalculatingEventArgs](#Gpt4AllBindingsModelRecalculatingEventArgs)|
|`class`|[Gpt4All.Bindings.LLModel](#Gpt4AllBindingsLLModel)|
|`class`|[Gpt4All.Bindings.LLModelPromptContext](#Gpt4AllBindingsLLModelPromptContext)|

# Examples
---

## Gpt4All.PredictRequestOptionsExtensions

```csharp
public static class PredictRequestOptionsExtensions
```

[`Gpt4All.PredictRequestOptions`](#Gpt4AllPredictRequestOptions) extension methods
### Methods
#### `ToPromptContext`(Gpt4All.PredictRequestOptions)

```csharp
public static LLModelPromptContext ToPromptContext(PredictRequestOptions opts)
```

Extension method to convert a [`Gpt4All.PredictRequestOptions`](#Gpt4AllPredictRequestOptions) to an [`Gpt4All.Bindings.LLModelPromptContext`](#Gpt4AllBindingsLLModelPromptContext) 
|parameter|type|summary|
|-|-|-|
|opts|`PredictRequestOptions`|


## Gpt4All.Gpt4All

```csharp
public class Gpt4All
```

### Methods
#### `GetPredictionAsync`(System.String,Gpt4All.PredictRequestOptions,System.Threading.CancellationToken)

```csharp
public Task<Gpt4All.ITextPredictionResult> GetPredictionAsync(String text, PredictRequestOptions opts, CancellationToken cancellationToken)
```

|parameter|type|summary|
|-|-|-|
|text|`String`|
|opts|`PredictRequestOptions`|
|cancellationToken|`CancellationToken`|

#### `GetStreamingPredictionAsync`(System.String,Gpt4All.PredictRequestOptions,System.Threading.CancellationToken)

```csharp
public Task<Gpt4All.ITextPredictionStreamingResult> GetStreamingPredictionAsync(String text, PredictRequestOptions opts, CancellationToken cancellationToken)
```

|parameter|type|summary|
|-|-|-|
|text|`String`|
|opts|`PredictRequestOptions`|
|cancellationToken|`CancellationToken`|

#### `Dispose`

```csharp
public Void Dispose()
```



## Gpt4All.DefaultPromptFormatter

```csharp
public class DefaultPromptFormatter
```

### Methods
#### `FormatPrompt`(System.String)

```csharp
public String FormatPrompt(String prompt)
```

|parameter|type|summary|
|-|-|-|
|prompt|`String`|


## Gpt4All.Gpt4AllModelFactory

```csharp
public class Gpt4AllModelFactory
```

### Methods
#### `LoadModel`(System.String)

```csharp
public IGpt4AllModel LoadModel(String modelPath)
```

|parameter|type|summary|
|-|-|-|
|modelPath|`String`|


## Gpt4All.IGpt4AllModel

```csharp
public interface IGpt4AllModel
```


## Gpt4All.IGpt4AllModelFactory

```csharp
public interface IGpt4AllModelFactory
```

### Methods
#### `LoadModel`(System.String)

```csharp
public IGpt4AllModel LoadModel(String modelPath)
```

|parameter|type|summary|
|-|-|-|
|modelPath|`String`|


## Gpt4All.IPromptFormatter

```csharp
public interface IPromptFormatter
```

Formats a prompt
### Methods
#### `FormatPrompt`(System.String)

```csharp
public String FormatPrompt(String prompt)
```

Format the provided prompt
|parameter|type|summary|
|-|-|-|
|prompt|`String`|


## Gpt4All.ModelFileUtils

```csharp
public static class ModelFileUtils
```

### Methods
#### `GetModelTypeFromModelFileHeader`(System.String)

```csharp
public static ModelType GetModelTypeFromModelFileHeader(String modelPath)
```

|parameter|type|summary|
|-|-|-|
|modelPath|`String`|


## Gpt4All.ModelOptions

```csharp
public class ModelOptions
```


## Gpt4All.ModelType

```csharp
public enum ModelType
```

The supported model types

## Gpt4All.ITextPrediction

```csharp
public interface ITextPrediction
```

Interface for text prediction services
### Methods
#### `GetPredictionAsync`(System.String,Gpt4All.PredictRequestOptions,System.Threading.CancellationToken)

```csharp
public Task<Gpt4All.ITextPredictionResult> GetPredictionAsync(String text, PredictRequestOptions opts, CancellationToken cancellation)
```

Get prediction results for the prompt and provided options.
|parameter|type|summary|
|-|-|-|
|text|`String`|
|opts|`PredictRequestOptions`|
|cancellation|`CancellationToken`|

#### `GetStreamingPredictionAsync`(System.String,Gpt4All.PredictRequestOptions,System.Threading.CancellationToken)

```csharp
public Task<Gpt4All.ITextPredictionStreamingResult> GetStreamingPredictionAsync(String text, PredictRequestOptions opts, CancellationToken cancellationToken)
```

Get streaming prediction results for the prompt and provided options.
|parameter|type|summary|
|-|-|-|
|text|`String`|
|opts|`PredictRequestOptions`|
|cancellationToken|`CancellationToken`|


## Gpt4All.ITextPredictionResult

```csharp
public interface ITextPredictionResult
```

### Methods
#### `GetPredictionAsync`(System.Threading.CancellationToken)

```csharp
public Task<System.String> GetPredictionAsync(CancellationToken cancellationToken)
```

|parameter|type|summary|
|-|-|-|
|cancellationToken|`CancellationToken`|


## Gpt4All.ITextPredictionStreamingResult

```csharp
public interface ITextPredictionStreamingResult
```

Represents the result of a streaming text prediction request
### Methods
#### `GetPredictionStreamingAsync`(System.Threading.CancellationToken)

```csharp
public IAsyncEnumerable<System.String> GetPredictionStreamingAsync(CancellationToken cancellationToken)
```

Gets an async enumerable of the tokens produced by the generatio
|parameter|type|summary|
|-|-|-|
|cancellationToken|`CancellationToken`|


## Gpt4All.PredictRequestOptions

```csharp
public class PredictRequestOptions
```

Represente the parameters for a text generation request

## Gpt4All.TextPredictionResult

```csharp
public class TextPredictionResult
```


### Methods
#### `GetPredictionAsync`(System.Threading.CancellationToken)

```csharp
public Task<System.String> GetPredictionAsync(CancellationToken cancellationToken)
```


|parameter|type|summary|
|-|-|-|
|cancellationToken|`CancellationToken`|


## Gpt4All.TextPredictionStreamingResult

```csharp
public class TextPredictionStreamingResult
```


### Methods
#### `GetPredictionAsync`(System.Threading.CancellationToken)

```csharp
public Task<System.String> GetPredictionAsync(CancellationToken cancellationToken)
```


|parameter|type|summary|
|-|-|-|
|cancellationToken|`CancellationToken`|

#### `GetPredictionStreamingAsync`(System.Threading.CancellationToken)

```csharp
public IAsyncEnumerable<System.String> GetPredictionStreamingAsync(CancellationToken cancellationToken)
```


|parameter|type|summary|
|-|-|-|
|cancellationToken|`CancellationToken`|


## Gpt4All.LibraryLoader.ILibraryLoader

```csharp
public interface ILibraryLoader
```

### Methods
#### `OpenLibrary`(System.String)

```csharp
public LoadResult OpenLibrary(String fileName)
```

|parameter|type|summary|
|-|-|-|
|fileName|`String`|


## Gpt4All.LibraryLoader.LoadResult

```csharp
public class LoadResult
```

### Methods
#### `Failure`(System.String)

```csharp
public static LoadResult Failure(String errorMessage)
```

|parameter|type|summary|
|-|-|-|
|errorMessage|`String`|


## Gpt4All.LibraryLoader.NativeLibraryLoader

```csharp
public static class NativeLibraryLoader
```

### Methods
#### `SetLibraryLoader`(Gpt4All.LibraryLoader.ILibraryLoader)

```csharp
public static Void SetLibraryLoader(ILibraryLoader libraryLoader)
```

Sets the library loader used to load the native libraries. Overwrite this only if you want some custom loading.
|parameter|type|summary|
|-|-|-|
|libraryLoader|`ILibraryLoader`|


## Gpt4All.Bindings.ILLModel

```csharp
public interface ILLModel
```

Represents the interface exposed by the universal wrapper for GPT4All language models built around llmodel C-API.
### Methods
#### `GetStateSizeBytes`

```csharp
public UInt64 GetStateSizeBytes()
```

Gets the size of the model state

#### `GetThreadCount`

```csharp
public Int32 GetThreadCount()
```

Get the read count the model is using

#### `SetThreadCount`(System.Int32)

```csharp
public Void SetThreadCount(Int32 threadCount)
```

Set the thread count the model will use
|parameter|type|summary|
|-|-|-|
|threadCount|`Int32`|

#### `IsLoaded`

```csharp
public Boolean IsLoaded()
```

Check if the model is loaded

#### `Load`(System.String)

```csharp
public Boolean Load(String modelPath)
```

Load the model
|parameter|type|summary|
|-|-|-|
|modelPath|`String`|

#### `Prompt`(System.String,Gpt4All.Bindings.LLModelPromptContext,System.Func[Gpt4All.Bindings.ModelPromptEventArgs, System.Boolean],System.Func[Gpt4All.Bindings.ModelResponseEventArgs,System.Boolean],System.Func[Gpt4All.Bindings.ModelRecalculatingEventArgs, System.Boolean],System.Threading.CancellationToken)

```csharp
public Void Prompt(String text, LLModelPromptContext context, Func<int, bool> promptCallback, Func<int, string, bool> responseCallback, Func<bool, bool> recalculateCallbac, CancellationToken cancellationToken)
```

|parameter|type|summary|
|-|-|-|
|text|`String`|
|context|`LLModelPromptContext`|
|promptCallback|`Func<int, bool>`|
|responseCallback|`Func<int, string, bool>`|
|recalculateCallback|`Func<bool, bool>`|
|cancellationToken|`CancellationToken`|

#### `RestoreStateData`(System.Byte*)

```csharp
public UInt64 RestoreStateData(Byte* source)
```

Restore the state of the model
|parameter|type|summary|
|-|-|-|
|source|`Byte*`|

#### `SaveStateData`(System.Byte*)

```csharp
public UInt64 SaveStateData(Byte* destination)
```

Save the current model state
|parameter|type|summary|
|-|-|-|
|destination|`Byte*`|


## Gpt4All.Bindings.ModelResponseEventArgs

```csharp
public class ModelResponseEventArgs
```

Arguments for the response processing callback

## Gpt4All.Bindings.ModelPromptEventArgs

```csharp
public class ModelPromptEventArgs
```

Arguments for the prompt processing callback

## Gpt4All.Bindings.ModelRecalculatingEventArgs

```csharp
public class ModelRecalculatingEventArgs
```

Arguments for the recalculating callback

## Gpt4All.Bindings.LLModel

```csharp
public class LLModel
```

Base class and universal wrapper for GPT4All language models built around llmodel C-API.
### Methods
#### `Prompt`(System.String,Gpt4All.Bindings.LLModelPromptContext,System.Func[Gpt4All.Bindings.ModelPromptEventArgs, System.Boolean],System.Func[Gpt4All.Bindings.ModelResponseEventArgs,System.Boolean],System.Func[Gpt4All.Bindings.ModelRecalculatingEventArgs, System.Boolean],System.Threading.CancellationToken)

```csharp
public Void Prompt(String text, LLModelPromptContext context, Func<int, bool> promptCallback, Func<int, string, bool> responseCallback, Func<bool, bool> recalculateCallback, CancellationToken cancellationToken)
```

|parameter|type|summary|
|-|-|-|
|text|`String`|
|context|`LLModelPromptContext`|
|promptCallback|`Func<int, bool>`|
|responseCallback|`Func<int, string, bool>`|
|recalculateCallback|`Func<bool, bool>`|
|cancellationToken|`CancellationToken`|

#### `SetThreadCount`(System.Int32)

```csharp
public Void SetThreadCount(Int32 threadCount)
```

Set the number of threads to be used by the model.
|parameter|type|summary|
|-|-|-|
|threadCount|`Int32`|

#### `GetThreadCount`

```csharp
public Int32 GetThreadCount()
```

Get  the number of threads used by the model.

#### `GetStateSizeBytes`

```csharp
public UInt64 GetStateSizeBytes()
```

Get the size of the internal state of the model.
> **Note** This state data is specific to the type of model you have created.


#### `SaveStateData`(System.Byte*)

```csharp
public UInt64 SaveStateData(Byte* source)
```

Saves the internal state of the model to the specified destination address.
|parameter|type|summary|
|-|-|-|
|source|`Byte*`|

#### `RestoreStateData`(System.Byte*)

```csharp
public UInt64 RestoreStateData(Byte* destination)
```

Restores the internal state of the model using data from the specified address.
|parameter|type|summary|
|-|-|-|
|destination|`Byte*`|

#### `IsLoaded`

```csharp
public Boolean IsLoaded()
```

Check if the model is loaded.

#### `Load`(System.String)

```csharp
public Boolean Load(String modelPath)
```

Load the model from a file.
|parameter|type|summary|
|-|-|-|
|modelPath|`String`|

#### `Dispose`

```csharp
public Void Dispose()
```

Dispose the model

#### `Create`(System.IntPtr,Gpt4All.ModelType,Microsoft.Extensions.Logging.ILogger)

```csharp
public static LLModel Create(IntPtr handle, ModelType modelType, ILogger logger)
```

Create a new model from a pointer
|parameter|type|summary|
|-|-|-|
|handle|`IntPtr`|
|modelType|`ModelType`|
|logger|`ILogger`|


## Gpt4All.Bindings.LLModelPromptContext

```csharp
public class LLModelPromptContext
```

Wrapper around the llmodel_prompt_context structure for holding the prompt context.
> **Note** The implementation takes care of all the memory handling of the raw logits pointer and theraw tokens pointer.Attempting to resize them or modify them in any way can lead to undefined behavior

