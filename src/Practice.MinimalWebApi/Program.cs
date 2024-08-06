var builder = WebApplication.CreateBuilder(args);

builder.AddFluentValidationEndpointFilter();
builder.Services.AddOpenApi();
builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true); // minimal api 바인딩 실패시 exception
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddPostEcho();


var app = builder.Build();

app.UseExceptionHandler();
app.UseSwaggerUI(static o => o.SwaggerEndpoint("/openapi/v1.json", "v1"));
app.MapOpenApi();

var api = app.MapGroup("").AddFluentValidationFilter();



api.MapPostEcho();
app.Run();
