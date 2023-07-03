using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;

namespace Fastec.MyFastec.Service;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services);

        var app = builder.Build();
        Configure(app);
        app.Run();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddElsa(elsa =>
        {
            //TODO: Replace placeholder
            elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef => ef.UseSqlServer("")));

            elsa.UseWorkflowsApi();

            elsa.UseHttp();

            elsa.UseIdentity(identity =>
            {
                identity.UseAdminUserProvider();
                identity.TokenOptions = options => options.SigningKey = "secret-token-signing-key";
            });

            elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());
        });

        services.AddRazorPages();
        services.AddControllers();
    }

    public static void Configure(WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseWorkflowsApi();
        app.UseWorkflows();
        app.MapControllers();
        app.MapRazorPages();
    }
}