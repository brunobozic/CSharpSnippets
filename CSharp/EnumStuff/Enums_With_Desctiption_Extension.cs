public enum MyEnum
{
    [Description("String 1")]
    V1= 1,
    [Description("String 2")]
    V2= 2
} 

public static class MyEnumExtensions
{
    public static string ToDescriptionString(this MyEnum val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
} 


MyEnum myLocal = MyEnum.V1;
print(myLocal.ToDescriptionString());