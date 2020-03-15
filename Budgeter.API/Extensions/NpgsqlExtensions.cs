using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.API.Extensions
{
    public static class NpgsqlExtensions
    {
        public static NpgsqlCommand AddParam<T>(this NpgsqlCommand cmd, string name, T value)
        {
            cmd.Parameters.Add(new NpgsqlParameter<T>(name, value));
            return cmd;
        }
    }
}
