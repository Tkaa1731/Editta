﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Pobytne.Client.Authentication;
using Pobytne.Shared.Authentication;
using System.Security.Claims;

namespace Pobytne.Client.Services
{
	public class AuthenticationService(ILocalStorageService storageService, PobytneService pobytneService, IServiceProvider sp)
    {
		private readonly ILocalStorageService _storageService = storageService;
		private readonly PobytneService _pobytneService = pobytneService;
		private readonly IServiceProvider sp = sp;

        public async Task<object?> Login(LoginRequest logRequest)
		{
			var logResponse = await _pobytneService.LoginAsync(logRequest);
			if (logResponse is UserAccount user)
			{
				await SaveUserAccount(user);
				(sp.GetService<AuthenticationStateProvider>() as CustomAuthenticationStateProvider)!.UpdateAuthenticationState();
			}
			return logResponse;
		} // update Token
		public async Task<bool> Refresh()
		{
			var account = await ReadUserAccount();
			if (account is null)
				return false;
			var logResponse = await _pobytneService.RefreshAsync(new RefreshRequest() { UserId = account.User.Id, RefreshToken = account.Refresh });
			if (logResponse is UserAccount user)
			{
				await SaveUserAccount(user);
				(sp.GetService<AuthenticationStateProvider>() as CustomAuthenticationStateProvider)!.UpdateAuthenticationState();
				return true;
			}
            await RemoveUserAccount();
            (sp.GetService<AuthenticationStateProvider>() as CustomAuthenticationStateProvider)!.UpdateAuthenticationState();
            return false;
		}
		public async Task Logout()
		{
			await Revoke();// odhlas ze serveru a odstran ze storage
			(sp.GetService<AuthenticationStateProvider>() as CustomAuthenticationStateProvider)!.UpdateAuthenticationState();// odhlas od klienta

		}
        public async Task Revoke()
		{
			var user = await ReadUserAccount();
			if (user is not null)
			{
				await RemoveUserAccount();
				await _pobytneService.RevokeAsync(new RefreshRequest() { UserId = user.User.Id, RefreshToken = user.Refresh });
			}
		}
		public async Task<UserAccount?> GetValidUser()
		{
			var user = await ReadUserAccount();
			if (user is null)
				return null;
			if (user.ExpiryTimeStamp < DateTime.UtcNow)
			{
				if (await Refresh()) return await GetValidUser();// refresh se podaril
				return null;
			}
			return user;
		}
		public async Task<string> GetToken() // pri vraceni do hlavicky
		{
			var result = string.Empty;

			var user = await ReadUserAccount();
			if (user != null)
				result = user.Token;

			return result;
		}
		public async Task<TimeSpan> ExpiresIn()
		{
			var user = await ReadUserAccount();
			if (user is null)
			{
				return TimeSpan.Zero;
			}
			return user.ExpiryTimeStamp - DateTime.UtcNow;
		}
		// storage handler
		private async Task RemoveUserAccount()
		{
			await _storageService.RemoveItemAsync(LocalStorageService.USER_SESSION);
		}
		private async Task SaveUserAccount(UserAccount user)
		{
			await _storageService.SaveItemEncrypted(LocalStorageService.USER_SESSION, user);
		}
		private async Task<UserAccount?> ReadUserAccount()
		{
			var user = await _storageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);
			if (user is null) return null;
			return user;
		}
	}
}