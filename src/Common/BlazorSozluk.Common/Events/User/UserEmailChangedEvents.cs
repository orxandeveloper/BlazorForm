using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Events.User
{
    public class UserEmailChangedEvents
    {
        public string OldEmailAdress { get; set; }  
        public string NewEmailAdress { get; set; }
    }
}
