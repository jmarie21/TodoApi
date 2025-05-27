using TodoApi.Data;

namespace TodoApi.Endpoints
{
    public static class UserEndpoints
    {
        public static RouteGroupBuilder MapUserEndPoints(this WebApplication app)
        {
            var group = app.MapGroup("/auth");

            group.MapGet("/register", () => "Hello Updated World");

            return group;
        }
    }
}
