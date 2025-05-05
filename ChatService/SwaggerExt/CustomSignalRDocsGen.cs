using System;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.CustomSwaggerGen;

using System.Collections;
// using ChatService.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;



public class CustomSigRDocsGen : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {

        var hub_root = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<SignalRHubRoot>() != null)
            .ToList().First();
        

        if (hub_root != null)
        {
            var methods = hub_root.GetMethods();
            foreach (var method in methods) {
                var invokableAttribute = method.GetCustomAttribute<InvokableSignalREvent>();
                if (invokableAttribute == null) continue;
                var descriptionAttribute = method.GetCustomAttribute<SignalRCustomEventDescription>();
                var response = method.GetCustomAttribute<EventWithResponse>();
                var parameters = method.GetParameters();
                var newPath = invokableAttribute.EndpointOverride ?? $"/invoke/{method.Name}";

                var op = invokableAttribute.EndpointOverride == null ? OperationType.Head : 
                    method.Name == "OnConnectedAsync" ? OperationType.Trace : OperationType.Get;

                var paramDescriptions = method.GetCustomAttributes<EventArgDescription>();

                var newOperation = new OpenApiOperation
                {
                    OperationId = method.Name,
                    Tags = new List<OpenApiTag> { new OpenApiTag { Name = "SignalR Events" } }
                };

                if (response != null) {
                    newOperation.Responses = new OpenApiResponses
                    {
                        { "event", new OpenApiResponse { Description = $"\"{response.ResponseEventName}\" event will be sent as response to invoker only." } }
                    };
                }
                

                newOperation.Description = $"Event name: {method.Name};\t";
                var endpointSummary = invokableAttribute.Description;
                if (endpointSummary != null) {
                    newOperation.Summary = endpointSummary;
                    newOperation.Description += $"{endpointSummary};\t";
                }
                var endpointDescription = descriptionAttribute?.Description;
                if (endpointDescription != null) newOperation.Description += endpointDescription;

                if (!swaggerDoc.Paths.ContainsKey(newPath))
                {
                    swaggerDoc.Paths.Add(newPath, new OpenApiPathItem());
                }

                swaggerDoc.Paths[newPath].Operations[op] = newOperation;

                foreach (var param in parameters)
                {
                    
                    var parameter = new OpenApiParameter
                    {
                        Name = param.Name,
                        In = ParameterLocation.Query,
                        Schema = SwaggerSchemaGenerator.GetOpenApiSchemaFromType(param.ParameterType),
                        Required = true
                    };
                    if (paramDescriptions != null) {
                        var desc = paramDescriptions.FirstOrDefault(d => d.Param == param.Name);
                        if (desc != null) {
                            parameter.Description = desc.Description;
                        }
                    }
                    if (!swaggerDoc.Paths[newPath].Operations[op].Parameters.Contains(parameter))
                    {
                        swaggerDoc.Paths[newPath].Operations[op].Parameters.Add(parameter);
                    }
                    
                }
                // invoke-response
                // OperationType.Patch -> response
                // you dont need to json.deserialize that event! 

                if (response != null) {
                    var responseEventName = response.ResponseEventName;
                    var responseCustomEventDescription = response.ResponseEventDescription;
                    var responseEventSummary = response.ResponseEventSummary;
                    var responseEventType = response.ResponseEventType;

                    var responsePath = $"/invoke-response/{responseEventName}";
                    var newResponseOperation = new OpenApiOperation
                    {
                        OperationId = responseEventName,
                        Tags = new List<OpenApiTag> { new OpenApiTag { Name = "SignalR Events" } }
                    };
                    newResponseOperation.Description = $"Event name: {responseEventName};\t";
                    if (responseEventSummary != null) {
                        newResponseOperation.Summary = responseEventSummary;
                        newResponseOperation.Description += $"{responseEventSummary};\t";
                    }
                    if (responseCustomEventDescription != null) {
                        newResponseOperation.Description += $"{responseCustomEventDescription}\t";
                    }
                    if (!swaggerDoc.Paths.ContainsKey(responsePath))
                    {
                        swaggerDoc.Paths.Add(responsePath, new OpenApiPathItem());
                    }
                    swaggerDoc.Paths[responsePath].Operations[OperationType.Patch] = newResponseOperation;

                    var schema = SwaggerSchemaGenerator.GetOpenApiSchemaFromType(responseEventType);
                    var parameter = new OpenApiParameter
                    {
                        Name = "<json>",
                        In = ParameterLocation.Query,
                        Schema = schema,
                        Required = true
                    };
                    if (response.TypeDescription != null) {
                        parameter.Description = response.TypeDescription;
                    }
                    if (!swaggerDoc.Paths[responsePath].Operations[OperationType.Patch].Parameters.Contains(parameter))
                    {
                        swaggerDoc.Paths[responsePath].Operations[OperationType.Patch].Parameters.Add(parameter);
                    }
                }
            }
        }
    
        var listenableEventTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetCustomAttribute<ListenableSignalREvent>() != null)
            .ToList();

        foreach (var eventType in listenableEventTypes)
        {
            var eventInstance = Activator.CreateInstance(eventType);
            var eventName = eventType.GetProperties()
                .Where(p => p.GetCustomAttribute<ListenableEventNameAttribute>() != null)
                .Select(p => p.GetValue(eventInstance)?.ToString())
                .FirstOrDefault();


            if (string.IsNullOrEmpty(eventName))
                continue;

            var newPath = $"/listen/{eventName}";

            var newOperation = new OpenApiOperation
            {
                OperationId = eventName,
                Tags = new List<OpenApiTag> { new OpenApiTag { Name = "SignalR Events" } }
            };
            newOperation.Description = $"Event name: {eventName};\t";
            var endpointSummary = eventType.GetCustomAttribute<ListenableSignalREvent>()?.Description;
            if (endpointSummary != null) {
                newOperation.Summary = endpointSummary;
                newOperation.Description += $"{endpointSummary}\t";
            }
            var endpointDescription = eventType.GetCustomAttribute<SignalRCustomEventDescription>()?.Description;
            if (endpointDescription != null) newOperation.Description += endpointDescription;

            if (!swaggerDoc.Paths.ContainsKey(newPath))
            {
                swaggerDoc.Paths.Add(newPath, new OpenApiPathItem());
            }

            swaggerDoc.Paths[newPath].Operations[OperationType.Options] = newOperation;

            foreach (var property in eventType.GetProperties())
            {
                var description = property.GetCustomAttribute<CustomEventDescriptionAttribute>()?.Description;
                if (!string.IsNullOrEmpty(description))
                {

                    var parameter = new OpenApiParameter
                    {
                        Name = property.Name,
                        In = ParameterLocation.Query,
                        Description = description,
                        Schema = SwaggerSchemaGenerator.GetOpenApiSchemaFromType(property.PropertyType),
                        Required = true
                    };
                    var propertyType = property.PropertyType;

                    if (!swaggerDoc.Paths[newPath].Operations[OperationType.Options].Parameters.Contains(parameter))
                    {
                        swaggerDoc.Paths[newPath].Operations[OperationType.Options].Parameters.Add(parameter);
                    }
                }
            }
        }
    }
}



public class SwaggerSchemaGenerator
{
    public static OpenApiSchema GetOpenApiSchemaFromType(Type type)
    {
        var schema = new OpenApiSchema();

        if (type == typeof(int))
        {
            schema.Type = "integer";
            schema.Format = "int32";
        }
        else if (type == typeof(long))
        {
            schema.Type = "integer";
            schema.Format = "int64";
        }
        else if (type == typeof(string))
        {
            schema.Type = "string";
        }
        else if (type == typeof(bool))
        {
            schema.Type = "boolean";
        }
        else if (type == typeof(DateTime))
        {
            schema.Type = "string";
            schema.Format = "date-time";
        }
        else if (type == typeof(decimal))
        {
            schema.Type = "number";
            schema.Format = "decimal";
        }
        else if (type == typeof(float))
        {
            schema.Type = "number";
            schema.Format = "float";
        }
        else if (type.IsEnum)
        {
            schema.Type = "string";
            schema.Enum = (IList<Microsoft.OpenApi.Any.IOpenApiAny>)Enum.GetNames(type).Cast<object>().ToList();
        }
        else if (type == typeof(string[]))
        {
            schema.Type = "array";
            schema.Items = new OpenApiSchema { Type = "string" };
        }
        else if (type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type) && type.GetGenericArguments().Length == 2 && type.GetGenericArguments()[0] == typeof(string))
        {
            var genericArgs = type.GetGenericArguments();
            schema.Type = "object";
            schema.AdditionalPropertiesAllowed = true;
            schema.AdditionalProperties = GetOpenApiSchemaFromType(genericArgs[1]); 
        }
        else if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            var elementType = type.GetGenericArguments().FirstOrDefault();
            if (elementType != null)
            {
                schema.Type = "array";
                schema.Items = GetOpenApiSchemaFromType(elementType);
            }
        }
        else
        {
            schema.Type = "object";
            schema.Properties = GetPropertiesSchemas(type);
        }

        return schema;
    }

    private static IDictionary<string, OpenApiSchema> GetPropertiesSchemas(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var propertySchemas = new Dictionary<string, OpenApiSchema>();

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;
            var schema = GetOpenApiSchemaFromType(propertyType);

            // Add additional logic to capture descriptions, required attributes, etc.
            propertySchemas[property.Name] = schema;
        }

        return propertySchemas;
    }
}



[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ListenableSignalREvent : Attribute
{
    public string? Description { get; }

    public ListenableSignalREvent(string? description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class InvokableSignalREvent : Attribute
{
    public string? Description { get; }
    public string? EndpointOverride { get; }

    public InvokableSignalREvent(string? description)
    {
        Description = description;
    }
    public InvokableSignalREvent(string? description, string? endpointOverride)
    {
        Description = description;
        EndpointOverride = endpointOverride;
    }
}
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class SignalRHubRoot : Attribute
{
    public SignalRHubRoot(){}
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class SignalRCustomEventDescription : Attribute
{
    public string Description { get; }

    public SignalRCustomEventDescription(string description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class ListenableEventNameAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class CustomEventDescriptionAttribute : Attribute
{
    public string Description { get; }

    public CustomEventDescriptionAttribute(string description)
    {
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class EventArgDescription : Attribute
{
    public string Param { get; }
    public string Description { get; }

    public EventArgDescription(string param, string description)
    {
        Param = param;
        Description = description;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class EventWithResponse : Attribute
{
    public string ResponseEventName { get; }
    public string? ResponseEventSummary { get; }
    public string? ResponseEventDescription { get; }
    public Type ResponseEventType { get; }
    public string? TypeDescription{ get; }

    public EventWithResponse(string responseEventName, string? responseEventSummary, string? responseEventDescription, Type responseEventTypes, string? typeDescription) //, params EventResponseArg[] responseEventArgs
    {
        ResponseEventName = responseEventName;
        ResponseEventSummary = responseEventSummary;
        ResponseEventDescription = responseEventDescription;
        ResponseEventType = responseEventTypes;
        TypeDescription = typeDescription;
    }
}

