using System.Reflection;
using System.Text;

namespace Cube_Playground
{
    internal static class Utils
    {
        public static string Refactor(Type t1, Type t2)
        {
            List<PropertyInfo> t1Props = t1.GetProperties().ToList();
            List<PropertyInfo> t2Props = t2.GetProperties().ToList();
            List<PropertyInfo> commonProps = new();

            foreach (PropertyInfo prop in t1Props)
            {
                if (t2Props.Any(x => x.Name == prop.Name))
                {
                    commonProps.Add(prop);
                }
            }

            StringBuilder sb = new();
            foreach (PropertyInfo prop in commonProps)
            {
                sb.Append($"public {prop.PropertyType.Name} {prop.Name} {{ get; set; }}\r\n");
            }

            return sb.ToString();
        }
    }
}
