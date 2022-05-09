using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand,Guid>
    {
        private readonly IMapper mapper; 
        private readonly IUserRepository userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetByIdAsync(request.Id);
            if (dbUser is null)
                throw new DataBaseValidationException("User not Found");
            mapper.Map(request, dbUser);
            
            var dbEmailAddress=dbUser.EmailAddress;
            var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

            var rows = await userRepository.UpdateAsync(dbUser);

            if (emailChanged && rows > 0)
            {
                var @event = new UserEmailChangedEvents()
                {
                    OldEmailAdress = null,
                    NewEmailAdress = dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange(exchangeName: SozlukConstants.UserExchangeName, exchangeType: SozlukConstants.DefaultExchangeType,
                                                   queueName: SozlukConstants.UserEmailChangedQueueName,
                                                   obj: @event);
                dbUser.EmailConfirmed = false;
                await userRepository.UpdateAsync(dbUser);   
            }
            return dbUser.Id;
        }
    }
}
