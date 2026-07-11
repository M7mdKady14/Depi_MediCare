using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.DTOs.UserDTOs
{
    public class UserResultDTO 
    {
        public bool success { get; set; }
        public string token {  get; set; }
        public IEnumerable<string> errors { get; set; }
    }
}
