using System.Diagnostics;
namespace DemoApi;

public static class Tracing
{
    public const string Name = "Api";
    public static ActivitySource Source = new ActivitySource(Name, "1.0.0");
}
