﻿using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Events.User;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User.ChangePassword
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, bool>
    {
        private readonly IUserRepository userRepository;
        public ChangeUserPasswordCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.UserId.HasValue)
                throw new ArgumentNullException(nameof(request.UserId));

            var dbUser=await userRepository.GetByIdAsync(request.UserId.Value);
            if (dbUser is null)
                throw new DataBaseValidationException("User not found");

            var encPassword = PasswordEncryptor.Encrypt(request.OldPassword);
            if (dbUser.Password != encPassword)
                throw new DataBaseValidationException("Old Password wrong");
            dbUser.Password= PasswordEncryptor.Encrypt(request.NewPassword); ;
            await userRepository.UpdateAsync(dbUser);

            return true;
        }
    }
}
