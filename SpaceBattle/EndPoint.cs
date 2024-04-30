using Hwdtech;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ShipNamespace
{
    public class Endpoint
    {
        private static WebApplication? webApplication;
        private static Action? setCurrentScope;

        public static void InitScope(Action newScope)
        {
            setCurrentScope = newScope;
        }
        public static void InitAndStart()
        {
            var webApplicationBuilder = WebApplication.CreateBuilder();
            webApplication = webApplicationBuilder.Build();
            webApplication.UseHttpsRedirection();
            webApplication.MapPost("/message", (Message message) =>
            {
                if (setCurrentScope != null)
                    setCurrentScope();

                try
                {

                    if (Guid.TryParse(message.Fields["game id"].ToString(), out var gameId))
                    {
                        var fakeCommand = IoC.Resolve<IComand>("executeFakeCommand");
                        IoC.Resolve<IComand>("Send Command", gameId, fakeCommand).Execute();
                    }

                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest();
                }
            });
            webApplication.RunAsync("http://localhost:5050");
        }
    } 
}
