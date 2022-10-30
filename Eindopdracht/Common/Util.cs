using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common;

public static class Util
{
    private static readonly string PathDir =
        Path.Combine(Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("Eindopdracht", StringComparison.Ordinal)),
            "Eindopdracht\\Common\\Packets\\");

    public static byte[] Concat(byte[] b1, byte[] b2, int count)
    {
        var r = new byte[b1.Length + count];
        Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
        Buffer.BlockCopy(b2, 0, r, b1.Length, count);
        return r;
    }

    public static void WriteJson(JObject o, string filename)
    {
        var sw = new StreamWriter(PathDir + filename);
        var stream = new JsonTextWriter(sw);
        o.WriteTo(stream);
        stream.Close();
        sw.Dispose();
    }

    public static JObject GetJson(string filename)
    {
        var sr = new StreamReader(PathDir + filename);
        var stream = new JsonTextReader(sr);
        JObject data = (JObject)JToken.ReadFrom(stream);
        stream.Close();
        sr.Dispose();
        return data;
    }
    
    public static JObject? SendReplacedObject<TR, TO>(string variable, TR replacement, int position, TO targetObject)
    {
        Log.Send().Information(PathDir);
        
        var data = targetObject switch
        {
            string => (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(PathDir + targetObject))),
            JObject jObject => jObject,
            _ => null
        };

        var currentObject = data;

        for (var i = 0; i < position; i++)
        {
            currentObject = ((JObject?)currentObject!["data"])!;
        }

        switch (replacement)
        {
            case string s:
                currentObject![variable] = s;
                break;
            case int i:
                currentObject![variable] = i;
                break;
            case string[] sArray:
                var jsArray = JArray.FromObject(sArray);
                currentObject![variable] = jsArray;
                break;
            case bool b:
                currentObject![variable] = b;
                break;
        }
        return data;
    }
}