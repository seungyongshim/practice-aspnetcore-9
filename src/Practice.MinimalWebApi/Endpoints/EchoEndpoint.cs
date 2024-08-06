global using Practice.MinimalWebApi.Endpoints;
using FluentValidation;
using static Practice.MinimalWebApi.Endpoints.InFile;

namespace Practice.MinimalWebApi.Endpoints;
public static class EchoAsyncEndpoint
{
    public static IServiceCollection AddPostEcho(this IServiceCollection services)
       => services.AddSingleton<IValidator<Request>, RequestValidator>();


    public static IEndpointConventionBuilder MapPostEcho(this IEndpointRouteBuilder api) =>
         api.MapPost("/v1/echo", EchoAsync)
           .Produces<Response>(StatusCodes.Status201Created)
           .ProducesValidationProblem()
           .ProducesProblem(401, "application/problem+json")
           .ProducesProblem(422)
           .ProducesProblem(429)
           .WithSummary("Echo")
           .WithDescription("Echo")
           .WithTags("Echo")
           .WithOpenApi();

}

file static class InFile
{
    public static async ValueTask<IResult> EchoAsync
    (
       HttpContext context,
       Request dto
    )
    {
        await Task.Delay(10).ConfigureAwait(false);
        return Results.Ok(new Response { Echo = dto.Echo });
    }
}


file record Request
{
    public required string Echo { get; init; }
}

file record Response
{
    public required string Echo { get; init; }
}

file class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x).Custom(static (dto, context) =>
        {
            if (dto.Echo is "")
            {
                context.AddFailure("echo", "is empty, Not supported.");
            }


            if (dto.Echo is null)
            {
                context.AddFailure("echo", "is null, Not supported.");
            }

            
            if (dto.Echo is "a")
            {
                context.AddFailure("echo", "is a, Not supported.");
            }
        });
    }
}
