﻿using AutoMapper;
using Business.Wrappers;
using Common.Constants;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Auth.Commands.AuthRegister
{
    public class AuthRegisterHandler : IRequestHandler<AuthRegisterCommand, Response>
    {
        private readonly UserManager<Common.Entities.User> _userManager;
        private readonly IMapper _mapper;

        public AuthRegisterHandler(UserManager<Common.Entities.User> userManager,
                                   IMapper mapper
            )
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Response> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await new AuthRegisterCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
                throw new ValidationException("Email already exists.");

            user = _mapper.Map<Common.Entities.User>(request);

            var registerResult = await _userManager.CreateAsync(user, request.Password);
            if (!registerResult.Succeeded)
                throw new ValidationException(registerResult.Errors.Select(x => x.Description));

            var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
            if (!addToRoleResult.Succeeded)
                throw new ValidationException(addToRoleResult.Errors.Select(x => x.Description));

            return new Response()
            {
                Message = "User added successfully"
            };
        }
    }
}