using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ABPExample.EntityFramework.Common
{
    public  class DbCommandInterceptor:IObserver<KeyValuePair<string,object>>
    {
        public void OnCompleted()
        {
            //
        }

        public void OnError(Exception error)
        {
            //
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            var (pairKey, pairValue) = value;
            if (pairKey != RelationalEventId.CommandExecuting.Name) return;

            var command = ((CommandEventData)pairValue).Command;

            Console.WriteLine("");
            Console.WriteLine(command.CommandText);
            Console.WriteLine("");
        }
    }

    public class CommandListener:IObserver<DiagnosticListener>
    {
        private readonly DbCommandInterceptor _dbCommandInterceptor = new DbCommandInterceptor();

        public void OnCompleted()
        {
            //
        }

        public void OnError(Exception error)
        {
            //
        }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == DbLoggerCategory.Name) value.Subscribe(_dbCommandInterceptor);
        }
    }
}
