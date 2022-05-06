﻿using AutoMapper;
using BlazorSozluk.Api.Application.Interfaces.Repositories;
using BlazorSozluk.Common.Infrastructure;
using BlazorSozluk.Common.Infrastructure.Exceptions;
using BlazorSozluk.Common.Models.Queries;
using BlazorSozluk.Common.Models.RequestModels;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Api.Application.Features.Commands.User
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetSingleAsync(x => x.EmailAddress == request.EmailAdress);
            if (dbUser == null)
                throw new DataBaseValidationException("User Not found");
            var pass = PasswordEncryptor.Encrypt(request.Password);
            if (dbUser.Password != pass)
                throw new DataBaseValidationException("Password is wrong");
            if (!dbUser.EmailConfirmed)
                throw new DataBaseValidationException("Email Address is not confirmed yet");
            var result=mapper.Map<LoginUserViewModel>(dbUser);

            result.Token = GenerateToken();
        }
        private string GenerateToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfig:Secret"]));
            var cred=new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry=DateTime.Now.AddDays(10);
            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: cred, notBefore: DateTime.Now);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}