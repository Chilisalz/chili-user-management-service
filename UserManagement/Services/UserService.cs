﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagementService.Contracts.Requests;
using UserManagementService.DataAccessLayer;
using UserManagementService.Dtos;
using UserManagementService.Exceptions;
using UserManagementService.Extensions;
using UserManagementService.Models;
using UserManagementService.Services.ServiceResult;

namespace UserManagementService.Services
{
    public class UserService : IUserService
    {
        private readonly UserManagementContext _context;
        private readonly IMapper _mapper;
        public UserService(UserManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var delUser = await _context.Users.FindByIdAsync(id);
            if (delUser == null)
                throw new UserNotFoundException($"User with id {id} not found");
            _context.Users.Remove(delUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<GetUsersResult> GetAllUsersAsync(int page)
        {
            var users = await _context.Users.Include(u => u.SecretQuestion).Include(u => u.Role).Skip((page - 1) * 10).Take(10).ToListAsync();
            var itemsTotal = _context.Users.Count();
            return new GetUsersResult() { Users = users, Pagination = new Pagination(page, users.Count, itemsTotal) };
        }
        public async Task<ChiliUser> GetChiliUserByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.SecretQuestion).Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id) ?? throw new UserNotFoundException($"User with id {id} not found");
        }
        public async Task<ChiliUserDto> UpdateUserAsync(Guid id, ChiliUserDto request)
        {
            var user = await _context.Users.Include(u => u.SecretQuestion).Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                throw new UserNotFoundException();
            if (await _context.Users.AnyAsync(x => x.UserName == request.UserName && x.Id != id))
                throw new UsernameAlreadyTakenException($"Username {request.UserName} is already used");
            if (await _context.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
                throw new EmailAlreadyTakenException($"Email {request.Email} is already used");

            foreach (var requestUserProperty in request.GetType().GetProperties())
            {
                var value = requestUserProperty.GetValue(request);

                if (value != null)
                {
                    if (value is Guid g && g == Guid.Empty)
                        continue;

                    var userProperty = user.GetType().GetProperty(requestUserProperty.Name);
                    userProperty.SetValue(user, Convert.ChangeType(value, requestUserProperty.PropertyType), null);

                }
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<ChiliUserDto>(user);
        }
        public async Task<bool> ChangePasswordAsync(Guid id, ChangePasswordRequest request)
        {
            var user = await _context.Users.FindByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException($"User with id {id} not found");
            PasswordHasher<ChiliUser> passwordHasher = new();
            if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword) == PasswordVerificationResult.Failed)
                throw new InvalidPasswordException("Invalid password.");

            user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<SecurityQuestion>> GetAllSecurityQuestionsAsync()
        {
            return await _context.SecurityQuestions.ToListAsync();
        }
        public async Task<SecurityQuestion> GetSecurityQuestionOfUserAsync(string email)
        {
            var user = await _context.Users.Include(u => u.SecretQuestion).FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new UserNotFoundException($"User with email {email} not found");
            return user.SecretQuestion;
        }
        public async Task<bool> ValidateSecretAnswerAsync(ValidateSecretAnswerRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null)
                throw new UserNotFoundException($"User with id {request.UserId} not found");
            PasswordHasher<ChiliUser> passwordHasher = new();
            if (passwordHasher.VerifyHashedPassword(user, user.SecretAnswer, request.SecretAnswer) == PasswordVerificationResult.Failed)
                throw new WrongSecretAnswerException($"Wrong secret answer");
            return true;
        }
    }
}
