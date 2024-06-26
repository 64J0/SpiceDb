﻿using Google.Protobuf.WellKnownTypes;

namespace SpiceDb;
internal static class Util
{
    public static Authzed.Api.V1.ZedToken? ToAuthzedToken(this SpiceDb.Models.ZedToken? token) => token is null ? null : new Authzed.Api.V1.ZedToken { Token = token.Token };
    public static SpiceDb.Models.ZedToken? ToSpiceDbToken(this Authzed.Api.V1.ZedToken? token) => token is null ? null : new SpiceDb.Models.ZedToken(token.Token);

    public static Dictionary<string, object> FromStruct(this Struct s)
    {
        var ps = new Dictionary<string, object>();

        foreach (var key in s.Fields.Keys)
        {
            if (key is null) continue;

            ps.Add(key, s.Fields[key]);
        }

        return ps;
    }

    public static Struct ToStruct(this Dictionary<string, object> dict)
    {
        var ps = new Struct();
        foreach (var pair in dict)
        {
            string key = pair.Key;
            object value = pair.Value;
            Value pValue = value switch
            {
                string s => new Value { StringValue = s },
                bool b => new Value { BoolValue = b },
                double d => new Value { NumberValue = d },
                int i => new Value { NumberValue = i },
                long l => new Value { NumberValue = l },
                uint u => new Value { NumberValue = u },
                null => new Value { NullValue = NullValue.NullValue },
                _ => throw new Exception($"Unsupported type: {value.GetType().Name}"),
            };
            ps.Fields.Add(key, pValue);
        }

        return ps;
    }

}
