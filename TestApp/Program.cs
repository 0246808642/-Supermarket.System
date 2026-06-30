using System;
using System.Linq;
using System.Reflection;

class Program {
    static void Main() {
        var a = Assembly.Load("Swashbuckle.AspNetCore.SwaggerGen");
        var t = a.GetType("Microsoft.Extensions.DependencyInjection.SwaggerGenOptionsExtensions");
        if(t!=null) {
            foreach (var m in t.GetMethods().Where(x => x.Name == "AddSecurityRequirement"))
                Console.WriteLine(m.ToString());
        }
    }
}
