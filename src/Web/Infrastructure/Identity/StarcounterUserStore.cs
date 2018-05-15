using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Starcounter.Linq;
using Starcounter.Nova;

namespace Infrastructure.Identity
{
    public class StarcounterUserStore: IUserPasswordStore<ApplicationUser>
    {
        public void Dispose()
        {
            
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(Db.Transact(() => user.Id.ToString()));
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(Db.Transact(() => user.UserName));
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            Db.Transact(() => user.UserName = userName);
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(Db.Transact(() => user.NormalizedUserName));
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            Db.Transact(() => user.NormalizedUserName = normalizedName);
            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Db.Transact(() => Mapper.Map(user, Db.Insert<ApplicationUser>()));
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Db.Transact(() => Mapper.Map(user, Db.FromId(user.Id)));
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            Db.Transact(() => Db.Delete(Db.FromId(user.Id)));
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var applicationUser = Db.Transact(() => MapToPoco(Db.FromId<ApplicationUser>(Convert.ToUInt64(userId))));
            return Task.FromResult(applicationUser);
        }

        public Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var applicationUser = Db.Transact(() => MapToPoco(
                DbLinq.Objects<ApplicationUser>()
                    .FirstOrDefault(user => user.NormalizedUserName == normalizedUserName)));
            return Task.FromResult(applicationUser);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            Db.Transact(() => user.PasswordHash = passwordHash);
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(Db.Transact(() => user.PasswordHash));
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(Db.Transact(() => user.PasswordHash != null));
        }

        private static ApplicationUser MapToPoco(ApplicationUser dbProxy)
        {
            return Mapper.Map<ApplicationUser>(dbProxy);
        }
    }
}