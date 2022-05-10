﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common
{
    public class SozlukConstants
    {
        public const string RabbitMqHost = "localhost";
        public const string DefaultExchangeType = "direct";

        public const string UserExchangeName = "UserExchange";
        public const string UserEmailChangedQueueName = "UserEmailChangedQueue";
        
        
        public const string FavEchangeName= "FavEchangeName";
        public const string CreateEntryCommentFavQueueName= "CreateEntryCommentFavQueueName";

    }
}
