using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.Exceptions
{
    public class DataAccessException : Exception
    {
        public DataAccessException(Exception ex , string CustomMessage , ILogger logger)
        {
            logger.LogError($"Exception : {ex.Message} and Custom Message : {CustomMessage}");
        }
    }
}
